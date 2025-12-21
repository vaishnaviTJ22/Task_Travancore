using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI scoreText;
    public Slider healthSlider;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI waveNumberText;

    public Button restartBtn;
    public Button homeBtn;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        restartBtn.onClick.AddListener(Restart);
        homeBtn.onClick.AddListener(Home);
    }

    void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GamePlay");
    }

    void Home()
    {
        SceneManager.LoadScene("Home");
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateHealthBar(int current, int max)
    {
        healthSlider.value = (float)current / max;
    }

    public void UpdateWaveUI(int waveNumber)
    {
        waveNumberText.gameObject.SetActive(true);
        waveNumberText.text = $"wavenumber: {waveNumber.ToString()}";
        Debug.Log("wave ui");
        StartCoroutine(DelayDisableWaveTxt());
    }

    IEnumerator DelayDisableWaveTxt()
    {
        Debug.Log("delay");
        yield return new WaitForSeconds(2f);
        waveNumberText.gameObject.SetActive(false);
    }

    public void ShowGameOver(int score, int highScore)
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Final Score: " + score;
        highScoreText.text = "High Score: " + highScore;
    }
}
