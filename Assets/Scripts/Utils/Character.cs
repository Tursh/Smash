using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore;
using Random = UnityEngine.Random;

public enum CharacterRenderType
{
    Sprite,
    Model
}

public enum Character
{
    None = -1, Mouse = 0 , Ship = 1, Ninja = 2, Target = 3
}

public abstract class CharacterData : MonoBehaviour
{
    [SerializeField] protected float JumpWindup = 20;
    [SerializeField] protected float JumpMultiplier = 10;
    [SerializeField] protected float mvSpeed = 0.025f;
    [SerializeField] protected float airResistance = 0.9f;
    [SerializeField] protected float gravity = -0.05f;
    [SerializeField] protected float distanceToGround = 1;
    [SerializeField] protected float InitialDamage = 0f;
    public GameObject[] Prefabs;
    
    //This is necessary in case the blinking behaviour is not within the GameObject itself (it would be in one of its children)
    [SerializeField] 
    protected BlinkingBehaviour BlinkingBehaviour;
    
    protected int InvulnerabilityTimer;
    protected Rigidbody2D Rigidbody;
    protected BoxCollider2D BoxCollider;
    protected PlayerInfo PlayerInfo;
    

    protected Animator Animator;
    protected PlayerLoopComponent PlayerLoopComponent;
    protected Vector2 LeftJoystickPosition;
    protected Vector2 RightJoystickPosition;
    protected float LTPosition;
    protected float RTPosition;
    protected Attack Attack;
    
    public AttackState AttackState = AttackState.Idle;

    private GameObject groundPlatfrom;

    public GameObject GroundPlatform
    {
        get => groundPlatfrom;
        set
        {
            if (value != null)
                value.gameObject.layer = Layers.STAGE;

            groundPlatfrom = value;
            lastPlatformPosition = Vector3.back;
        }
    }

    private Vector3 lastPlatformPosition;
    private bool isLoopComponentNotNull;

    protected virtual void Awake()
    {
        InvulnerabilityTimer = 60 * 3;
        Attack = new Attack();
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        BoxCollider = GetComponent<BoxCollider2D>();
        PlayerLoopComponent = GetComponent<PlayerLoopComponent>();
        isLoopComponentNotNull = PlayerLoopComponent != null;
        PlayerInfo = GetComponent<PlayerInfo>();
        if (BlinkingBehaviour == null)
            BlinkingBehaviour = GetComponent<BlinkingBehaviour>();
    }

    protected virtual void OnLeftJoystick(InputValue position)
    {
        LeftJoystickPosition = position.Get<Vector2>();
    }

    protected virtual void OnRightJoystick(InputValue position)
    {
        RightJoystickPosition = position.Get<Vector2>();
    }

    protected virtual void OnDPad(InputValue value)
    {
    }

    protected virtual void OnLT(InputValue value)
    {
        LTPosition = value.Get<float>();
    }

    protected virtual void OnRT(InputValue value)
    {
        RTPosition = value.Get<float>();
    }

    protected virtual void OnA(InputValue value)
    {
        if (value.isPressed)
            Jump();
    }

    protected virtual void OnB(InputValue value)
    {
        if (value.isPressed)
            Jump();
    }

    protected virtual void OnX(InputValue value)
    {
    }

    protected virtual void OnY(InputValue value)
    {
    }

    protected virtual void OnLB(InputValue value)
    {
    }

    protected virtual void OnRB(InputValue value)
    {
    }

    protected virtual void OnStart(InputValue value)
    {
        if (value.isPressed)
        {
            SceneManager.LoadScene(0);
            Destroy(GameObject.Find("GameManager"));
        }
    }

    protected virtual void OnSelect(InputValue value)
    {
    }

    protected virtual void OnLeftJoystickPress(InputValue value)
    {
    }

    protected virtual void OnRightJoystickPress(InputValue value)
    {
    }

    protected virtual void Jump()
    {
    }

    protected virtual void FixedUpdate()
    {
        //Apply user input, air resistance and gravity
        Vector2 velocity = Rigidbody.velocity;
        velocity += new Vector2(LeftJoystickPosition.x * mvSpeed, 0);
        velocity *= airResistance;
        velocity.y += gravity;
        
        if (!Attack.IsEmpty())
        {
            AttackState = AttackState.Attacking;
            Attack.Dequeue().Act(gameObject);
        }
        else
        {
            AttackState = AttackState.Idle;
        }

        //Set rigid body velocity to new velocity
        Rigidbody.velocity = velocity;

        //If on platform set position relative to it
        if (GroundPlatform != null)
        {
            Vector3 position = GroundPlatform.transform.position;
            if (lastPlatformPosition.z > -0.5f)
                transform.position += position - lastPlatformPosition;
            lastPlatformPosition = position;
        }
        else
            lastPlatformPosition = Vector3.back;

        BlinkingBehaviour.isBlinking = InvulnerabilityTimer-- > 0;
    }

    protected void DisableAttack()
    {
        AttackState = AttackState.Idle;
        Attack.Clear();
    }

    /// <summary>
    /// Hurts the player
    /// </summary>
    /// <param name="Direction">Direction Thrown</param>
    /// <param name="Multiplier"></param>
    /// <param name="Damage">Damage done to the character</param>
    /// <param name="SetKnockback">If the knockback is set with the multiplier or player percent will scale the knockback</param>
    public virtual void Hurt(Vector2 Direction, float Multiplier, float Damage, bool SetKnockback = false)
    {
        if (InvulnerabilityTimer < 0)
        {
            PlayerInfo.Damage += Damage;
            Rigidbody.velocity += Direction * (Multiplier * (1 + (SetKnockback ? 0 : PlayerInfo.Damage)));
            if (PlayerInfo.Damage < 0f)
                Die();
        }
    }

    public void SetAnimatorState(string state, bool status)
    {
        Animator.SetBool(state, status);
        //Set the dummy animation state
        if (PlayerLoopComponent != null)
            PlayerLoopComponent.SetDummyAnimatorState(state, status);
    }

    public void SetAnimatorState(int state, bool status)
    {
        Animator.SetBool(state, status);
        //Set the dummy animation state
        if (isLoopComponentNotNull)
            PlayerLoopComponent.SetDummyAnimatorState(state, status);
    }

    public void SetAnimatorState(int state, int status)
    {
        Animator.SetInteger(state, status);
        //Set the dummy animation state
        if (isLoopComponentNotNull)
            PlayerLoopComponent.SetDummyAnimatorState(state, status);
    }

    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
        if (isLoopComponentNotNull)
            PlayerLoopComponent.setDummyRotation(rotation);
    }

    public void TriggerAnimatorState(string state)
    {
        Animator.SetTrigger(state);
        if (PlayerLoopComponent != null)
            PlayerLoopComponent.TriggerDummyAnimatorState(state);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        float CollisionVelocity = Math.Abs(other.relativeVelocity[Math.Abs(other.contacts[0].normal.x) > 0.1f ? 0 : 1]);
        if (other.gameObject.layer == Layers.STAGE 
            && (Math.Abs(CollisionVelocity)) > 15)
        {
                Hurt(Vector2.zero, 0, (CollisionVelocity - 15) * 0.005f, false);
        }
    }

    protected virtual bool EvaluateAttacks(GameObject self)
    {
        if (!Attack.IsEmpty())
        {
            AttackState = AttackState.Attacking;
            Attack.Dequeue().Act(self);
        }
        else
            AttackState = AttackState.Idle;

        return !Attack.IsEmpty();
    }

    protected virtual void Die()
    {
        PlayerInfo.Stocks--;
        PlayerInfo.Damage = InitialDamage;
        InvulnerabilityTimer = 60 * 3;
        transform.Translate(new Vector3(Random.Range(-20, 20), Random.Range(-20, 20)));
        if (PlayerInfo.Stocks <= 0)
            Lose();
    }

    protected void Lose()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().PlayerLoses(PlayerInfo.Player);
    }
}