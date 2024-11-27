using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnimationSliderControl : MonoBehaviour
{
    public Animator animator;
    public Slider animationSlider;
    public AnimationControl animationControl;
    private Button playButton;  // Play ��ť
    private Button stopButton;  // Stop ��ť
    private Button resetButton; // Reset ��ť
    private Button placeButton; // Place ��ť
    private bool isDragging = false;

    public static bool isDraggingSlider = false;

    void Start()
    {
        // �Զ����ҳ����е�Ψһ Slider
        if (animationSlider == null)
        {
            animationSlider = GameObject.Find("ControlBar").GetComponent<Slider>();
        }

        if (animationSlider != null)
        {
            animationSlider.minValue = 0f;
            animationSlider.maxValue = 1f;
            animationSlider.value = 0f;

            animationSlider.onValueChanged.AddListener(OnSliderValueChanged);

            AddEventTrigger(animationSlider.gameObject, EventTriggerType.PointerDown, (data) => { OnPointerDown(); });
            AddEventTrigger(animationSlider.gameObject, EventTriggerType.PointerUp, (data) => { OnPointerUp(); });
        }
        else
        {
            Debug.LogError("δ�ҵ� Slider����ȷ�������д���һ�� Slider��");
        }

        // �Զ����Ұ�ť
        playButton = GameObject.Find("Play").GetComponent<Button>();
        stopButton = GameObject.Find("Stop").GetComponent<Button>();
        resetButton = GameObject.Find("ResetButton").GetComponent<Button>();
        placeButton = GameObject.Find("PlaceButton").GetComponent<Button>();

        // ��Ӱ�ť����¼�������
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
        else
        {
            Debug.LogError("δ�ҵ� Play ��ť����ȷ�������д���һ�� Play ��ť��");
        }

        if (stopButton != null)
        {
            stopButton.onClick.AddListener(OnStopButtonClicked);
        }
        else
        {
            Debug.LogError("δ�ҵ� Stop ��ť����ȷ�������д���һ�� Stop ��ť��");
        }

        if (resetButton != null)
        {
            resetButton.onClick.AddListener(OnResetButtonClicked);
        }
        else
        {
            Debug.LogError("δ�ҵ� Reset ��ť����ȷ�������д���һ�� Reset ��ť��");
        }

        if (placeButton != null)
        {
            placeButton.onClick.AddListener(OnPlaceButtonClicked);
        }
        else
        {
            Debug.LogError("δ�ҵ� Place ��ť����ȷ�������д���һ�� Place ��ť��");
        }
    }

    void Update()
    {
        if (!isDragging && animator != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (animationSlider != null)
            {
                animationSlider.value = stateInfo.normalizedTime % 1;
            }
        }
    }

    public void OnSliderValueChanged(float value)
    {
        if (isDragging && animator != null)
        {
            float adjustedValue = Mathf.Clamp(value, 0, 0.99f);
            animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, adjustedValue);
            animator.speed = 0;
        }
    }

    public void OnPointerDown()
    {
        isDragging = true;
        isDraggingSlider = true;
    }

    public void OnPointerUp()
    {
        isDragging = false;
        isDraggingSlider = false;
        if (animationControl != null && !animationControl.IsPaused)
        {
            animator.speed = 1;
        }
    }

    private void AddEventTrigger(GameObject obj, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = eventType
        };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    // Play ��ť����¼�
    public void OnPlayButtonClicked()
    {
        if (animator != null)
        {
            animator.speed = 1;
        }
    }

    // Stop ��ť����¼�
    public void OnStopButtonClicked()
    {
        if (animator != null)
        {
            animator.speed = 0;
        }
    }

    // Reset ��ť����¼�
    public void OnResetButtonClicked()
    {
        if (animator != null)
        {
            animator.Play(0, 0, 0); // ���ö�������ʼ״̬
            animator.speed = 0;     // ��ͣ����
            animationSlider.value = 0; // ���û���
        }

        // ���������λ�á���ת������
        Reset resetScript = animator.GetComponent<Reset>();
        if (resetScript != null)
        {
            resetScript.ResetTransform();
        }
        else
        {
            Debug.LogWarning("δ�ҵ� Reset �ű�����ȷ���ö��󸽼��� Reset �ű���");
        }
    }

    // Place ��ť����¼�
    public void OnPlaceButtonClicked()
    {
        // ʵ����ϣ���ķ����߼�
        Debug.Log("Place button clicked");
    }
}
