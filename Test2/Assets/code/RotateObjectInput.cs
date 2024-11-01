using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectInput : MonoBehaviour
{
    public float rotationSpeed = 0.2f; // ������ת�ٶ�
    private Vector3 lastMousePosition;

    void Update()
    {
        // ����Ƿ��Ǵ�������
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                float rotationX = touch.deltaPosition.x * rotationSpeed;
                float rotationY = touch.deltaPosition.y * rotationSpeed;

                // ���ݴ�������������ת����
                transform.Rotate(Vector3.up, -rotationX, Space.World);
                transform.Rotate(Vector3.right, rotationY, Space.World);
            }
        }
        // ���û�д����������������
        else if (Input.GetMouseButton(0)) // ��ס������
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float rotationX = delta.x * rotationSpeed;
            float rotationY = delta.y * rotationSpeed;

            // ��������ƶ�������ת����
            transform.Rotate(Vector3.up, -rotationX, Space.World);
            transform.Rotate(Vector3.right, rotationY, Space.World);
        }

        // �����������λ��
        lastMousePosition = Input.mousePosition;
    }
}

