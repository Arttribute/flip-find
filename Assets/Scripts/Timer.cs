using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Timer : MonoBehaviour
{
    public TMP_Text timerText; // Reference to the UI Text component for displaying the timer
    private float elapsedTime;

    void Update()
    {
        elapsedTime += Time.deltaTime; // Increment elapsedTime by the time that has passed since the last frame
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        //int milliseconds = Mathf.FloorToInt((elapsedTime * 1000F) % 1000F);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetTimer()
    {
        elapsedTime = 0;
    }
}
