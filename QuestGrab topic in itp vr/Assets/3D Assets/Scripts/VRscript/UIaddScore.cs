using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIaddScore : MonoBehaviour
{
    public float score = 0;
   // public Text scoreText;
    private AudioSource winSource;
    private void Start()
    {
        UpdateScoreText(); // ��ʼ�� UI
        winSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // ���������ײ���Ƿ��� "BoardArea" ��ǩ
        if (other.CompareTag("BoardArea"))
        {
            // ���ӷ���
            score += 1;
          //  Debug.Log("Score: " + score);
            UpdateScoreText();
            winSource.Play();
            Debug.Log("audioShouldPlay");
        }
       
    }
    private void UpdateScoreText()
    {
      //  scoreText.text = "Score: " + score.ToString(); // �� score ת��Ϊ�ַ���
    }
}
