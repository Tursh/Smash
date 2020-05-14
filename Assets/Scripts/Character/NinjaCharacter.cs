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
        Kicking,
        Punching
    }

    [SerializeField] private BoxCollider2DFix BoxCollider2DFix;

    private Dictionary<string, int> ParameterIDs = new Dictionary<string, int>();

    private static FrameOfAttack[] AAttack;
    private CharacterState NinjaState;

    static NinjaCharacter()
    {
    }

    protected void Start()
    {
        NinjaState = CharacterState.Idle;
        ParameterIDs.Add("Running", Animator.StringToHash("Running"));
        ParameterIDs.Add("Falling", Animator.StringToHash("Falling"));
        ParameterIDs.Add("Jumping", Animator.StringToHash("Jumping"));
        ParameterIDs.Add("Kicking", Animator.StringToHash("Kicking"));
        ParameterIDs.Add("PunchAttackState", Animator.StringToHash("PunchAttackState"));

        BoxCollider2DFix.AddBoxColliderFix("Idle",
            new ColliderFixInfo(new Vector2(1, 1.705f), new Vector2(0, -0.016f)));
        BoxCollider2DFix.AddBoxColliderFix("Jumping",
            new ColliderFixInfo(new Vector2(1, 1.316f), new Vector2(-0.1f, -0.021f)));
        BoxCollider2DFix.AddBoxColliderFix("Falling",
            new ColliderFixInfo(new Vector2(1.485f, 1.521f), new Vector2(-0.406f, 0.099f)));
        BoxCollider2DFix.AddBoxColliderFix("Running",
            new ColliderFixInfo(new Vector2(1, 1.664f), new Vector2(0, -0.193f)));
    }

    protected override void FixedUpdate()
    {
        Vector2 velocity = Rigidbody.velocity;

        base.FixedUpdate();

        if (NinjaState == CharacterState.Jumping)
            if (jumpTimer++ > JumpWindup)
                Jump();

        if (NinjaState == CharacterState.Kicking && kickCoolDown-- == 0)
        {
            SetAnimatorState(ParameterIDs["Kicking"], false);
            NinjaState = CharacterState.Idle;
        }

        if (NinjaState == CharacterState.Punching && punchCoolDown-- == 0)
        {
            SetAnimatorState(ParameterIDs["PunchAttackState"], 0);
            punchState = 0;
            NinjaState = CharacterState.Idle;
        }


        //Animator.SetFloat("Velocity", Mathf.Abs(velocity.x)*0.4f);

        //If ninja is not on ground, set falling animation true
        SetAnimatorState(ParameterIDs["Falling"], GroundPlatform == null);
        //If the ninja is idle and start moving, set to running animation
        SetAnimatorState(ParameterIDs["Running"], Mathf.Abs(velocity.x) > 0.1f);

        SetAnimatorState(ParameterIDs["Jumping"], NinjaState == CharacterState.Jumping);

        if (velocity.x != 0)
            SetRotation(Quaternion.AngleAxis(Rigidbody.velocity.x > 0 ? 90 : -90, Vector3.up));
    }


    private int kickCoolDown = 0;

    void kick()
    {
        if (NinjaState == CharacterState.Idle)
        {
            SetAnimatorState(ParameterIDs["Kicking"], true);
            kickCoolDown = 20;
            NinjaState = CharacterState.Kicking;
        }
    }


    private int punchCoolDown = 0,
        punchState = 0;

    private float lastPunch;

    void punch()
    {
        if ((NinjaState == CharacterState.Idle || NinjaState == CharacterState.Punching) &&
            (punchState == 0 || (Time.time - lastPunch > 0.15f && punchState != 3)))
        {
            lastPunch = Time.time;
            ++punchState;
            punchCoolDown = 20;
            SetAnimatorState(ParameterIDs["PunchAttackState"], punchState);
            NinjaState = CharacterState.Punching;
        }
    }

    protected override void Jump()
    {
        Vector2 velocity = Rigidbody.velocity;
        NinjaState = CharacterState.Idle;
        Rigidbody.velocity = velocity + Vector2.up * JumpMultiplier;
    }

    private int jumpTimer = 0;

    protected override void OnKeySpace(InputValue value)
    {
        if (value.isPressed && NinjaState == CharacterState.Idle)
        {
            NinjaState = CharacterState.Jumping;
            jumpTimer = 0;
        }
    }

    protected override void OnA(InputValue value)
    {
        OnKeySpace(value);
    }

    protected override void OnB(InputValue value)
    {
        if(value.isPressed)
        kick();
    }

    protected override void OnX(InputValue value)
    {
        if(value.isPressed)
        punch();
    }
}