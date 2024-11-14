using UnityEngine;
using UnityEngine.UI;

public class AnimationSliderControl : MonoBehaviour
{
    public Animator animator;
    public Slider animationSlider;
    public AnimationControl animationControl;
    private bool isDragging = false;

    // Ϊ RotateObjectInput �ṩ���ʵľ�̬����
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
            // ���ƻ�������ֵ�� 0 �� 0.9999f ֮�䣬���⶯���ص���һ֡
            float adjustedValue = Mathf.Clamp(value, 0, 0.99f);
            animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, adjustedValue);
            animator.speed = 0;
        }
    }

    public void OnPointerDown()
    {
        isDragging = true;
        isDraggingSlider = true; // ���þ�̬����Ϊ true��ָʾ�����϶�
    }

    public void OnPointerUp()
    {
        isDragging = false;
        isDraggingSlider = false; // �ͷž�̬����
        if (animationControl != null && !animationControl.IsPaused)
        {
            animator.speed = 1; // �ָ���������
        }
    }
}
