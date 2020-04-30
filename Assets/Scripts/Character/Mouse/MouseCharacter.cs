using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCharacter : CharacterData
{
    private Dictionary<string, FrameOfAttack[]> FramesOfAttacks;
    
    private float rotationalTarget;
    private int shootTimer;
    private int shootTime = 30;
    private bool firedLastTime = false;
    
    void Start()
    {
        CharacterRenderType = CharacterRenderType.Sprite;
        Attack = new Attack();
        FramesOfAttacks = new Dictionary<string, FrameOfAttack[]>
        {
            {
                "A", new[]
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
                }
            },
            {
                "LT", new[]
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
                }
            }
        };
    }

    protected override void FixedUpdate()
    {
        var velocity = Rigidbody.velocity;
        shootTimer++;
        if (LTPosition > 0.7f)
        {
            if (LeftJoystickPosition.normalized.magnitude > 0.3f)
                rotationalTarget = Utils.Vec22Degree(LeftJoystickPosition);
        }
        else
            velocity += LeftJoystickPosition;

        if (RTPosition > 0.7f)
        {
            if (!firedLastTime && shootTimer > shootTime)
            {
                Attack.Push(FramesOfAttacks["LT"]);
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

        EvaluateAttacks(gameObject);
        Rigidbody.velocity = velocity;
    }

    protected override void AOnperformed(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton())
        {
            Attack.Push(FramesOfAttacks["A"]);
            AttackState = AttackState.Attacking;
        }
    }

    private float RotateGradually(float targetDegrees, float scale)
    {
        float gradualRotation = -transform.localRotation.eulerAngles.z + targetDegrees;
        
        if (gradualRotation > 180f)
            gradualRotation -= 360f;
        else if (gradualRotation < -180f)
            gradualRotation += 360f;

        gradualRotation *= scale;

        return gradualRotation;
    }


}
