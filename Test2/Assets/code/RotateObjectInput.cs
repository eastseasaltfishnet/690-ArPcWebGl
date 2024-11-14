using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectInput : MonoBehaviour
{
    public float rotationSpeed = 0.2f; // ������ת�ٶ�

    // PC �˵����ź�ƽ���ٶ�
    public float scaleSpeedPC = 0.01f;
    public float panSpeedPC = 0.1f;

    // �ƶ��˵����ź�ƽ���ٶ�
    public float scaleSpeedMobile = 0.003f;  // �ϵ͵������ٶ�
    public float panSpeedMobile = 0.005f;    // ���͵�ƽ���ٶ�

    private Vector3 lastMousePosition;       // ��¼��һ�����λ��
    private float initialTouchDistance;      // ��ʼ˫ָ����
    private Vector3 initialObjectScale;      // ��¼��ʼ����
    private Vector2 initialTouchCenter;      // ��¼˫ָ��ʼ������λ��
    private float initialAngle;              // ��¼˫ָ��ʼ�Ƕ�

    // �������ŷ�Χ
    public float minScale = 0.5f;            // ��С���ű���
    public float maxScale = 2.0f;            // ������ű���

    // ƽ�Ƶ�����������
    private float maxPanMagnitudeMobile = 50f;  // ����ƽ�ƾ��룬�Է��ƶ���ƫ�ƹ���

    void Update()
    {
        // ��鵱ǰ�豸ƽ̨����ʹ�ö�Ӧ���ٶ�
        float currentScaleSpeed = Application.isMobilePlatform ? scaleSpeedMobile : scaleSpeedPC;
        float currentPanSpeed = Application.isMobilePlatform ? panSpeedMobile : panSpeedPC;

        // ��������϶���������ֱ�ӷ��أ���ִ����ת����
        if (AnimationSliderControl.isDraggingSlider)
        {
            return;
        }

        // ��鴥�����루�������ƶ��豸��
        if (Application.isMobilePlatform && Input.touchCount > 0)
        {
            // ��ָ����������ת
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    float rotationX = touch.deltaPosition.x * rotationSpeed;
                    float rotationY = touch.deltaPosition.y * rotationSpeed;

                    // Ӧ����ת����������ϵ��
                    transform.Rotate(Vector3.up, rotationX, Space.World);
                    transform.Rotate(Vector3.right, -rotationY, Space.World);
                }
            }
            // ˫ָ�����������š�ƽ�ƺ���ת
            else if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                // ��¼˫ָ�����ĳ�ʼ���롢��ʼ���š���ʼ����λ�úͳ�ʼ�Ƕ�
                if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
                {
                    initialTouchDistance = Vector2.Distance(touch0.position, touch1.position);
                    initialObjectScale = transform.localScale;
                    initialTouchCenter = (touch0.position + touch1.position) / 2;
                    initialAngle = Mathf.Atan2(touch1.position.y - touch0.position.y, touch1.position.x - touch0.position.x) * Mathf.Rad2Deg;
                }

                // ��ǰ˫ָ����
                float currentTouchDistance = Vector2.Distance(touch0.position, touch1.position);

                // ֻ�е�˫ָ���뷢���仯ʱ��ִ������
                if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
                {
                    // �������ı仯�������Ŵ����С
                    float scaleMultiplier = currentTouchDistance / initialTouchDistance;

                    // �����µ����ű��������ڳ�ʼ����
                    Vector3 newScale = initialObjectScale * scaleMultiplier;
                    newScale = Vector3.Max(newScale, Vector3.one * minScale); // ������С����
                    newScale = Vector3.Min(newScale, Vector3.one * maxScale); // �����������
                    transform.localScale = newScale;
                }

                // ���㵱ǰ˫ָ������λ�ú���Ե�ƽ�ƾ���
                Vector2 currentTouchCenter = (touch0.position + touch1.position) / 2;
                Vector2 panDelta = currentTouchCenter - initialTouchCenter;

                // �����ƶ���ƽ�Ƶķ���
                panDelta = Vector2.ClampMagnitude(panDelta, maxPanMagnitudeMobile);

                // ֻ�е�������ָ�����ƶ�ʱ�Ž���ƽ��
                if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
                {
                    Vector3 panMovement = new Vector3(panDelta.x * currentPanSpeed, panDelta.y * currentPanSpeed, 0);
                    transform.Translate(panMovement, Space.World);
                    initialTouchCenter = currentTouchCenter; // ��������λ��
                }

                // ���㵱ǰ�ĽǶ�
                float currentAngle = Mathf.Atan2(touch1.position.y - touch0.position.y, touch1.position.x - touch0.position.x) * Mathf.Rad2Deg;
                float angleDelta = currentAngle - initialAngle;

                // Ӧ����ת
                transform.Rotate(Vector3.forward, angleDelta, Space.World);

                // ���³�ʼ�Ƕ�
                initialAngle = currentAngle;
            }
        }
        else if (!Application.isMobilePlatform)
        {
            // ������PC�˵�������
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
                // �����µ����ű���
                Vector3 newScale = transform.localScale + Vector3.one * scroll * scaleSpeedPC * 50;

                // Ӧ����������
                newScale = Vector3.Max(newScale, Vector3.one * minScale);
                newScale = Vector3.Min(newScale, Vector3.one * maxScale);

                // Ӧ���µ�����
                transform.localScale = newScale;
            }
        }

        // ������һ�����λ��
        lastMousePosition = Input.mousePosition;
    }
}
