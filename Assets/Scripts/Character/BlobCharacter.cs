using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum CharacterState
{
    Idle,
    Jumping,
    Falling,
    Running
}

public class BlobCharacter : CharacterData
{
    public float JumpWindup = 20;
    public float JumpMultiplier = 10;

    private static FrameOfAttack[] AAttack;
    public CharacterState BlobState;

    public CharacterState PreviousState;
    private BoxCollider2D feet;
    public bool Falling;
    public Vector2 facing;

    protected void Start()
    {
        BlobState = CharacterState.Idle;
        feet = GetComponentInChildren<BoxCollider2D>();
        facing = Vector2.left;
        CharacterRenderType = CharacterRenderType.Model;
        
        AAttack = new FrameOfAttack[]
        {

        };
    }
    

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (BlobState != CharacterState.Falling)
        {
            Vector2 previousFacing = facing;

            if (LeftJoystickPosition.x > 0.2f)
                facing = Vector2.right;
            else if (LeftJoystickPosition.x < -0.2f)
                facing = Vector2.left;

            if (previousFacing != facing)
                TriggerAnimatorState("Turn");
            
            SetAnimatorState("Turned", facing == Vector2.right);
        }

        if (BlobState == CharacterState.Jumping)
            if (jumpTimer++ > JumpWindup)
                Jump();

        Vector2 velocity = Rigidbody.velocity;

        //Animator.SetFloat("Velocity", Mathf.Abs(velocity.x)*0.4f);

        //If blob is not on ground, set falling animation true
        if (GroundPlatform == null)
        {
            SetAnimatorState("Falling", true);
            BlobState = CharacterState.Falling;
        }
        else
        {
            SetAnimatorState("Falling", false);
            BlobState = CharacterState.Idle;
        }
        
        //If the blob is idle and start moving, set to running animation
        SetAnimatorState("IsRunning", Mathf.Abs(velocity.x) > 0.1f);
        
        SetAnimatorState("Jumping", BlobState == CharacterState.Jumping);

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