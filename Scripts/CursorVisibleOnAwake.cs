using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorVisibleOnAwake : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Cursor.visible = true;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        Cursor.lockState = hasFocus ? CursorLockMode.Confined : CursorLockMode.None;
    }
}
