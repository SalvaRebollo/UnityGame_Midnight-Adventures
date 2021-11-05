using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDControl : MonoBehaviour
{
    public TextMeshProUGUI healthTxt;
    public TextMeshProUGUI timeTxt;
    public TextMeshProUGUI remainingCoins;

    public void setHealthTxt(int health)
    {
        healthTxt.text = "Health: " + health;
    }

    public void setTimeTxt(int time)
    {
        int seconds = time % 60;
        int minutes = time / 60;
        timeTxt.text = "Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void setCoinsTxt(int coins)
    {
        remainingCoins.text = "Coins: " + coins;
    }
}
