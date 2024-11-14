using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectShowTag : MonoBehaviour
{
    public Canvas canvas; // ���е� Canvas
    public Camera mainCamera; // �������
    private Outline outline; // Outline ���
    private TextMeshProUGUI nameDisplay; // ������ʾ���ֵ� UI ���
    private GameObject background; // ��ɫ����
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

        // ������������
        background = new GameObject("Background");
        background.transform.SetParent(canvas.transform);
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = Color.white;

        RectTransform bgRect = background.GetComponent<RectTransform>();
        bgRect.sizeDelta = new Vector2(200, 50);

        // ���� TextMeshProUGUI ����
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
            Vector3 worldPosition = transform.position + Vector3.up * 0.1f; // ��С��ƫ����
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

            // ȷ��λ������Ļ��Χ��
            screenPosition.x = Mathf.Clamp(screenPosition.x, 0, Screen.width - 200); // 200�Ǳ������
            screenPosition.y = Mathf.Clamp(screenPosition.y, 0, Screen.height - 50);  // 50�Ǳ����߶�

            background.transform.position = screenPosition;
            Debug.Log("Adjusted Screen Position: " + screenPosition); // ������������Ļ����
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
