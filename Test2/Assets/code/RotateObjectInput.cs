using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectInput : MonoBehaviour
{
    public float rotationSpeed = 0.2f; // ������ת�ٶ�

    // PC ���ƶ��˵����ź�ƽ���ٶ�
    public float scaleSpeedPC = 0.01f;
    public float panSpeedPC = 0.1f;
    public float scaleSpeedMobile = 0.003f;
    public float panSpeedMobile = 0.005f;

    private Vector3 lastMousePosition;
    private float initialTouchDistance;
    private Vector3 initialObjectScale;
    private Vector2 initialTouchCenter;
    private float initialAngle;

    public float minScale = 0.5f; // ��С���ű���
    public float maxScale = 2.0f; // ������ű���

    void Update()
    {
        float currentScaleSpeed = Application.isMobilePlatform ? scaleSpeedMobile : scaleSpeedPC;
        float currentPanSpeed = Application.isMobilePlatform ? panSpeedMobile : panSpeedPC;

        // ��������϶���������ֱ�ӷ���
        if (AnimationSliderControl.isDraggingSlider) return;

        // ��鴥������
        if (Input.touchCount > 0)
        {
            // ��ָ��ת
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
            // ˫ָ���ź�ƽ��
            else if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                // ��¼��ʼ˫ָ�����ľ��롢����λ�úͽǶ�
                if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
                {
                    initialTouchDistance = Vector2.Distance(touch0.position, touch1.position);
                    initialObjectScale = transform.localScale;
                    initialTouchCenter = (touch0.position + touch1.position) / 2;
                    initialAngle = Mathf.Atan2(touch1.position.y - touch0.position.y, touch1.position.x - touch0.position.x) * Mathf.Rad2Deg;
                }

                float currentTouchDistance = Vector2.Distance(touch0.position, touch1.position);
                float scaleMultiplier = currentTouchDistance / initialTouchDistance;

                // ����
                if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
                {
                    Vector3 newScale = initialObjectScale * scaleMultiplier;
                    newScale = Vector3.Max(newScale, Vector3.one * minScale);
                    newScale = Vector3.Min(newScale, Vector3.one * maxScale);
                    transform.localScale = newScale;
                }

                // ����˫ָƽ������
                Vector2 currentTouchCenter = (touch0.position + touch1.position) / 2;
                Vector2 panDelta = currentTouchCenter - initialTouchCenter;

                // ����ÿ֡ƽ����������ֹ����˲��ƽ��
                panDelta = Vector2.ClampMagnitude(panDelta, 10.0f); // ��������
                Vector3 panMovement = new Vector3(panDelta.x * currentPanSpeed, panDelta.y * currentPanSpeed, 0);

                // Ӧ��ƽ��
                if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
                {
<<<<<<< HEAD
                    transform.Translate(panMovement, Space.World);
                    initialTouchCenter = currentTouchCenter; // ʵʱ��������λ��
=======
                    Vector3 panMovement = new Vector3(panDelta.x * currentPanSpeed*1.3f, panDelta.y * currentPanSpeed * 1.3f, 0);
                    transform.Translate(-panMovement, Space.World);
                    initialTouchCenter = currentTouchCenter; // ��������λ��

>>>>>>> parent of d15f726a (contro update)
                }

                // ��ת
                float currentAngle = Mathf.Atan2(touch1.position.y - touch0.position.y, touch1.position.x - touch0.position.x) * Mathf.Rad2Deg;
                float angleDelta = currentAngle - initialAngle;
                transform.Rotate(Vector3.forward, angleDelta, Space.World);
                initialAngle = currentAngle;
            }
        }
        else
        {
            // PC �˵�������
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
