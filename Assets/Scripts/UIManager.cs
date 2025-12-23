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

    private Coroutine waveTextCoroutine;

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
        Time.timeScale = 1f;
        SceneManager.LoadScene("Home");
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateHealthBar(int current, int max)
    {
        healthSlider.value = (float)current / max;
    }

    public void UpdateWaveUI(int waveNumber)
    {
        if (GameManager.Instance.isGameOver) return;

        if (waveTextCoroutine != null)
        {
            StopCoroutine(waveTextCoroutine);
        }

        waveNumberText.transform.parent.gameObject.SetActive(true);
        waveNumberText.text = $"wave {waveNumber.ToString()}";
        Debug.Log("wave ui");
        waveTextCoroutine = StartCoroutine(DelayDisableWaveTxt());
    }

    IEnumerator DelayDisableWaveTxt()
    {
        Debug.Log("delay");
        yield return new WaitForSeconds(2f);

        if (!GameManager.Instance.isGameOver)
        {
            waveNumberText.transform.parent.gameObject.SetActive(false);
        }

        waveTextCoroutine = null;
    }

    public void ShowGameOver(int score, int highScore)
    {
        if (waveTextCoroutine != null)
        {
            StopCoroutine(waveTextCoroutine);
            waveTextCoroutine = null;
        }

        waveNumberText.transform.parent.gameObject.SetActive(false);

        gameOverPanel.SetActive(true);
        finalScoreText.text = "Final Score: " + score;
        highScoreText.text = "High Score: " + highScore;
    }
}
