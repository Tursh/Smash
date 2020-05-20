using UnityEngine;
using UnityEngine.InputSystem;

public class ShipCharacter : CharacterData
{
    private float targetRotation;
    private bool RBIsBeingPressed;
    private int RBTimer;
    [SerializeField] private Material cantShootMaterial;
    [SerializeField] private Material normalMaterial;
    private MeshRenderer meshRenderer;

    private FrameOfAttack RB = new FrameOfAttack(o =>
    {
        AttackFunctions.SimpleProjectileAttack(new FrameDataProjectile(
            Utils.Vec32Vec2(-o.transform.up), 
            Utils.Vec32Vec2(-o.transform.up)*5,
            -o.transform.up*3,
            o.GetComponent<CharacterData>().Prefabs[0], 0.01f, 1f, framesOfLife: 30)).Act(o);
        return true;
    });
    
    protected override void Awake()
    {
        base.Awake();
        InitialDamage = 2f;
        PlayerInfo.Damage = InitialDamage;
    }
    
    protected void Start()
    {
        RBTimer = 0;
        targetRotation = 0f;
        RBIsBeingPressed = false;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    protected override void OnRB(InputValue value)
    {
        RBIsBeingPressed = value.isPressed;
    }
    
    protected override void FixedUpdate()
    {
        if (RBTimer > 3)
        {
            meshRenderer.material = normalMaterial;
            if (RBIsBeingPressed)
            {
                Attack.Enqueue(RB);
                RBTimer = 0;
            }
        }
        else
            meshRenderer.material = cantShootMaterial;

        if (LeftJoystickPosition.magnitude > 0.7f)
            targetRotation = Utils.Vec22Degree(LeftJoystickPosition) + 180f;
            
        transform.Rotate(0,0,Utils.RotateGradually(
            transform.eulerAngles.z,
            targetRotation, 0.1f));

        if (LTPosition > 0.2f)
            Rigidbody.velocity += Utils.Vec32Vec2(-transform.up * (LTPosition * mvSpeed));

        RBTimer++;
        EvaluateAttacks(gameObject);
        
        Rigidbody.velocity *= airResistance;
        
        BlinkingBehaviour.isBlinking = InvulnerabilityTimer-- > 0;
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
    

}
