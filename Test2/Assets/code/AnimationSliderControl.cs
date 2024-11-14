using UnityEngine;
using UnityEngine.UI;

public class AnimationSliderControl : MonoBehaviour
{
    public Animator animator;
    public Slider animationSlider;
    public AnimationControl animationControl;
    private bool isDragging = false;

    // 为 RotateObjectInput 提供访问的静态变量
    public static bool isDraggingSlider = false;

    void Start()
    {
        animationSlider.minValue = 0f;
        animationSlider.maxValue = 1f;
        animationSlider.value = 0f;
        animationSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void Update()
    {
        if (!isDragging)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            animationSlider.value = stateInfo.normalizedTime % 1;
        }
    }

    public void OnSliderValueChanged(float value)
    {
        if (isDragging)
        {
            // 限制滑动条的值在 0 到 0.9999f 之间，避免动画回到第一帧
            float adjustedValue = Mathf.Clamp(value, 0, 0.99f);
            animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, adjustedValue);
            animator.speed = 0;
        }
    }

    public void OnPointerDown()
    {
        isDragging = true;
        isDraggingSlider = true; // 设置静态变量为 true，指示正在拖动
    }

    public void OnPointerUp()
    {
        isDragging = false;
        isDraggingSlider = false; // 释放静态变量
        if (animationControl != null && !animationControl.IsPaused)
        {
            animator.speed = 1; // 恢复动画播放
        }
    }
}
