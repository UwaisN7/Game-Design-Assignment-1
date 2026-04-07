using System;
using TMPro;
using UnityEngine;


public class Timer : MonoBehaviour
{

    public TextMeshProUGUI timerText;


    public float time;
    private bool rankActive;
    private bool timerActive;
    void Start()
    {
        timerActive = false;
        time = 0f;

        rankActive = false;
        if (timerText != null)
            timerText.text = "0.00";
    }


    void Update()
    {
        if (timerActive)
        {
            StartTimer();

        }

        if (!timerActive&& rankActive  )
        {
            StopTimer();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("StartTime"))
        {
            timerActive = true;
        }

        if (other.CompareTag("StopTimer"))
        {
            timerActive = false;
            rankActive = true;  
        }
    }



    void StartTimer()
    {
        time += Time.deltaTime;

        if (timerText != null)
        {
            timerText.text = time.ToString("F2");
        }
    }
    void StopTimer()
    {
        string rank = GetRank(time);
       

        if (timerText != null)
        {
            timerText.text = $"Time: {time:F2}\nRank: {rank}";
        }
    }

    string GetRank(float time)
    {
        if (time <= 70f) 
            return "Nice! You got an S-rank";
        else if (time <= 90f) 
            return "A-rank you are good!";
        else if (time <= 120f) 
            return "B-rank you did well but you can do better";
        else if (time <= 150f) 
            return "C-rank i believe in you";
        else
            return "F-rank you got this dont worry";
    }
}

