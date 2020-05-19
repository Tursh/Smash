using System;
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

    private Dictionary<string, TimerFramesOfAttack> TimerFramesOfAttacks;

    [SerializeField] private CharacterState NinjaState;

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

        Attack = new Attack();
        TimerFramesOfAttacks = new Dictionary<string, TimerFramesOfAttack>
        {
            {
                "Kick", new TimerFramesOfAttack(20, new[]
                {
                    new FrameOfAttack(
                        o =>
                        {
                            Vector2 positionOffset = new Vector2((o.transform.rotation.y > 0 ? 1 : -1) * 30, 0.5f);
                            AttackFunctions.SimplePhysicalAttack(
                                new FrameDataPhysical(
                                    new Vector2(o.transform.rotation.y > 0 ? 1 : -1, 0),
                                    positionOffset,
                                    Prefabs[0], radius: 0.5f, framesOfLife: 10))(o);
                            return true;
                        })
                }, 25)
            },
            {
                "Punch 1", new TimerFramesOfAttack(20, new[]
                {
                    new FrameOfAttack(
                        o =>
                        {
                            Vector2 positionOffset = new Vector2((o.transform.rotation.y > 0 ? 1 : -1) * 32, 1.4f);
                            AttackFunctions.SimplePhysicalAttack(
                                new FrameDataPhysical(
                                    new Vector2(o.transform.rotation.y > 0 ? 1 : -1, 0),
                                    positionOffset,
                                    Prefabs[0], radius: 0.35f, framesOfLife: 10))(o);
                            return true;
                        })
                }, 10)
            },
            {
                "Punch 2", new TimerFramesOfAttack(20, new[]
                {
                    new FrameOfAttack(
                        o =>
                        {
                            Vector2 positionOffset = new Vector2((o.transform.rotation.y > 0 ? 1 : -1) * 32, 1.4f);
                            AttackFunctions.SimplePhysicalAttack(
                                new FrameDataPhysical(
                                    new Vector2(o.transform.rotation.y > 0 ? 1 : -1, 0),
                                    positionOffset,
                                    Prefabs[0], radius: 0.35f, framesOfLife: 10))(o);
                            return true;
                        })
                }, 10)
            },
            {
                "Punch 3", new TimerFramesOfAttack(30, new[]
                {
                    new FrameOfAttack(
                        o =>
                        {
                            Vector2 positionOffset = new Vector2((o.transform.rotation.y > 0 ? 1 : -1) * 30, 1);
                            AttackFunctions.SimplePhysicalAttack(
                                new FrameDataPhysical(
                                    new Vector2(0, 1),
                                    positionOffset,
                                    Prefabs[1], radius: 0.5f, framesOfLife: 10, damage:1, multiplier:10))(o);
                            return true;
                        })
                }, 20)
            }
        };
    }

    protected override void FixedUpdate()
    {
        Vector2 velocity = Rigidbody.velocity;

        if (AttackState == AttackState.Attacking)
            LeftJoystickPosition.x = 0;

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
            SetRotation(Quaternion.AngleAxis(Rigidbody.velocity.x > 0 ? 89 : -89, Vector3.up));
    }


    private int kickCoolDown = 0;

    void kick()
    {
        if (NinjaState == CharacterState.Idle)
        {
            SetAnimatorState(ParameterIDs["Kicking"], true);
            kickCoolDown = 20;
            NinjaState = CharacterState.Kicking;
            if (AttackState == AttackState.Idle)
                Attack.Enqueue(TimerFramesOfAttacks["Kick"]);
            AttackState = AttackState.Attacking;
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
            Attack.Enqueue(TimerFramesOfAttacks["Punch " + punchState]);
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
    protected override void OnA(InputValue value)
    {
        if (value.isPressed && NinjaState == CharacterState.Idle)
        {
            NinjaState = CharacterState.Jumping;
            jumpTimer = 0;
        }
    }

    protected override void OnB(InputValue value)
    {
        if (value.isPressed)
            kick();
    }

    protected override void OnX(InputValue value)
    {
        if (value.isPressed)
            punch();
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == Layers.PLAYER && other.contacts[0].normal.y > 0.1f)
        {
            Rigidbody.velocity += Vector2.up * 60;
        }
    }
}