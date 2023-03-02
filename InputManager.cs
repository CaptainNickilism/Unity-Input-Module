using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class InputManager : MonoBehaviour
{
    static public InputManager instance = null;
    public Canvas canvas;                          // Reserve a field for the Canvas object
    public RectTransform viewport;                 // Reference to the actual visible area of the Canvas (needed if Canvas is set to resize)

    [Tooltip("How many seconds have to pass to consider a hold")]
    public float holdTimeThreshold = 0.1f;
    [Tooltip("How many pixels the input has to move for it to be considered a drag")]
    public float dragDistanceThreshold = 5f;

    [SerializeField]private bool isHolding;             // Wether the player is holding or not
    [SerializeField]private bool isDragging;            // Wether the player is dragging or not
    private float inputTime;            // The time in which the input has started
    private Vector2 startPos;           // Logged position on input start
    public Vector2 cursorPos;           // The correct cursor position
    private GameObject loggedObject;    // Logged object on input start


    public bool enableDebugMode;        // If false, none of the Debug messages will be printed in the console

    void Awake()
    {
        if (instance != null && instance != this) // Make sure there is only one instance running at the same time
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    private void Update()
    {
        if (!canvas)
        {
            canvas = FindObjectOfType<Canvas>();                            // Set reference to the Canvas in the scene
            viewport = canvas.gameObject.GetComponent<RectTransform>();     // Set reference to the Canvas viewport
        }
    }

    public void OnInputStart(Vector2 pos)       // Actions to perform on input start
    {
        inputTime = Time.time;                  // Log the current time
        startPos = pos;                         // Log the current cursor position
        loggedObject = GetGameObject(pos);      // Log the object at cursor position      
    }

    public void OnInputHold(Vector2 pos)        // Actions to perform each frame while input is being held down
    {
        cursorPos = GetCursorPosition(pos);
        if (Mathf.Abs(pos.x - startPos.x) >= dragDistanceThreshold || Mathf.Abs(pos.y - startPos.y) >= dragDistanceThreshold) // If drag distance threshold is reached
        {
            if (!isDragging)
            {
                isHolding = false;
                isDragging = true;
                InteractionManager.instance.StartDragAction(loggedObject); // Add check if it's draggable
            }
            else
            {
                InteractionManager.instance.DragAction(loggedObject);     
            }           
        }
        else if (Time.time - inputTime >= holdTimeThreshold) // If the hold time threshold is reached
        {
            if (!isHolding)
            {
                isDragging = false;
                isHolding = true;
                InteractionManager.instance.StartHoldAction(loggedObject);// Add check if it's holdable
            }
            else
            {
                InteractionManager.instance.HoldAction(loggedObject);   
            }   
        }
    }

    public void OnInputEnd(Vector2 pos)         // Actions to perform on input end
    {
        if (isHolding)
        {
            isHolding = false;
            InteractionManager.instance.StopHoldAction(loggedObject);
        }
        else if (isDragging)
        {
            isDragging = false;
            InteractionManager.instance.StopDragAction(GetGameObject(pos));
        }
        else
        {
            InteractionManager.instance.ClickAction(loggedObject);  // Add check if clickable
        }
    }

    public void EscapeAction()  // Actions to perform when pressing ESC
    {
        MenuManager.instance.TogglePause();
    }

    private GameObject GetGameObject(Vector2 pos)   // Raycast on cursor position and return the gameObject in that location
    {
        RaycastHit2D raycast = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
        if (raycast) //If the raycast hits a collider
        {
            return raycast.collider.gameObject;
        }
        else
        {
            if(enableDebugMode) Debug.LogWarning("No ray created");
            return null;
        }
    }

    public Vector2 GetCursorPosition(Vector2 pos)
    {
        Vector2 newPos;
        newPos.x = pos.x / canvas.scaleFactor - viewport.rect.size.x / 2;
        newPos.y = pos.y / canvas.scaleFactor - viewport.rect.size.y / 2;
        return newPos;
    }



}
