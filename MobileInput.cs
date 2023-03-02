using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                InputManager.instance.OnInputStart(Input.GetTouch(0).position);
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                InputManager.instance.OnInputHold(Input.GetTouch(0).position);
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                InputManager.instance.OnInputEnd(Input.GetTouch(0).position);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                InputManager.instance.EscapeAction();
            }
        }    
    } 
}
