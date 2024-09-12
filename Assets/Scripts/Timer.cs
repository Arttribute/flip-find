using UnityEngine;
using TMPro;


public class Timer : MonoBehaviour
{
    public TMP_Text timerText; // Reference to the UI Text component for displaying the timer
    [SerializeField] private TMP_Text finalTimerText;
    private float elapsedTime;

    void Update()
    {
        if (Time.timeScale > 0)
        {
            elapsedTime += Time.deltaTime; // Increment elapsedTime by the time that has passed since the last frame
            UpdateTimerUI();
        }

    }



    public void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        //int milliseconds = Mathf.FloorToInt((elapsedTime * 1000F) % 1000F);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public string DisplayFinalTime()
    {

        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        return string.Format("{0:00}:{1:00}", minutes, seconds);

    }

    public void ResetTimer()
    {
        elapsedTime = 0;
        Time.timeScale = 1;
    }

}
