using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private Button musicBtn;

    [SerializeField]
    private Sprite soundOff, soundOn;

    public void PlayGame()
    {
        GameManager.instance.gameStartedFromMainMenu = true;
        SceneManager.LoadScene("Gameplay");
    }

    public void ControlMusic()
    {
        if (GameManager.instance.canPlayMusic)
        {
            musicBtn.image.sprite = soundOff;
            GameManager.instance.canPlayMusic = false;
        }
        else
        {
            musicBtn.image.sprite = soundOn;
            GameManager.instance.canPlayMusic = true;
        }
    }
}
