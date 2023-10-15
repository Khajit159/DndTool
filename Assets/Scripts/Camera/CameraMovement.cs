using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public float DefaultSpeed = 1;
    [SerializeField] float shiftMultiplier = 2f;
    public float ZoomScrollSpeed = 0.5f;
    private EventSystem system;

    private Camera cam;

    public bool BlockScrolling = false;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        system = EventSystem.current;
    }
    void Update()
    {
        bool allow = true;
        if (system == null)
        {
            system = EventSystem.current;
            if (system == null) return;
        }
        GameObject currentObject = system.currentSelectedGameObject;
        if (currentObject != null)
        {
            InputField inputField = currentObject.GetComponent<InputField>();
            if (inputField != null)
            {
                if (inputField.isFocused)
                {
                    allow = false;
                }
            }
            else
            {
                TMP_InputField tmpInput = currentObject.GetComponent<TMP_InputField>();
                if (tmpInput != null)
                {
                    if (tmpInput.isFocused)
                    {
                        allow = false;
                    }
                }
            }
        }

        if (allow)
        {
            Vector2 move = Vector2.zero;
            float speed = DefaultSpeed * (Input.GetKey(KeyCode.LeftShift) ? shiftMultiplier : 1f) * Time.deltaTime * 9.1f;
            if (Input.GetKey(KeyCode.W)) move += Vector2.up * speed;
            if (Input.GetKey(KeyCode.S)) move -= Vector2.up * speed;
            if (Input.GetKey(KeyCode.D)) move += Vector2.right * speed;
            if (Input.GetKey(KeyCode.A)) move -= Vector2.right * speed;
            transform.Translate(move);

            if (Input.GetKeyUp(KeyCode.Space))
            {
                cam.orthographicSize = 5;
                cam.gameObject.transform.position = new Vector3(0, 0, -1);
            }

            if (BlockScrolling) { return; }
            if (Input.mouseScrollDelta.y == 1)
            {
                if (cam.orthographicSize != 1) cam.orthographicSize -= ZoomScrollSpeed;
            }

            if (Input.mouseScrollDelta.y == -1)
            {
                if (cam.orthographicSize != 20) cam.orthographicSize += ZoomScrollSpeed;
            }
        }
    }
}
