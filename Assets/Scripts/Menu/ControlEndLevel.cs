using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlEndLevel : MonoBehaviour
{
    public TextMeshProUGUI mensajeFinalTexto;
    private GameDataControl gameData;

    void Start()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameDataControl>();
        string mensajeFinal = (gameData.IsWinner) ? "You Win !" : "You Lose !";
        if (gameData.IsWinner) mensajeFinal += "\n\n- Level results -\n\nScore: " + gameData.Score + "\n\nExpended time: " + gameData.TimeExpended + " seconds";
        mensajeFinalTexto.text = mensajeFinal;

    }
}
