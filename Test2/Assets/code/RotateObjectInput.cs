using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectInput : MonoBehaviour
{
    public float rotationSpeed = 0.2f; // 控制旋转速度

    // PC 端的缩放和平移速度
    public float scaleSpeedPC = 0.01f;
    public float panSpeedPC = 0.1f;

    // 移动端的缩放和平移速度
    public float scaleSpeedMobile = 0.003f;  // 较低的缩放速度
    public float panSpeedMobile = 0.005f;    // 较低的平移速度

    private Vector3 lastMousePosition; // 记录上一次鼠标位置

    // 限制缩放范围
    public float minScale = 0.5f;      // 最小缩放比例
    public float maxScale = 2.0f;      // 最大缩放比例

    void Update()
    {
        // 检查当前设备平台，并使用对应的速度
        float currentScaleSpeed = Application.isMobilePlatform ? scaleSpeedMobile : scaleSpeedPC;
        float currentPanSpeed = Application.isMobilePlatform ? panSpeedMobile : panSpeedPC;

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

                // 应用缩放，并限制缩放范围
                Vector3 newScale = transform.localScale + Vector3.one * deltaMagnitudeDiff * currentScaleSpeed;
                newScale = Vector3.Max(newScale, Vector3.one * minScale); // 限制最小缩放
                newScale = Vector3.Min(newScale, Vector3.one * maxScale); // 限制最大缩放
                transform.localScale = newScale;

                // 平移操作：计算两只手指平均移动的距离
                Vector2 touchDeltaPosition = (touch0.deltaPosition + touch1.deltaPosition) / 2;
                Vector3 panMovement = new Vector3(-touchDeltaPosition.x * currentPanSpeed, -touchDeltaPosition.y * currentPanSpeed, 0);

                // 应用平移
                transform.Translate(panMovement, Space.World);
            }
        }
        else
        {
            // 这里是PC端的鼠标操作，不需要更改
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
                Vector3 panMovement = new Vector3(delta.x * panSpeedPC, delta.y * panSpeedPC, 0);

                transform.Translate(panMovement, Space.World);
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                transform.localScale += Vector3.one * scroll * scaleSpeedPC * 50;
            }
        }

        // 更新上一次鼠标位置
        lastMousePosition = Input.mousePosition;
    }
}
