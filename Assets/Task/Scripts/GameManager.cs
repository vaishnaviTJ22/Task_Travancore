using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;
    public bool isGameOver;

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

    public void GameOver()
    {
        isGameOver = true;
        UIManager.Instance.ShowGameOver(score);
        Time.timeScale = 0f;
    }
}
