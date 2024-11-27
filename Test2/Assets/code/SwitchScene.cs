using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchScene : MonoBehaviour
{
    public Button startButton; // ����Inspector�з������ť

    void Start()
    {
        // ��鰴ť�Ƿ��Ѿ�����
        if (startButton != null)
        {
            // ����ť��ӵ���¼�������
            startButton.onClick.AddListener(LoadTest1Scene);
        }
        else
        {
            Debug.LogError("StartButton is not assigned.");
        }
    }

    void LoadTest1Scene()
    {
        // ������Ϊ"Test1"�ĳ���
        SceneManager.LoadScene("Test1");
    }
}
