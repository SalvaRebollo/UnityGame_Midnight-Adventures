using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ControlButtonsMenu : MonoBehaviour
{

    private GameDataControl gameData;

    void Start()
    {
        if (GameObject.Find("GameData"))
            gameData = GameObject.Find("GameData").GetComponent<GameDataControl>();
    }
    public void onButtonPlay()
    {
        SceneManager.LoadScene("Level1");
    }

    public void onButtonCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void onButtonMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void onButtonNextLevel()
    {
        SceneManager.LoadScene(gameData.NextLevelName);
    }

    public void onButtonExit()
    {
        Application.Quit();
    }

}
