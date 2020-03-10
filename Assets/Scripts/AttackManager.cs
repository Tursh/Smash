using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public enum AttackState
{
    Idle,
    Attacking
}

public class AttackManager : MonoBehaviour
{
    public AttackState AttackState = AttackState.Idle;
    private CharacterData CharacterData;
    private PlayerPhysics PlayerPhysics;
    private Attack[] CurrentAttack;
    private int CurrentAttackFrame = 0;

    void Start()
    {
        PlayerPhysics = GetComponent<PlayerPhysics>();
        CharacterData = GetComponent<CharacterData>();

        PlayerPhysics.OnGroundEventHandler += OnGround;
        
        CurrentAttack = new Attack[] {};
    }

    void FixedUpdate()
    {

        if (AttackState == AttackState.Attacking)
        {
            if (CurrentAttackFrame >= CurrentAttack.Length)
                DisableAttack();
            else
            {
                CurrentAttack[CurrentAttackFrame].Act(gameObject);
                ++CurrentAttackFrame;
            }
        }
    }

    void DisableAttack()
    {
        AttackState = AttackState.Idle;
        CurrentAttackFrame = 0;
    }

    void OnGround(object sender, OnGroundEnventArgs args)
    {
        DisableAttack();
    }
}