using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectInput : MonoBehaviour
{
    public float rotationSpeed = 0.2f;
    public float scaleSpeed = 0.01f;
    public float panSpeed = 0.1f;
    private Vector3 lastMousePosition;

    void Update()
    {
        // 如果正在拖动进度条，直接返回不执行旋转代码
        if (AnimationSliderControl.isDraggingSlider)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    float rotationX = touch.deltaPosition.x * rotationSpeed;
                    float rotationY = touch.deltaPosition.y * rotationSpeed;
                    transform.Rotate(Vector3.up, rotationX, Space.World);
                    transform.Rotate(Vector3.right, rotationY, Space.World);
                }
            }
            else if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);
                float prevTouchDeltaMag = (touch0.position - touch1.position).magnitude;
                float touchDeltaMag = (touch0.position - touch1.position).magnitude;
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                transform.localScale += Vector3.one * deltaMagnitudeDiff * scaleSpeed;

                Vector2 touchDeltaPosition = (touch0.deltaPosition + touch1.deltaPosition) / 2;
                Vector3 panMovement = new Vector3(-touchDeltaPosition.x * panSpeed, -touchDeltaPosition.y * panSpeed, 0);
                transform.Translate(panMovement, Space.World);
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                float rotationX = delta.x * rotationSpeed;
                float rotationY = delta.y * rotationSpeed;
                transform.Rotate(Vector3.up, -rotationX, Space.World);
                transform.Rotate(Vector3.right, rotationY, Space.World);
            }

            if (Input.GetMouseButton(2))
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                Vector3 panMovement = new Vector3(-delta.x * panSpeed, -delta.y * panSpeed, 0);
                transform.Translate(panMovement, Space.World);
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                transform.localScale += Vector3.one * scroll * scaleSpeed * 50;
            }
        }

        lastMousePosition = Input.mousePosition;
    }
}
