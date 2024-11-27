using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchScene : MonoBehaviour
{
    public Button startButton; // 将在Inspector中分配给按钮

    void Start()
    {
        // 检查按钮是否已经分配
        if (startButton != null)
        {
            // 给按钮添加点击事件监听器
            startButton.onClick.AddListener(LoadTest1Scene);
        }
        else
        {
            Debug.LogError("StartButton is not assigned.");
        }
    }

    void LoadTest1Scene()
    {
        // 加载名为"Test1"的场景
        SceneManager.LoadScene("Test1");
    }
}
