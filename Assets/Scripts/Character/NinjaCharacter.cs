using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NinjaCharacter : CharacterData
{
    private enum CharacterState
    {
        Idle,
        Jumping,
        Falling,
        Landing,
        Running,
        Kicking,
        R_Punch,
        L_Punch,
        UpperCut
    }

    [SerializeField]
    private BoxCollider2DFix BoxCollider2DFix;
    
    private Dictionary<string, int> ParameterIDs = new Dictionary<string, int>();

    private static FrameOfAttack[] AAttack;
    private CharacterState NinjaState;

    private CharacterState PreviousState;

    static NinjaCharacter()
    {
        AAttack = new FrameOfAttack[]
        {
            AttackFunctions.SimplePhysicalAttack(new FrameDataPhysical())
        };
    }

    protected void Start()
    {
        NinjaState = CharacterState.Idle;
        ParameterIDs.Add("Running", Animator.StringToHash("Running"));
        ParameterIDs.Add("Falling", Animator.StringToHash("Falling"));
        ParameterIDs.Add("Jumping", Animator.StringToHash("Jumping"));
        ParameterIDs.Add("Kicking", Animator.StringToHash("Kicking"));
        ParameterIDs.Add("PunchAttackState", Animator.StringToHash("PunchAttackState"));
        
        BoxCollider2DFix.AddBoxColliderFix("Idle", new ColliderFixInfo(new Vector2(1, 1.705f), new Vector2(0, -0.016f)));
        BoxCollider2DFix.AddBoxColliderFix("Jumping", new ColliderFixInfo(new Vector2(1, 1.316f), new Vector2(-0.1f, -0.021f)));
        BoxCollider2DFix.AddBoxColliderFix("Falling", new ColliderFixInfo(new Vector2(1.485f, 1.521f), new Vector2(-0.406f, 0.099f)));
        BoxCollider2DFix.AddBoxColliderFix("Running", new ColliderFixInfo(new Vector2(1, 1.664f), new Vector2(0, -0.193f)));
        
        
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (NinjaState == CharacterState.Jumping)
            if (jumpTimer++ > JumpWindup)
                Jump();

        Vector2 velocity = Rigidbody.velocity;

        //Animator.SetFloat("Velocity", Mathf.Abs(velocity.x)*0.4f);

        //If ninja is not on ground, set falling animation true
        SetAnimatorState(ParameterIDs["Falling"], GroundPlatform == null);
        //If the ninja is idle and start moving, set to running animation
        SetAnimatorState(ParameterIDs["Running"], Mathf.Abs(velocity.x) > 0.1f);

        SetAnimatorState(ParameterIDs["Jumping"], NinjaState == CharacterState.Jumping);

        Rigidbody.velocity = velocity;
    }

    protected override void Jump()
    {
        Vector2 velocity = Rigidbody.velocity;
        NinjaState = CharacterState.Falling;
        Rigidbody.velocity = velocity + Vector2.up * JumpMultiplier;
    }

    private CharacterState lastState = CharacterState.Idle;


    /// <summary>
    /// Since the ninja is a 3d mesh, the box collider don't follow it correctly
    /// </summary>
    private void FixBoxCollider()
    {
        var animationState = Animator.GetCurrentAnimatorStateInfo(0);
        if (animationState.IsName("running") && lastState != CharacterState.Running)
        {
        }
    }

    private int jumpTimer = 0;

    protected override void KeySpaceOnperformed(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton())
        {
            NinjaState = CharacterState.Jumping;
            jumpTimer = 0;
        }
    }

    protected override void AOnperformed(InputAction.CallbackContext ctx)
    {
    }
}