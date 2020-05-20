using System;
using UnityEngine;

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
            GameObject.Find("P1").GetComponent<GeneralUi>().PlayerInfo = this;
        else if (Player == 1)
            GameObject.Find("P2").GetComponent<GeneralUi>().PlayerInfo = this;
    }
}