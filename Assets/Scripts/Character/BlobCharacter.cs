using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class BlobCharacter : CharacterData
{
    public float JumpWindup = 999;
    public float JumpMultiplier = 10;

    private enum CharacterState
    {
        Idle, Jumping, Falling, Running
    }
    
    private static FrameOfAttack[] AAttack;
    private CharacterState BlobState;
    private CharacterState PreviousState;
    private BoxCollider2D feet;
    public bool Falling;
    
    static BlobCharacter()
    {
        AAttack = new FrameOfAttack[]
        {
            AttackFunctions.SimplePhysicalAttack(new FrameDataPhysical())
        };
    }
    
    public void OnGround()
    {
        Animator.SetBool("Falling", false);
    }
    public void InAir()
    {
        Animator.SetBool("Falling", true);
    }

    protected void Start()
    {
        BlobState = CharacterState.Idle;
        feet = GetComponentInChildren<BoxCollider2D>();
    }

    protected override void FixedUpdate()
    { 
        base.FixedUpdate();

        if (BlobState == CharacterState.Jumping )
            if (jumpTimer++ > JumpWindup)
                Jump();

        Vector2 velocity = Rigidbody.velocity;

        //Animator.SetFloat("Velocity", Mathf.Abs(velocity.x)*0.4f);

        if (Mathf.Abs(velocity.x) < 0.1f)
            Animator.SetBool("IsRunning", false);
        else
            Animator.SetBool("IsRunning", true);
        
        if (BlobState == CharacterState.Jumping)
            Animator.SetBool("Jumping", true);
        else 
            Animator.SetBool("Jumping", false);
        Rigidbody.velocity = velocity;
    }

    private float jumpTimer;
    protected override void Jump()
    {
        Vector2 velocity = Rigidbody.velocity;
        BlobState = CharacterState.Falling;
        Rigidbody.velocity = velocity + Vector2.up * JumpMultiplier;
    }

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
