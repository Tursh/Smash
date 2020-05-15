using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class DamageEventArg
{
    public DamageEventArg(float damage, int stocks)
    {
        Damage = damage;
        Stocks = stocks;
    }

    public int Stocks;
    public float Damage;
}

public class GeneralUi : MonoBehaviour
{
    public PlayerInfo playerInfo;
    private Text PlayerName;
    private Text PlayerPercent;
    private Text PlayerStocks;
    private void Start()
    {
        var textBoxes = GetComponentsInChildren<Text>();
        foreach (var textBox in textBoxes)
        {
            if (textBox.name.Contains("Name"))
                PlayerName = textBox;
            else if (textBox.name.Contains("%"))
                PlayerPercent = textBox;
            else if (textBox.name.Contains("Stocks"))
                PlayerStocks = textBox;
        }
        
        PlayerName.text = playerInfo.Name;
        PlayerStocks.text = playerInfo.Stocks.ToString();
        PlayerPercent.text = (playerInfo.Damage * 100).ToString("N0") + " %";
        playerInfo.Modified += Modified;
    }

    private void Modified(object sender, DamageEventArg e)
    {
        PlayerStocks.text = e.Stocks.ToString();
        PlayerPercent.text = (e.Damage * 100).ToString("N0") + " %";
    }
}
