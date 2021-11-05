using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataControl : MonoBehaviour
{
    private int score;
    private string nextLevelName;
    private bool ganado;
    private int timeExpended;

    public int Score { get => score; set => score = value; }
    public string NextLevelName { get => nextLevelName; set => nextLevelName = value; }
    public bool IsWinner { get => ganado; set => ganado = value; }
    public int TimeExpended { get => timeExpended; set => timeExpended = value; }

    private void Awake()
    {
        int numInstaces = FindObjectsOfType<GameDataControl>().Length;

        if (numInstaces != 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }

    }
}
