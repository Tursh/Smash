using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        HurtboxCollider = GetComponentInChildren<BoxCollider2D>();
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

            if (CurrentAttackFrame > CurrentAttack.Length)
            {
                PlayerPhysics.PlayerSubState = PlayerSubState.Idle;
                
            }
            else
            {
                ++CurrentAttackFrame;
                HurtboxComponent.Enabled = false;
            }
        }
    }
    
    public void Light()
    {
        PlayerPhysics.PlayerSubState = PlayerSubState.Attacking;
        HurtboxComponent.Enabled = true;
        if (PlayerPhysics.PlayerState == PlayerState.OnGround)
        {
            CurrentAttack = CharacterData.AttackSet.LightGrounded.Forward;
        }
    }

    public void Heavy()
    {
        PlayerPhysics.PlayerSubState = PlayerSubState.Attacking;
        HurtboxComponent.Enabled = true;
    }

}
