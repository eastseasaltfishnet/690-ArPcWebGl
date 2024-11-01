using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectInput : MonoBehaviour
{
    public float rotationSpeed = 0.2f; // 控制旋转速度
    public float scaleSpeed = 0.01f;    // 控制缩放速度
    public float panSpeed = 0.1f;       // 控制平移速度
    private Vector3 lastMousePosition;  // 记录上一次鼠标位置

    void Update()
    {
        // 如果正在拖动进度条，直接返回，不执行旋转代码
        if (AnimationSliderControl.isDraggingSlider)
        {
            return;
        }

        // 检查触摸输入（适用于移动设备）
        if (Input.touchCount > 0)
        {
            // 单指触摸用于旋转
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    // 根据触摸移动的距离计算旋转量
                    float rotationX = touch.deltaPosition.x * rotationSpeed;
                    float rotationY = touch.deltaPosition.y * rotationSpeed;

                    // 应用旋转（世界坐标系）
                    transform.Rotate(Vector3.up, rotationX, Space.World);
                    transform.Rotate(Vector3.right, rotationY, Space.World);
                }
            }
            // 双指触摸用于缩放和平移
            else if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                // 计算两次触摸之间的距离变化用于缩放
                float prevTouchDeltaMag = (touch0.position - touch1.position).magnitude;
                float touchDeltaMag = (touch0.position - touch1.position).magnitude;
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                // 应用缩放
                transform.localScale += Vector3.one * deltaMagnitudeDiff * scaleSpeed;
//
//// 平移操作：计算两只手指平均移动的距离
//Vector2 touchDeltaPosition = (touch0.deltaPosition + touch1.deltaPosition) / 2;
//Vector3 panMovement = new Vector3(-touchDeltaPosition.x * panSpeed, -touchDeltaPosition.y * panSpeed, 0);
//
//// 应用平移
//transform.Translate(panMovement, Space.World);
            }
        }
        else
        {
            // 鼠标左键按下时旋转
            if (Input.GetMouseButton(0))
            {
                // 计算鼠标当前位置和上一次位置的差值
                Vector3 delta = Input.mousePosition - lastMousePosition;
                float rotationX = delta.x * rotationSpeed;
                float rotationY = delta.y * rotationSpeed;

                // 应用旋转（世界坐标系）
                transform.Rotate(Vector3.up, -rotationX, Space.World);
                transform.Rotate(Vector3.right, rotationY, Space.World);
            }

            // 鼠标中键按下时平移
            if (Input.GetMouseButton(2))
            {
                // 计算平移的距离
                Vector3 delta = Input.mousePosition - lastMousePosition;
                Vector3 panMovement = new Vector3(-delta.x * panSpeed, -delta.y * panSpeed, 0);

                // 应用平移
                transform.Translate(panMovement, Space.World);
            }

            // 鼠标滚轮用于缩放
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                // 应用缩放，调整系数以获得适当的缩放速度
                transform.localScale += Vector3.one * scroll * scaleSpeed * 50;
            }
        }

        // 更新上一次鼠标位置
        lastMousePosition = Input.mousePosition;
    }
}
