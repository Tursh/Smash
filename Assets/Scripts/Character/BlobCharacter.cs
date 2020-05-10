using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class BlobCharacter : CharacterData
{
    private enum CharacterState
    {
        Idle,
        Jumping,
        Falling ,
        Running
    }

    private Dictionary<string, int> AnimationIDs = new Dictionary<string, int>();

    private static FrameOfAttack[] AAttack;
    private CharacterState BlobState;

    private CharacterState PreviousState;

    static BlobCharacter()
    {
        AAttack = new FrameOfAttack[]
        {
            AttackFunctions.SimplePhysicalAttack(new FrameDataPhysical())
        };
    }

    protected void Start()
    {
        BlobState = CharacterState.Idle;
        AnimationIDs.Add("IsRunning", Animator.StringToHash("IsRunning"));
        AnimationIDs.Add("Falling", Animator.StringToHash("Falling"));
        AnimationIDs.Add("Jumping", Animator.StringToHash("Jumping"));
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (BlobState == CharacterState.Jumping)
            if (jumpTimer++ > JumpWindup)
                Jump();

        Vector2 velocity = Rigidbody.velocity;

        //Animator.SetFloat("Velocity", Mathf.Abs(velocity.x)*0.4f);

        //If blob is not on ground, set falling animation true
        SetAnimatorState(AnimationIDs["Falling"], GroundPlatform == null);
        //If the blob is idle and start moving, set to running animation
        SetAnimatorState(AnimationIDs["IsRunning"], Mathf.Abs(velocity.x) > 0.1f);
        
        SetAnimatorState(AnimationIDs["Jumping"], BlobState == CharacterState.Jumping);

        Rigidbody.velocity = velocity;
    }

    protected override void Jump()
    {
        Vector2 velocity = Rigidbody.velocity;
        BlobState = CharacterState.Falling;
        Rigidbody.velocity = velocity + Vector2.up * JumpMultiplier;
    }

    private int jumpTimer = 0;

    protected override void KeySpaceOnperformed(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton())
        {
            BlobState = CharacterState.Jumping;
            jumpTimer = 0;
        }
    }

    protected override void AOnperformed(InputAction.CallbackContext ctx)
    {
    }
    
}