using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum CharacterRenderType
{
    Sprite,
    Model
}

public abstract class CharacterData : MonoBehaviour
{
    public float JumpWindup = 20;
    public float JumpMultiplier = 10;

    public float mvSpeed = 0.025f;
    public float airResistance = 0.9f;
    public float gravity = -0.05f;
    public float distanceToGround = 1;
    public int PlayerNumber;
    protected float InitialDamage = 0f;
    protected int InvulnerabilityTimer;

    public GameObject[] Prefabs;

    protected PlayerControls PlayerControls;
    protected Rigidbody2D Rigidbody;
    protected BoxCollider2D BoxCollider;
    protected CharacterRenderType CharacterRenderType;
    protected PlayerInfo PlayerInfo;
    protected GameObject UiPlayerInfo;
    [SerializeField]
    protected BlinkingBehaviour BlinkingBehaviour;

    protected Animator Animator;
    private PlayerLoopComponent PlayerLoopComponent;

    protected Vector2 LeftJoystickPosition;
    protected Vector2 RightJoystickPosition;
    protected float LTPosition;
    protected float RTPosition;

    protected Attack Attack;
    public AttackState AttackState = AttackState.Idle;

    [SerializeField] private GameObject groundPlatfrom;

    public GameObject GroundPlatform
    {
        get => groundPlatfrom;
        set
        {
            if (value != null)
            {
                value.gameObject.layer = Layers.STAGE;
                //Debug.Log("Now on " + value.name);
            }
            else
            {
                //Debug.Log("Leaving " + GroundPlatform.name);
                groundPlatfrom.gameObject.layer = Layers.SEMI_STAGE;
            }

            groundPlatfrom = value;
            lastPlatfromPosition = Vector3.back;
        }
    }

    private Vector3 lastPlatfromPosition;
    private bool _isLoopComponentNotNull;

    protected virtual void Awake()
    {
        InvulnerabilityTimer = 60 * 3;
        Attack = new Attack();
        PlayerControls = new PlayerControls();
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        BoxCollider = GetComponent<BoxCollider2D>();
        PlayerLoopComponent = GetComponent<PlayerLoopComponent>();
        PlayerInfo = GetComponent<PlayerInfo>();
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

    //debug purposes
    protected virtual void OnKeySpace(InputValue value)
    {
        if (value.isPressed)
            Jump();
    }

    protected virtual void OnKeyA(InputValue value)
    {
        if (value.isPressed)
            LeftJoystickPosition += Vector2.left;
        else
            LeftJoystickPosition += Vector2.right;
    }

    protected virtual void OnKeyD(InputValue value)
    {
        if (value.isPressed)
            LeftJoystickPosition += Vector2.right;
        else
            LeftJoystickPosition += Vector2.left;
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
            if (lastPlatfromPosition.z > -0.5f)
                transform.position += position - lastPlatfromPosition;
            lastPlatfromPosition = position;
        }
        else
            lastPlatfromPosition = Vector3.back;

        
        BlinkingBehaviour.isBlinking = InvulnerabilityTimer-- > 0;
        

    }

    protected void DisableAttack()
    {
        AttackState = AttackState.Idle;
        Attack.Clear();
    }

    private void OnEnable()
    {
        PlayerControls.Enable();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Direction">Direction Thrown</param>
    /// <param name="Multiplier"></param>
    /// <param name="Damage"></param>
    /// <param name="SetKnockback">If the knockback is set</param>
    public virtual void Hurt(Vector2 Direction, float Multiplier, float Damage, bool SetKnockback = false)
    {
        if (InvulnerabilityTimer < 0)
        {
            PlayerInfo.Damage = Damage;
            Rigidbody.velocity += Direction * (Multiplier * (1 + (SetKnockback ? 0 : PlayerInfo.Damage)));
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
        if (_isLoopComponentNotNull != null)
            PlayerLoopComponent.SetDummyAnimatorState(state, status);
    }

    public void SetAnimatorState(int state, int status)
    {
        Animator.SetInteger(state, status);
        //Set the dummy animation state
        if (_isLoopComponentNotNull)
            PlayerLoopComponent.SetDummyAnimatorState(state, status);
    }

    public void setRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
        if (_isLoopComponentNotNull)
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
        if (other.gameObject != groundPlatfrom)
        {
            other.gameObject.layer =
                other.transform.position.y <= BoxCollider.bounds.min.y ? Layers.STAGE : Layers.SEMI_STAGE;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject != groundPlatfrom)
        {
            other.gameObject.layer =
                other.contacts[0].normal.y < 0 && other.transform.position.y <= transform.position.y
                    ? Layers.STAGE
                    : Layers.SEMI_STAGE;
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
        {
            AttackState = AttackState.Idle;
        }

        return !Attack.IsEmpty();
    }

    protected virtual void Die()
    {
        PlayerInfo.Stocks--;
        PlayerInfo.Damage = InitialDamage;
        InvulnerabilityTimer = 60 * 3;
        transform.Translate(new Vector3(Random.Range(-5,5), Random.Range(-5,5)));
    }

    protected void Lose()
    {
        
    }
}