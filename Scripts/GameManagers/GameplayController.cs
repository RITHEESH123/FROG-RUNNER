using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;

    [SerializeField]
    private AudioSource audioSource;

    private Text scoreText, healthText, levelText;
    private float score, health, level;

    private BGScrollerScript bgScroller;

    private GameObject pausePanel;

    [HideInInspector]
    public bool canCountScore;

    void Awake()
    {
        MakeInstance();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        levelText = GameObject.Find("LevelText").GetComponent<Text>();

        bgScroller = GameObject.Find("Background").GetComponent<BGScrollerScript>();
        pausePanel = GameObject.Find("PausePanel");
        pausePanel.SetActive(false);
    }
    void Start()
    {
        if (GameManager.instance.canPlayMusic)
        {
            audioSource.Play();
        }
    }
    void Update()
    {
        IncrementScore(1);
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneWasLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneWasLoaded;
        instance = null;
    }

    void OnSceneWasLoaded(Scene scene,LoadSceneMode mode)
    {
        if (scene.name == "Gameplay")
        {
            if (GameManager.instance.gameStartedFromMainMenu)
            {
                GameManager.instance.gameStartedFromMainMenu = false;
                score = 0;
                health = 3;
                level = 0;
            }
            else if (GameManager.instance.gameRestartedPlayerDied)
            {
                GameManager.instance.gameRestartedPlayerDied = false;
                score = GameManager.instance.score;
                health = GameManager.instance.health;
            }
            scoreText.text = score.ToString();
            healthText.text = health.ToString();
            levelText.text = level.ToString();
        }
    }

    public void TakeDamage()
    {
        health--;
        if (health >= 0)
        {
            //restart the game
            StartCoroutine(PlayerDied("Gameplay"));
            healthText.text = health.ToString();
        }
        else
        {
            //Gameover
            StartCoroutine(PlayerDied("MainMenu"));
        }
    }

    public void IncrementHealth()
    {
        health++;
        healthText.text = health.ToString();
    }

    public void IncrementScore(float scoreValue)
    {
        if (canCountScore)
        {
            score += scoreValue;
            scoreText.text = score.ToString();
        }
    }

    IEnumerator PlayerDied(string sceneName)
    {
        canCountScore = false;
        //Stop bg scrolling
        bgScroller.canScroll = false;

        GameManager.instance.score = score;
        GameManager.instance.health = health;
        GameManager.instance.gameRestartedPlayerDied = true;

        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(sceneName);
    }

    public void PauseGame()
    {
        canCountScore = false;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        canCountScore = true;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Reload()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Gameplay");
    }
    
}
