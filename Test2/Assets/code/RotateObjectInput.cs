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
    public float panSpeedMobile = 0.005f;    // �ϵ͵�ƽ���ٶ�

    private Vector3 lastMousePosition; // ��¼��һ�����λ��
    private float initialTouchDistance; // ��ʼ˫ָ����
    private Vector3 initialObjectScale; // ��¼��ʼ����

    // �������ŷ�Χ
    public float minScale = 0.5f;      // ��С���ű���
    public float maxScale = 2.0f;      // ������ű���

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
        if (Input.touchCount > 0)
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
                    transform.Rotate(Vector3.right, rotationY, Space.World);
                }
            }
            // ˫ָ�����������ź�ƽ��
            else if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                // ��˫ָ������ʼʱ����¼��ʼ˫ָ���������ĳ�ʼ����
                if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
                {
                    initialTouchDistance = Vector2.Distance(touch0.position, touch1.position);
                    initialObjectScale = transform.localScale;
                }

                // ��ǰ˫ָ����
                float currentTouchDistance = Vector2.Distance(touch0.position, touch1.position);
                float scaleMultiplier = (currentTouchDistance / initialTouchDistance);

                // Ӧ�����Ų��������ŷ�Χ
                Vector3 newScale = initialObjectScale * scaleMultiplier;
                newScale = Vector3.Max(newScale, Vector3.one * minScale); // ������С����
                newScale = Vector3.Min(newScale, Vector3.one * maxScale); // �����������
                transform.localScale = newScale;

                // ƽ�Ʋ�����������ֻ��ָƽ���ƶ��ľ���
                Vector2 touchDeltaPosition = (touch0.deltaPosition + touch1.deltaPosition) / 2;
                Vector3 panMovement = new Vector3(-touchDeltaPosition.x * currentPanSpeed, -touchDeltaPosition.y * currentPanSpeed, 0);

                // Ӧ��ƽ��
                transform.Translate(panMovement, Space.World);
            }
        }
        else
        {
            // ������PC�˵�������������Ҫ����
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

        // ������һ�����λ��
        lastMousePosition = Input.mousePosition;
    }
}
