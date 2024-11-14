using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectShowTag : MonoBehaviour
{
    public Canvas canvas; // 现有的 Canvas
    public Camera mainCamera; // 主摄像机
    private Outline outline; // Outline 组件
    private TextMeshProUGUI nameDisplay; // 用于显示名字的 UI 组件
    private GameObject background; // 白色背景
    private bool isSelected = false;

    void Start()
    {
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // 创建背景对象
        background = new GameObject("Background");
        background.transform.SetParent(canvas.transform);
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = Color.white;

        RectTransform bgRect = background.GetComponent<RectTransform>();
        bgRect.sizeDelta = new Vector2(200, 50);

        // 创建 TextMeshProUGUI 文字
        GameObject textObj = new GameObject("NameDisplay");
        textObj.transform.SetParent(background.transform);
        nameDisplay = textObj.AddComponent<TextMeshProUGUI>();
        nameDisplay.fontSize = 24;
        nameDisplay.alignment = TextAlignmentOptions.Center;
        nameDisplay.color = Color.black;
        nameDisplay.text = "";
        nameDisplay.gameObject.SetActive(false);

        RectTransform textRect = nameDisplay.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(200, 50);
        textRect.localPosition = Vector3.zero;

        background.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                ToggleSelection();
            }
            else if (isSelected)
            {
                ToggleSelection();
            }
        }

        if (isSelected && nameDisplay != null)
        {
            Vector3 worldPosition = transform.position + Vector3.up * 0.1f; // 更小的偏移量
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

            // 确保位置在屏幕范围内
            screenPosition.x = Mathf.Clamp(screenPosition.x, 0, Screen.width - 200); // 200是背景宽度
            screenPosition.y = Mathf.Clamp(screenPosition.y, 0, Screen.height - 50);  // 50是背景高度

            background.transform.position = screenPosition;
            Debug.Log("Adjusted Screen Position: " + screenPosition); // 输出调整后的屏幕坐标
        }
    }

    void ToggleSelection()
    {
        isSelected = !isSelected;

        if (isSelected)
        {
            if (outline != null)
            {
                outline.enabled = true;
            }
            nameDisplay.text = gameObject.name;
            background.SetActive(true);
            nameDisplay.gameObject.SetActive(true);
        }
        else
        {
            if (outline != null)
            {
                outline.enabled = false;
            }
            background.SetActive(false);
            nameDisplay.gameObject.SetActive(false);
        }
    }
}
