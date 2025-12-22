using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;
    public bool isGameOver;

    private const string HIGH_SCORE_KEY = "HighScore";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void AddScore(int value)
    {
        score += value;
        UIManager.Instance.UpdateScore(score);
    }

    private int GetHighScore()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    private void SaveHighScore(int newScore)
    {
        int currentHighScore = GetHighScore();
        if (newScore > currentHighScore)
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, newScore);
            PlayerPrefs.Save();
        }
    }

    public void GameOver()
    {
        isGameOver = true;

        SaveHighScore(score);
        int highScore = GetHighScore();

        UIManager.Instance.ShowGameOver(score, highScore);
        Time.timeScale = 0f;
    }
}
