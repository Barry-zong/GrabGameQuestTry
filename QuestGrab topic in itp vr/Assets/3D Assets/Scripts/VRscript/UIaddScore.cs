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
        UpdateScoreText(); // 初始化 UI
        winSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // 检查进入的碰撞体是否有 "BoardArea" 标签
        if (other.CompareTag("BoardArea"))
        {
            // 增加分数
            score += 1;
          //  Debug.Log("Score: " + score);
            UpdateScoreText();
            winSource.Play();
            Debug.Log("audioShouldPlay");
        }
       
    }
    private void UpdateScoreText()
    {
      //  scoreText.text = "Score: " + score.ToString(); // 将 score 转换为字符串
    }
}
