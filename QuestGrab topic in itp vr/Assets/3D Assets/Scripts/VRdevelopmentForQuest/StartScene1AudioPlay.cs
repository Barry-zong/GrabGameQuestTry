using UnityEngine;

public class StartScene1AudioPlay : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private bool m_IsPlaying = false;  // ���ڸ��ٵ�ǰ����״̬
    public bool mainBackgroundMusicPlay = false;

    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // ֻ��״̬�ı�ʱ��ִ�в���/ֹͣ����
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