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
            new ColliderFixInfo(new Vector2(1.084f, 1.521f), new Vector2(-0.206f, 0.099f)));
        BoxCollider2DFix.AddBoxColliderFix("Running",
            new ColliderFixInfo(new Vector2(1, 1.664f), new Vector2(0, -0.193f)));

        Attack = new Attack();
        TimerFramesOfAttacks = new Dictionary<string, TimerFramesOfAttack>
        {
            {
                "Kick", new TimerFramesOfAttack(1, new[]
                {
                    new FrameOfAttack(
                        o =>
                        {
                            Vector2 positionOffset = new Vector2((o.transform.rotation.y > 0 ? 1 : -1) * 30, 0.5f);
                            AttackFunctions.SimplePhysicalAttack(
                                new FrameDataPhysical(
                                    new Vector2(o.transform.rotation.y > 0 ? 1 : -1, 0),
                                    positionOffset,
                                    Prefabs[0], radius: 0.5f, framesOfLife: 10, setKnockBack: true, damage:0.05f, multiplier: 3))(o);
                            return true;
                        })
                }, 25)
            },
            {
                "Punch 1", new TimerFramesOfAttack(1, new[]
                {
                    new FrameOfAttack(
                        o =>
                        {
                            Vector2 positionOffset = new Vector2((o.transform.rotation.y > 0 ? 1 : -1) * 32, 1.24f);
                            AttackFunctions.SimplePhysicalAttack(
                                new FrameDataPhysical(
                                    new Vector2(o.transform.rotation.y > 0 ? 1 : -1, 0),
                                    positionOffset,
                                    Prefabs[0], radius: 0.51f, framesOfLife: 10, damage: 0.01f))(o);
                            return true;
                        })
                }, 10)
            },
            {
                "Punch 2", new TimerFramesOfAttack(1, new[]
                {
                    new FrameOfAttack(
                        o =>
                        {
                            Vector2 positionOffset = new Vector2((o.transform.rotation.y > 0 ? 1 : -1) * 32, 1.24f);
                            AttackFunctions.SimplePhysicalAttack(
                                new FrameDataPhysical(
                                    new Vector2(o.transform.rotation.y > 0 ? 1 : -1, 0),
                                    positionOffset,
                                    Prefabs[0], radius: 0.51f, framesOfLife: 10, damage: 0.01f))(o);
                            return true;
                        })
                }, 10)
            },
            {
                "Punch 3", new TimerFramesOfAttack(1, new[]
                {
                    new FrameOfAttack(
                        o =>
                        {
                            Vector2 positionOffset = new Vector2((o.transform.rotation.y > 0 ? 1 : -1) * 30, 1);
                            AttackFunctions.SimplePhysicalAttack(
                                new FrameDataPhysical(
                                    new Vector2(0, 1),
                                    positionOffset,
                                    Prefabs[0], radius: 0.5f, framesOfLife: 10, damage: 0.2f, multiplier: 20,
                                    setKnockBack: true))(o);
                            return true;
                        })
                }, 20)
            }
        };
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (AttackState == AttackState.Attacking)
            Rigidbody.velocity = new Vector2(0, Rigidbody.velocity.y);

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

        if (bounceTimer-- > 0 && NinjaState == CharacterState.Jumping)
        {
            NinjaState = CharacterState.Idle;
            Jump();
            bouncedPlayer.GetComponent<CharacterData>().Hurt(Vector2.down, bounceCombo++ * .5f , 0.02f * bounceCombo++, true);
        }

        if (bounceCombo > 0 && GroundPlatform != null)
            bounceCombo = 0;

        //Animator.SetFloat("Velocity", Mathf.Abs(velocity.x)*0.4f);

        //If ninja is not on ground, set falling animation true
        SetAnimatorState(ParameterIDs["Falling"], GroundPlatform == null);
        //If the ninja is idle and start moving, set to running animation
        SetAnimatorState(ParameterIDs["Running"], Mathf.Abs(Rigidbody.velocity.x) > 0.1f);

        SetAnimatorState(ParameterIDs["Jumping"], NinjaState == CharacterState.Jumping);

        if (Math.Abs(Rigidbody.velocity.x) > 1)
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

    private int bounceTimer = 0, bounceCombo = 0;
    private GameObject bouncedPlayer = null;
    
    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == Layers.PLAYER && GroundPlatform == null)
        {
            Rigidbody.velocity += Vector2.up * 30;
            bounceTimer = 20;
            bouncedPlayer = other.gameObject;
        }
    }

    public override void Hurt(Vector2 Direction, float Multiplier, float Damage, bool SetKnockback = false)
    {
        base.Hurt(Direction, Multiplier, Damage, SetKnockback);
    }
}