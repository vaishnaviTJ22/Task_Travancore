using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI scoreText;
    public Slider healthSlider;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateHealthBar(int current, int max)
    {
        healthSlider.value = (float)current / max;
    }

    public void ShowGameOver(int score)
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Final Score: " + score;
    }
}
