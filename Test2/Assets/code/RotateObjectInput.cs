using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectInput : MonoBehaviour
{
    public float rotationSpeed = 0.2f; // 控制旋转速度

    // PC 和移动端的缩放和平移速度
    public float scaleSpeedPC = 0.01f;
    public float panSpeedPC = 0.1f;
    public float scaleSpeedMobile = 0.003f;
    public float panSpeedMobile = 0.005f;

    private Vector3 lastMousePosition;
    private float initialTouchDistance;
    private Vector3 initialObjectScale;
    private Vector2 initialTouchCenter;
    private float initialAngle;

    public float minScale = 0.5f; // 最小缩放比例
    public float maxScale = 2.0f; // 最大缩放比例

    void Update()
    {
        float currentScaleSpeed = Application.isMobilePlatform ? scaleSpeedMobile : scaleSpeedPC;
        float currentPanSpeed = Application.isMobilePlatform ? panSpeedMobile : panSpeedPC;

        // 如果正在拖动进度条，直接返回
        if (AnimationSliderControl.isDraggingSlider) return;

        // 检查触摸输入
        if (Input.touchCount > 0)
        {
            // 单指旋转
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
            // 双指缩放和平移
            else if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                // 记录初始双指触摸的距离、中心位置和角度
                if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
                {
                    initialTouchDistance = Vector2.Distance(touch0.position, touch1.position);
                    initialObjectScale = transform.localScale;
                    initialTouchCenter = (touch0.position + touch1.position) / 2;
                    initialAngle = Mathf.Atan2(touch1.position.y - touch0.position.y, touch1.position.x - touch0.position.x) * Mathf.Rad2Deg;
                }

                float currentTouchDistance = Vector2.Distance(touch0.position, touch1.position);
                float scaleMultiplier = currentTouchDistance / initialTouchDistance;

                // 缩放
                if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
                {
                    Vector3 newScale = initialObjectScale * scaleMultiplier;
                    newScale = Vector3.Max(newScale, Vector3.one * minScale);
                    newScale = Vector3.Min(newScale, Vector3.one * maxScale);
                    transform.localScale = newScale;
                }

                // 计算双指平移增量
                Vector2 currentTouchCenter = (touch0.position + touch1.position) / 2;
                Vector2 panDelta = currentTouchCenter - initialTouchCenter;

                // 限制每帧平移增量，防止出现瞬间平移
                panDelta = Vector2.ClampMagnitude(panDelta, 10.0f); // 限制增量
                Vector3 panMovement = new Vector3(panDelta.x * currentPanSpeed, panDelta.y * currentPanSpeed, 0);

                // 应用平移
                if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
                {
<<<<<<< HEAD
                    transform.Translate(panMovement, Space.World);
                    initialTouchCenter = currentTouchCenter; // 实时更新中心位置
=======
                    Vector3 panMovement = new Vector3(panDelta.x * currentPanSpeed*1.3f, panDelta.y * currentPanSpeed * 1.3f, 0);
                    transform.Translate(-panMovement, Space.World);
                    initialTouchCenter = currentTouchCenter; // 更新中心位置

>>>>>>> parent of d15f726a (contro update)
                }

                // 旋转
                float currentAngle = Mathf.Atan2(touch1.position.y - touch0.position.y, touch1.position.x - touch0.position.x) * Mathf.Rad2Deg;
                float angleDelta = currentAngle - initialAngle;
                transform.Rotate(Vector3.forward, angleDelta, Space.World);
                initialAngle = currentAngle;
            }
        }
        else
        {
            // PC 端的鼠标操作
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
                Vector3 newScale = transform.localScale + Vector3.one * scroll * scaleSpeedPC * 50;
                newScale = Vector3.Max(newScale, Vector3.one * minScale);
                newScale = Vector3.Min(newScale, Vector3.one * maxScale);
                transform.localScale = newScale;
            }
        }

        lastMousePosition = Input.mousePosition;
    }
}
