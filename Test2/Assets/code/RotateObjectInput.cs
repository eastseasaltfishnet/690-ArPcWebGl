using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectInput : MonoBehaviour
{
    public float rotationSpeed = 0.2f; // 控制旋转速度
    private Vector3 lastMousePosition;

    void Update()
    {
        // 检查是否是触摸输入
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                float rotationX = touch.deltaPosition.x * rotationSpeed;
                float rotationY = touch.deltaPosition.y * rotationSpeed;

                // 根据触摸滑动方向旋转物体
                transform.Rotate(Vector3.up, -rotationX, Space.World);
                transform.Rotate(Vector3.right, rotationY, Space.World);
            }
        }
        // 如果没有触摸，则检查鼠标输入
        else if (Input.GetMouseButton(0)) // 按住鼠标左键
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float rotationX = delta.x * rotationSpeed;
            float rotationY = delta.y * rotationSpeed;

            // 根据鼠标移动方向旋转物体
            transform.Rotate(Vector3.up, -rotationX, Space.World);
            transform.Rotate(Vector3.right, rotationY, Space.World);
        }

        // 更新最后的鼠标位置
        lastMousePosition = Input.mousePosition;
    }
}

