using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCharacter : CharacterData
{
    private Dictionary<string, TimerFramesOfAttack> TimerFramesOfAttacks;
    private Dictionary<string, CstmAnimation> Animations;

    private float rotationalTarget;
    private bool firedLastTime = false;

    protected override void Awake()
    {
        base.Awake();
        PlayerInfo.Damage = 2f;
    }

    void Start()
    {
        CharacterRenderType = CharacterRenderType.Sprite;
        Attack = new Attack();


        TimerFramesOfAttacks = new Dictionary<string, TimerFramesOfAttack>
        {
            {
                "A", new TimerFramesOfAttack(60,new []
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
                            Vector3 spawnPosition = o.transform.position +
                                                    Utils.Vec22Vec3(
                                                        Utils.NormalizedVectorFromAngle(
                                                            o.transform.eulerAngles.z + 105f));

                            Instantiate(Prefabs[2],
                                spawnPosition,
                                o.transform.rotation
                            );
                            return true;
                        }),
                })
            },
            {
                "X",new TimerFramesOfAttack( 120,new []
                {
                    new FrameOfAttack(
                        o =>
                        {
                            o.GetComponent<Rigidbody2D>().velocity =
                                Utils.NormalizedVectorFromAngle(
                                    o.transform.eulerAngles.z + 90f)*40f;
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
        var velocity = Rigidbody.velocity;
        foreach (var pair in TimerFramesOfAttacks)
        {
            TimerFramesOfAttacks[pair.Key].timer++;
        }

        if (LTPosition > 0.7f)
        {
            if (LeftJoystickPosition.normalized.magnitude > 0.3f)
                rotationalTarget = Utils.Vec22Degree(LeftJoystickPosition);
        }
        else
            velocity += LeftJoystickPosition;

        if (RTPosition > 0.7f)
        {
            if (!firedLastTime && TimerFramesOfAttacks["LT"].CanAttack() && AttackState != AttackState.Attacking)
            {
                Attack.Enqueue(TimerFramesOfAttacks["LT"]);
                firedLastTime = true;
            }
            else
                firedLastTime = true;
        }
        else
        {
            firedLastTime = false;
        }

        
        transform.Rotate(0,0, RotateGradually(rotationalTarget, 0.05f));
        
        velocity *= airResistance;

        Rigidbody.velocity = velocity;
        EvaluateAttacks(gameObject);
    }

    protected override void AOnperformed(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton() && TimerFramesOfAttacks["A"].CanAttack() && AttackState != AttackState.Attacking)
        {
            Attack.Enqueue(TimerFramesOfAttacks["A"]);
            AttackState = AttackState.Attacking;
        }
    }

    protected override void XOnperformed(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton() && TimerFramesOfAttacks["X"].CanAttack() && AttackState != AttackState.Attacking)
        {
            Attack.Enqueue(TimerFramesOfAttacks["X"]);
            AttackState = AttackState.Attacking;
        }
    }

    public float RotateGradually(float targetDegrees, float scale)
    {
        float gradualRotation = -transform.localRotation.eulerAngles.z + targetDegrees;
        
        if (gradualRotation > 180f)
            gradualRotation -= 360f;
        else if (gradualRotation < -180f)
            gradualRotation += 360f;

        gradualRotation *= scale;

        return gradualRotation;
    }

    public override void Hurt(Vector2 Direction, float Multiplier, float Damage, bool SetKnockback = false)
    {
        PlayerInfo.Damage -= Damage;

        Rigidbody.velocity += Direction * (Multiplier * (1 + (SetKnockback ? 0 : 2f - PlayerInfo.Damage)));
    }
}
