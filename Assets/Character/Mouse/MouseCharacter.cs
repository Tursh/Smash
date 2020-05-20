using System.Collections.Generic;
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
        InitialDamage = 2f;
        PlayerInfo.Damage = InitialDamage;
    }

    void Start()
    {
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
                                    Utils.Degree2Vec2(o.transform.eulerAngles.z - 90f), 
                                    positionOffset,
                                    Prefabs[0],multiplier: 30f))(o);
                            Vector3 spawnPosition = o.transform.position +
                                                    Utils.Vec22Vec3(
                                                        Utils.Degree2Vec2(
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
                                Utils.Degree2Vec2(
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
                        Vector2 vectorizedAngleOfAttack = Utils.Degree2Vec2(angleOfAttack);
                        Vector2 vectorizedAngleOfAttackOffset = Utils.Degree2Vec2(angleOfAttack + 15f);
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
                        projectile.Multiplier = 30f;
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

        
        transform.Rotate(0,0, Utils.RotateGradually(transform.eulerAngles.z,rotationalTarget, 0.05f));
        
        velocity *= airResistance;

        Rigidbody.velocity = velocity;
        EvaluateAttacks(gameObject);
        
        BlinkingBehaviour.isBlinking = InvulnerabilityTimer-- > 0;
        
    }

    protected override void OnA(InputValue value)
    {
        if (value.isPressed && TimerFramesOfAttacks["A"].CanAttack() && AttackState != AttackState.Attacking)
        {
            Attack.Enqueue(TimerFramesOfAttacks["A"]);
            AttackState = AttackState.Attacking;
        }
    }

    protected override void OnX(InputValue value)
    {
        if (value.isPressed && TimerFramesOfAttacks["X"].CanAttack() && AttackState != AttackState.Attacking)
        {
            Attack.Enqueue(TimerFramesOfAttacks["X"]);
            AttackState = AttackState.Attacking;
        }
    }

    public override void Hurt(Vector2 Direction, float Multiplier, float Damage, bool SetKnockback = false)
    {
        if (InvulnerabilityTimer < 0)
        {
            PlayerInfo.Damage -= Damage;
            Rigidbody.velocity += Direction * (Multiplier * (1 + (SetKnockback ? 0 : 2f - PlayerInfo.Damage)));
            if (PlayerInfo.Damage < 0f)
                Die();
        }
    }

    protected override void Die()
    {
        Instantiate(Prefabs[4]);
        base.Die();
        if (PlayerInfo.Stocks <= 0)
            Lose();
    }
}
