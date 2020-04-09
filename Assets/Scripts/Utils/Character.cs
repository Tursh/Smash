using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

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
    protected Animator Animator;

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

        Rigidbody.velocity = velocity;

        if (GroundPlatform != null)
        {
            Vector3 position = GroundPlatform.transform.position;
            if (lastPlatfromPosition.z > -0.5f)
                transform.position += position - lastPlatfromPosition;
            lastPlatfromPosition = position;
        }
        else
            lastPlatfromPosition = Vector3.back;
        
        Debug.Log(Rigidbody.velocity);
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

    public virtual void OnGround()
    {
    }

    public virtual void InAir()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.layer =
            GroundPlatform == null || other.transform.position.y <= BoxCollider.bounds.min.y
                ? Layers.STAGE : Layers.SEMI_STAGE;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.layer =
            GroundPlatform == null || other.transform.position.y <= BoxCollider.bounds.min.y ? Layers.STAGE : Layers.SEMI_STAGE;
    }
}