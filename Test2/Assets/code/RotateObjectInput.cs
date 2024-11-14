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
    public float panSpeedMobile = 0.005f;    // 降低的平移速度

    private Vector3 lastMousePosition;       // 记录上一次鼠标位置
    private float initialTouchDistance;      // 初始双指距离
    private Vector3 initialObjectScale;      // 记录初始缩放
    private Vector2 initialTouchCenter;      // 记录双指初始的中心位置
    private float initialAngle;              // 记录双指初始角度

    // 限制缩放范围
    public float minScale = 0.5f;            // 最小缩放比例
    public float maxScale = 2.0f;            // 最大缩放比例

    // 平移的最大幅度限制
    private float maxPanMagnitudeMobile = 50f;  // 限制平移距离，以防移动端偏移过大

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
        if (Application.isMobilePlatform && Input.touchCount > 0)
        {
            // 单指触摸用于旋转
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    float rotationX = touch.deltaPosition.x * rotationSpeed;
                    float rotationY = touch.deltaPosition.y * rotationSpeed;

                    // 应用旋转（世界坐标系）
                    transform.Rotate(Vector3.up, rotationX, Space.World);
                    transform.Rotate(Vector3.right, -rotationY, Space.World);
                }
            }
            // 双指触摸用于缩放、平移和旋转
            else if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                // 记录双指触摸的初始距离、初始缩放、初始中心位置和初始角度
                if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
                {
                    initialTouchDistance = Vector2.Distance(touch0.position, touch1.position);
                    initialObjectScale = transform.localScale;
                    initialTouchCenter = (touch0.position + touch1.position) / 2;
                    initialAngle = Mathf.Atan2(touch1.position.y - touch0.position.y, touch1.position.x - touch0.position.x) * Mathf.Rad2Deg;
                }

                // 当前双指距离
                float currentTouchDistance = Vector2.Distance(touch0.position, touch1.position);

                // 只有当双指距离发生变化时才执行缩放
                if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
                {
                    // 计算距离的变化比例，放大或缩小
                    float scaleMultiplier = currentTouchDistance / initialTouchDistance;

                    // 计算新的缩放比例，基于初始比例
                    Vector3 newScale = initialObjectScale * scaleMultiplier;
                    newScale = Vector3.Max(newScale, Vector3.one * minScale); // 限制最小缩放
                    newScale = Vector3.Min(newScale, Vector3.one * maxScale); // 限制最大缩放
                    transform.localScale = newScale;
                }

                // 计算当前双指的中心位置和相对的平移距离
                Vector2 currentTouchCenter = (touch0.position + touch1.position) / 2;
                Vector2 panDelta = currentTouchCenter - initialTouchCenter;

                // 限制移动端平移的幅度
                panDelta = Vector2.ClampMagnitude(panDelta, maxPanMagnitudeMobile);

                // 只有当两个手指都在移动时才进行平移
                if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
                {
                    Vector3 panMovement = new Vector3(panDelta.x * currentPanSpeed, panDelta.y * currentPanSpeed, 0);
                    transform.Translate(panMovement, Space.World);
                    initialTouchCenter = currentTouchCenter; // 更新中心位置
                }

                // 计算当前的角度
                float currentAngle = Mathf.Atan2(touch1.position.y - touch0.position.y, touch1.position.x - touch0.position.x) * Mathf.Rad2Deg;
                float angleDelta = currentAngle - initialAngle;

                // 应用旋转
                transform.Rotate(Vector3.forward, angleDelta, Space.World);

                // 更新初始角度
                initialAngle = currentAngle;
            }
        }
        else if (!Application.isMobilePlatform)
        {
            // 这里是PC端的鼠标操作
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
                // 计算新的缩放比例
                Vector3 newScale = transform.localScale + Vector3.one * scroll * scaleSpeedPC * 50;

                // 应用缩放限制
                newScale = Vector3.Max(newScale, Vector3.one * minScale);
                newScale = Vector3.Min(newScale, Vector3.one * maxScale);

                // 应用新的缩放
                transform.localScale = newScale;
            }
        }

        // 更新上一次鼠标位置
        lastMousePosition = Input.mousePosition;
    }
}
