using Unity.VisualScripting;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private const string LevelKey = "Level";
    private const string ScoreKey = "Score";
    private const string HighScoreKey = "HighScore";

    public void SaveGame(int currentLevel, int currentScore)
    {
        int highScore = PlayerPrefs.GetInt(HighScoreKey);

        PlayerPrefs.SetInt(LevelKey, currentLevel);
        PlayerPrefs.SetInt(ScoreKey, currentScore);
        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, currentScore);
        }

        PlayerPrefs.Save(); //Ensures data is written to disk
        Debug.Log("Game data saved!");
    }


    public void LoadGame()
    {
        int savedLevel = PlayerPrefs.GetInt(LevelKey, 0);
        int savedScore = PlayerPrefs.GetInt(ScoreKey, 0);

        Debug.Log($"Loaded level: {savedLevel}, score: {savedScore}");

        //Apply loaded data to game state as needed
        if (CollectablesManager.instance != null)
        {
            CollectablesManager.instance.SetLevel(savedLevel);
        }
        else
        {
            Debug.LogError("CollectablesManager.instance is null.");
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.SetScore(savedScore);
        }
        else
        {
            Debug.LogError("UIManager.Instance is null.");
        }


    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(HighScoreKey, 0);
    }



    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            // Save game when the application is paused (e.g player switches apps)
            SaveGame(CollectablesManager.instance.CurrentLevel, UIManager.Instance.CurrentScore);
        }
    }

    private void OnApplicationQuit()
    {
        //Save game when application is about to quit
        SaveGame(CollectablesManager.instance.CurrentLevel, UIManager.Instance.CurrentScore);
    }
}