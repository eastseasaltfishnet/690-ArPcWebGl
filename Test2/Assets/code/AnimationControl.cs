using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    private Animator animator;
    private bool isPaused = false;

    void Start()
    {
        // 获取 Animator 组件
        animator = GetComponent<Animator>();
    }

    // 播放动画
    public void PlayAnimation()
    {
        animator.speed = 1f; // 设置播放速度为正常速度
        isPaused = false;
    }

    // 暂停动画
    public void PauseAnimation()
    {
        animator.speed = 0f; // 设置播放速度为 0 来暂停动画
        isPaused = true;
    }

    // 切换播放/暂停状态
    public void TogglePlayPause()
    {
        if (isPaused)
        {
            PlayAnimation();
        }
        else
        {
            PauseAnimation();
        }
    }

    // 公开的只读属性，用于访问 isPaused 状态
    public bool IsPaused
    {
        get { return isPaused; }
    }
}
