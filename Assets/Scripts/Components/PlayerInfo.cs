using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInfo : MonoBehaviour
{
    private float damage;
    private int stocks = 4;
    public uint Player = 1;
    public float Damage
    {
        get => damage;
        set
        {
            Modified?.Invoke(this, new DamageEventArg(damage, stocks));
            damage = value;
        }
    }

    public string Name;
    public int Stocks
    {
        get => stocks;
        set
        {
            Modified?.Invoke(this, new DamageEventArg(damage, stocks));
            stocks = value;
        }
    }
    
    public EventHandler<DamageEventArg> Modified;

    public void Awake()
    {
        if (Player == 0)
            GameObject.Find("P1").GetComponent<GeneralUi>().playerInfo = this;
        else if (Player == 1)
            GameObject.Find("P2").GetComponent<GeneralUi>().playerInfo = this;
    }
}