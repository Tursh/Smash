using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AttackManager : MonoBehaviour
{
    private PlayerPhysics PlayerPhysics;
    private CharacterData CharacterData;
    private HurtboxComponent HurtboxComponent;
    private BoxCollider2D HurtboxCollider;
    private Attack[] CurrentAttack;
    private int CurrentAttackFrame = 0;

    void Start()
    {
        PlayerPhysics = GetComponent<PlayerPhysics>();
        CharacterData = GetComponent<CharacterData>();
        HurtboxComponent = GetComponentInChildren<HurtboxComponent>();
        HurtboxCollider = HurtboxComponent.transform.GetComponent<BoxCollider2D>();

        PlayerPhysics.OnGroundEventHandler += OnGround;
    }

    void FixedUpdate()
    {
        if (PlayerPhysics.PlayerSubState == PlayerSubState.Attacking)
        {
            Attack currentFrame = CurrentAttack[CurrentAttackFrame];

            HurtboxComponent.Damage = currentFrame.Damage;
            HurtboxComponent.Direction = currentFrame.Direction;
            HurtboxComponent.Multiplier = currentFrame.Multiplier;

            HurtboxCollider.size = currentFrame.BoxSize;
            HurtboxCollider.offset = currentFrame.Offset;
            
            ++CurrentAttackFrame;
            if (CurrentAttackFrame >= CurrentAttack.Length)
            {
                DisableAttack();
            }
        }
    }

    void DisableAttack()
    {
        PlayerPhysics.PlayerSubState = PlayerSubState.Idle;
        HurtboxComponent.Enabled = false;
        CurrentAttackFrame = 0;
    }
    void OnGround(object sender, OnGroundEnventArgs args)
    {
        DisableAttack();
    }


    public void Light()
    {
        
        HurtboxComponent.Enabled = true;
        if (PlayerPhysics.PlayerSubState == PlayerSubState.Idle)
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
            PlayerPhysics.PlayerSubState = PlayerSubState.Attacking;
        }
    }

    public void Heavy()
    {
        HurtboxComponent.Enabled = true;
        if (PlayerPhysics.PlayerSubState == PlayerSubState.Idle)
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
            PlayerPhysics.PlayerSubState = PlayerSubState.Attacking;
        }
    }
}