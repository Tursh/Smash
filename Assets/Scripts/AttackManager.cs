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
            HurtboxComponent.Multiplier = currentFrame.Multiplier;
            HurtboxCollider.size = currentFrame.BoxSize;

            if (PlayerPhysics.Facing == Vector2.left)
            {
                HurtboxComponent.Direction = new Vector2(- currentFrame.Direction.x,currentFrame.Direction.y);
                HurtboxCollider.offset = new Vector2(- currentFrame.Offset.x,currentFrame.Offset.y);
            }
            else
            {
                HurtboxComponent.Direction = currentFrame.Direction;
                HurtboxCollider.offset = currentFrame.Offset;
            }
            
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
        HurtboxComponent.Enabled = true;
    }

    
    public void Heavy()
    {
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
        HurtboxComponent.Enabled = true;
    }
}