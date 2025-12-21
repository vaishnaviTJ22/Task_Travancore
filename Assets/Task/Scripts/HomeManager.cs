using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    public Button playBtn;
    public Button quitBtn;
    // Start is called before the first frame update
    void Start()
    {
        playBtn.onClick.AddListener(Play);
        quitBtn.onClick.AddListener(Quit);
    }

    void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GamePlay");
    }
    void Quit()
    {
        Application.Quit();
    }
}
