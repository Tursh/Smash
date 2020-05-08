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

    void Start()
    {
        CharacterRenderType = CharacterRenderType.Sprite;
        Attack = new Attack();

        Animations = new Dictionary<string, CstmAnimation>
        {
            {"B", new CstmAnimation( (o,stp)=>
                {
                    float value = o.transform.eulerAngles.z;
                    value += 10f;
                    o.transform.eulerAngles = new Vector3(0,0,value);
                    return true;
                }, 360/10)
                
            }
        };
        
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
                                    o.transform.eulerAngles.z + 90f)*20f;
                            return true;
                        }), 
                })
            },
            {
                "B",new TimerFramesOfAttack( 60,new []
                {
                    new FrameOfAttack(
                        o =>
                        {
                            Instantiate(Prefabs[3]);
                            return true;
                        }),
                    
                }, 30)
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
                                                (new Vector3(vectorizedAngleOfAttackOffset.x,
                                                    vectorizedAngleOfAttackOffset.y)) * 1.0f;

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
            if (!firedLastTime && TimerFramesOfAttacks["LT"].CanAttack())
            {
                Attack.Push(TimerFramesOfAttacks["LT"]);
                firedLastTime = true;
            }
            else
                firedLastTime = true;
        }
        else
        {
            firedLastTime = false;
        }

        
        foreach (var anim in Animations)
        {
            if (!anim.Value.Done)
            {
                anim.Value.Step(gameObject);
            }
        }
        if (!AnimationIsPlaying())
            transform.Rotate(0,0, RotateGradually(rotationalTarget, 0.05f));
        
        velocity *= airResistance;

        Rigidbody.velocity = velocity;
        EvaluateAttacks(gameObject);
    }

    protected override void AOnperformed(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton() && TimerFramesOfAttacks["A"].CanAttack())
        {
            Attack.Push(TimerFramesOfAttacks["A"]);
            AttackState = AttackState.Attacking;
        }
    }
    
    protected override void BOnperformed(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton() && TimerFramesOfAttacks["B"].CanAttack())
        {
            Attack.Push(TimerFramesOfAttacks["B"]);
            Animations["B"].Start();
            AttackState = AttackState.Attacking;
        }
    }

    protected override void XOnperformed(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton() && TimerFramesOfAttacks["X"].CanAttack())
        {
            Attack.Push(TimerFramesOfAttacks["X"]);
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

    public bool AnimationIsPlaying()
    {
        return Animations.Any(o => o.Value.Done);
    }
}
