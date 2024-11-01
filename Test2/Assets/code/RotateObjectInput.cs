using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectInput : MonoBehaviour
{
    public float rotationSpeed = 0.2f; // ������ת�ٶ�
    public float scaleSpeed = 0.01f;    // ���������ٶ�
    public float panSpeed = 0.1f;       // ����ƽ���ٶ�
    private Vector3 lastMousePosition;  // ��¼��һ�����λ��

    void Update()
    {
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
                    // ���ݴ����ƶ��ľ��������ת��
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

                // �������δ���֮��ľ���仯��������
                float prevTouchDeltaMag = (touch0.position - touch1.position).magnitude;
                float touchDeltaMag = (touch0.position - touch1.position).magnitude;
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                // Ӧ������
                transform.localScale += Vector3.one * deltaMagnitudeDiff * scaleSpeed;
//
//// ƽ�Ʋ�����������ֻ��ָƽ���ƶ��ľ���
//Vector2 touchDeltaPosition = (touch0.deltaPosition + touch1.deltaPosition) / 2;
//Vector3 panMovement = new Vector3(-touchDeltaPosition.x * panSpeed, -touchDeltaPosition.y * panSpeed, 0);
//
//// Ӧ��ƽ��
//transform.Translate(panMovement, Space.World);
            }
        }
        else
        {
            // ����������ʱ��ת
            if (Input.GetMouseButton(0))
            {
                // ������굱ǰλ�ú���һ��λ�õĲ�ֵ
                Vector3 delta = Input.mousePosition - lastMousePosition;
                float rotationX = delta.x * rotationSpeed;
                float rotationY = delta.y * rotationSpeed;

                // Ӧ����ת����������ϵ��
                transform.Rotate(Vector3.up, -rotationX, Space.World);
                transform.Rotate(Vector3.right, rotationY, Space.World);
            }

            // ����м�����ʱƽ��
            if (Input.GetMouseButton(2))
            {
                // ����ƽ�Ƶľ���
                Vector3 delta = Input.mousePosition - lastMousePosition;
                Vector3 panMovement = new Vector3(-delta.x * panSpeed, -delta.y * panSpeed, 0);

                // Ӧ��ƽ��
                transform.Translate(panMovement, Space.World);
            }

            // ��������������
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                // Ӧ�����ţ�����ϵ���Ի���ʵ��������ٶ�
                transform.localScale += Vector3.one * scroll * scaleSpeed * 50;
            }
        }

        // ������һ�����λ��
        lastMousePosition = Input.mousePosition;
    }
}
