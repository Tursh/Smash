using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] public AttackState AttackState = AttackState.Idle;
    private CharacterData CharacterData;
    private PlayerPhysics PlayerPhysics;
    private GameObject Hurtbox;
    private Attack[] CurrentAttack;
    private int CurrentAttackFrame = 0;

    void Start()
    {
        PlayerPhysics = GetComponent<PlayerPhysics>();
        CharacterData = GetComponent<CharacterData>();
        

        PlayerPhysics.OnGroundEventHandler += OnGround;
    }

    void FixedUpdate()
    {
        if (AttackState == AttackState.Attacking)
        {
            Attack CurrentFrameAttack = CurrentAttack[CurrentAttackFrame];

            CurrentFrameAttack.Act(gameObject);
            
            ++CurrentAttackFrame;
            if (CurrentAttackFrame >= CurrentAttack.Length)
            {
                DisableAttack();
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


    public void Light()
    {
        if (AttackState == AttackState.Idle)
        {
            if (PlayerPhysics.PlayerState == PlayerState.OnGround)
            {
                CurrentAttack = CharacterData.AttackSet.LightGrounded.Forward;
            }
            else if (PlayerPhysics.PlayerState == PlayerState.InAir)
            {
                CurrentAttack = CharacterData.AttackSet.LightAerial.Forward;
            }

            CurrentAttackFrame = 0;
            AttackState = AttackState.Attacking;
        }
        //HurtboxComponent.Enabled = true;
    }

    
    public void Heavy()
    {
        if (AttackState == AttackState.Idle)
        {
            if (PlayerPhysics.PlayerState == PlayerState.OnGround)
            {
                CurrentAttack = CharacterData.AttackSet.HeavyGrounded.Forward;
            }
            else if (PlayerPhysics.PlayerState == PlayerState.InAir)
            {
                CurrentAttack = CharacterData.AttackSet.HeavyAerial.Forward;
            }

            CurrentAttackFrame = 0;
            AttackState = AttackState.Attacking;
        }
        //HurtboxComponent.Enabled = true;
    }
}