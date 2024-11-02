using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    private Animator animator;
    private bool isPaused = false;

    void Start()
    {
        // ��ȡ Animator ���
        animator = GetComponent<Animator>();
    }

    // ���Ŷ���
    public void PlayAnimation()
    {
        if (animator != null)
        {
            Debug.Log("Playing Animation");
            animator.speed = 1f;
            isPaused = false;
        }
        else
        {
            Debug.LogError("Animator is not assigned in PlayAnimation.");
        }


    }

    // ��ͣ����
    public void PauseAnimation()
    {
        animator.speed = 0f; // ���ò����ٶ�Ϊ 0 ����ͣ����
        isPaused = true;
    }

    // �л�����/��ͣ״̬
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

    // ������ֻ�����ԣ����ڷ��� isPaused ״̬
    public bool IsPaused
    {
        get { return isPaused; }
    }
}
