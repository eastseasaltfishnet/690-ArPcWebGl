using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnimationSliderControl : MonoBehaviour
{
    public Animator animator;
    public Slider animationSlider;
    public AnimationControl animationControl;
    private Button playButton;  // Play 按钮
    private Button stopButton;  // Stop 按钮
    private Button resetButton; // Reset 按钮
    private Button placeButton; // Place 按钮
    private bool isDragging = false;

    public static bool isDraggingSlider = false;

    void Start()
    {
        // 自动查找场景中的唯一 Slider
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
            Debug.LogError("未找到 Slider，请确保场景中存在一个 Slider。");
        }

        // 自动查找按钮
        playButton = GameObject.Find("Play").GetComponent<Button>();
        stopButton = GameObject.Find("Stop").GetComponent<Button>();
        resetButton = GameObject.Find("ResetButton").GetComponent<Button>();
        placeButton = GameObject.Find("PlaceButton").GetComponent<Button>();

        // 添加按钮点击事件监听器
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
        else
        {
            Debug.LogError("未找到 Play 按钮，请确保场景中存在一个 Play 按钮。");
        }

        if (stopButton != null)
        {
            stopButton.onClick.AddListener(OnStopButtonClicked);
        }
        else
        {
            Debug.LogError("未找到 Stop 按钮，请确保场景中存在一个 Stop 按钮。");
        }

        if (resetButton != null)
        {
            resetButton.onClick.AddListener(OnResetButtonClicked);
        }
        else
        {
            Debug.LogError("未找到 Reset 按钮，请确保场景中存在一个 Reset 按钮。");
        }

        if (placeButton != null)
        {
            placeButton.onClick.AddListener(OnPlaceButtonClicked);
        }
        else
        {
            Debug.LogError("未找到 Place 按钮，请确保场景中存在一个 Place 按钮。");
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

    // Play 按钮点击事件
    public void OnPlayButtonClicked()
    {
        if (animator != null)
        {
            animator.speed = 1;
        }
    }

    // Stop 按钮点击事件
    public void OnStopButtonClicked()
    {
        if (animator != null)
        {
            animator.speed = 0;
        }
    }

    // Reset 按钮点击事件
    public void OnResetButtonClicked()
    {
        if (animator != null)
        {
            animator.Play(0, 0, 0); // 重置动画到初始状态
            animator.speed = 0;     // 暂停动画
            animationSlider.value = 0; // 重置滑块
        }

        // 重置物体的位置、旋转和缩放
        Reset resetScript = animator.GetComponent<Reset>();
        if (resetScript != null)
        {
            resetScript.ResetTransform();
        }
        else
        {
            Debug.LogWarning("未找到 Reset 脚本，请确保该对象附加了 Reset 脚本。");
        }
    }

    // Place 按钮点击事件
    public void OnPlaceButtonClicked()
    {
        // 实现你希望的放置逻辑
        Debug.Log("Place button clicked");
    }
}
