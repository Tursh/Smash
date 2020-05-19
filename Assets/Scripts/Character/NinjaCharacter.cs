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

        Attack = new Attack();
        TimerFramesOfAttacks = new Dictionary<string, TimerFramesOfAttack>
        {
            {
                "Kick", new TimerFramesOfAttack(60, new[]
                {
                    new FrameOfAttack(
                        o =>
                        {
                            Vector2 positionOffset = new Vector2(-0.5f, 1);
                            AttackFunctions.SimplePhysicalAttack(
                                new FrameDataPhysical(
                                    Utils.Degree2Vec2(o.transform.rotation.z),
                                    positionOffset,
                                    Prefabs[0]))(o);
                            return true;
                        }),
                })
            },
            {
                "X", new TimerFramesOfAttack(120, new[]
                {
                    new FrameOfAttack(
                        o =>
                        {
                            o.GetComponent<Rigidbody2D>().velocity =
                                Utils.NormalizedVectorFromAngle(
                                    o.transform.eulerAngles.z + 90f) * 40f;
                            return true;
                        }),
                })
            },
            {
                "LT", new TimerFramesOfAttack(120, new[]
                {
                    new FrameOfAttack(o =>
                    {
                        MouseCharacter mouseCharacter = o.GetComponent<MouseCharacter>();
                        float angleOfAttack = o.transform.eulerAngles.z + 90f;
                        Vector2 vectorizedAngleOfAttack = Utils.NormalizedVectorFromAngle(angleOfAttack);
                        Vector2 vectorizedAngleOfAttackOffset = Utils.NormalizedVectorFromAngle(angleOfAttack + 15f);
                        Vector2 spawnPosition = o.transform.position +
                                                Utils.Vec22Vec3(vectorizedAngleOfAttackOffset) * 1.0f;

                        GameObject projectileThrown = Instantiate(
                            mouseCharacter.Prefabs[1],
                            spawnPosition,
                            o.transform.rotation);
                        Projectile projectile = projectileThrown.GetComponent<Projectile>();
                        projectile.Damage = 0.1f;
                        projectile.Direction = vectorizedAngleOfAttack;
                        projectile.Velocity = vectorizedAngleOfAttack;
                        projectile.Multiplier = 0.5f;
                        projectile.FramesOfLife = 100;
                        projectile.Source = o;

                        return true;
                    })
                })
            }
        };
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
}