using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public enum CharacterRenderType
{
    Sprite, Model
}

public abstract class CharacterData : MonoBehaviour
{
    public float mvSpeed = 0.025f;
    public float airResistance = 0.9f;
    public float gravity = -0.05f;
    public float distanceToGround = 1;

    public GameObject[] Prefabs;

    protected PlayerControls PlayerControls;
    protected Rigidbody2D Rigidbody;
    protected BoxCollider2D BoxCollider;
    protected CharacterRenderType CharacterRenderType;
    protected PlayerInfo PlayerInfo;

    private Animator Animator;
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

    protected virtual void Awake()
    {
        Attack = new Attack();
        PlayerControls = new PlayerControls();
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        BoxCollider = GetComponent<BoxCollider2D>();
        PlayerLoopComponent = GetComponent<PlayerLoopComponent>();
        PlayerInfo = GetComponent<PlayerInfo>();

        //PlayerPhysics.OnGroundEventHandler += OnGround;

        //vector2
        PlayerControls.Gameplay.LeftJoystick.performed += LeftJoystickOnperformed;
        PlayerControls.Gameplay.RightJoystick.performed += RightJoystickOnperformed;
        PlayerControls.Gameplay.DPad.performed += DPadOnperformed;

        //value
        PlayerControls.Gameplay.LT.performed += LTOnperformed;
        PlayerControls.Gameplay.RT.performed += RTOnperformed;

        //boolean
        PlayerControls.Gameplay.A.performed += AOnperformed;
        PlayerControls.Gameplay.B.performed += BOnperformed;
        PlayerControls.Gameplay.X.performed += XOnperformed;
        PlayerControls.Gameplay.Y.performed += YOnperformed;
        PlayerControls.Gameplay.LB.performed += LBOnperformed;
        PlayerControls.Gameplay.RB.performed += RBOnperformed;
        PlayerControls.Gameplay.Start.performed += StartOnperformed;
        PlayerControls.Gameplay.Select.performed += SelectOnperformed;
        PlayerControls.Gameplay.LeftJoystickPress.performed += LeftJoystickPressOnperformed;
        PlayerControls.Gameplay.RightJoystickPress.performed += RightJoystickPressOnperformed;

        //debug stuff with keyboard
        PlayerControls.Gameplay.KeyA.performed += KeyAOnperformed;
        PlayerControls.Gameplay.KeyD.performed += KeyDOnperformed;
        PlayerControls.Gameplay.KeySpace.performed += KeySpaceOnperformed;
    }

    protected virtual void LeftJoystickOnperformed(InputAction.CallbackContext ctx)
    {
        LeftJoystickPosition = ctx.ReadValue<Vector2>();
    }

    protected virtual void RightJoystickOnperformed(InputAction.CallbackContext ctx)
    {
        RightJoystickPosition = ctx.ReadValue<Vector2>();
    }

    protected virtual void DPadOnperformed(InputAction.CallbackContext ctx)
    {
    }

    protected virtual void LTOnperformed(InputAction.CallbackContext ctx)
    {
        LTPosition = ctx.ReadValue<float>();
    }

    protected virtual void RTOnperformed(InputAction.CallbackContext ctx)
    {
        RTPosition = ctx.ReadValue<float>();
    }

    protected virtual void AOnperformed(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton())
            Jump();
    }

    protected virtual void BOnperformed(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton())
            Jump();
    }

    protected virtual void XOnperformed(InputAction.CallbackContext ctx)
    {
    }

    protected virtual void YOnperformed(InputAction.CallbackContext ctx)
    {
    }

    protected virtual void LBOnperformed(InputAction.CallbackContext ctx)
    {
    }

    protected virtual void RBOnperformed(InputAction.CallbackContext ctx)
    {
    }

    protected virtual void StartOnperformed(InputAction.CallbackContext ctx)
    {
    }

    protected virtual void SelectOnperformed(InputAction.CallbackContext ctx)
    {
    }

    protected virtual void LeftJoystickPressOnperformed(InputAction.CallbackContext obj)
    {
    }

    protected virtual void RightJoystickPressOnperformed(InputAction.CallbackContext obj)
    {
    }

    //debug purposes
    protected virtual void KeySpaceOnperformed(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton()) Jump();
    }

    protected virtual void KeyAOnperformed(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton())
            LeftJoystickPosition += Vector2.left;
        else
            LeftJoystickPosition += Vector2.right;
    }

    protected virtual void KeyDOnperformed(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton())
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
            Attack.Pop().Act(gameObject);
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
        PlayerInfo.Damage = Damage;
        Rigidbody.velocity += Direction * (Multiplier * (1 + (SetKnockback ? 0 : PlayerInfo.Damage)));
    }

    public void SetAnimatorState(string state, bool status)
    {
        Animator.SetBool(state, status);
        //Set the dummy animation state
        if (LoopComponent != null)
            LoopComponent.SetDummyAnimatorState(state, status);
    }
    
    public void SetAnimatorState(int state, bool status)
    {
        Animator.SetBool(state, status);
        //Set the dummy animation state
        if (LoopComponent != null)
            LoopComponent.SetDummyAnimatorState(state, status);
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
            Attack.Pop().Act(self);
        }
        else
        {
            AttackState = AttackState.Idle;
        }

        return !Attack.IsEmpty();
    }
}