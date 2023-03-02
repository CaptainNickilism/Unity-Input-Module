using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PcInput : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // When is clicked
        {
            InputManager.instance.OnInputStart(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            InputManager.instance.OnInputHold(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0)) // When is released
        {
            InputManager.instance.OnInputEnd(Input.mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InputManager.instance.EscapeAction();
        }
    }
}
