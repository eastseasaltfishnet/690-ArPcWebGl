using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    void Start()
    {
        // ��¼��ʼ״̬
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

    // ���ô˷�����������
    public void ResetTransform()
    {
        transform.position = new Vector3(initialRotation.x, initialRotation.y, 11f);
        transform.rotation = initialRotation;
        transform.localScale = initialScale;
    }
}
