using UnityEngine;

public class StartScene1AudioPlay : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private bool m_IsPlaying = false;  // 用于跟踪当前播放状态
    public bool mainBackgroundMusicPlay = false;

    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 只在状态改变时才执行播放/停止操作
        if (mainBackgroundMusicPlay && !m_IsPlaying)
        {
            Debug.Log("startmusic");
            m_AudioSource.Play();
            m_AudioSource.loop = true;
            m_IsPlaying = true;
        }
        else if (!mainBackgroundMusicPlay && m_IsPlaying)
        {
            m_AudioSource.Stop();
            m_IsPlaying = false;
        }
    }
    public void StopNormalBackgroundAudio()
    {
        m_AudioSource.Stop();
    }
}