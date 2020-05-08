using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

public class VaisseauCharacter : CharacterData
{
    private float targetRotation;
    private bool RBIsBeingPressed;
    private int RBTimer;

    protected void Start()
    {
        RBTimer = 0;
        targetRotation = 0f;
        RBIsBeingPressed = false;
    }

    protected override void RBOnperformed(InputAction.CallbackContext ctx)
    {
        RBIsBeingPressed = ctx.ReadValueAsButton();
    }

    private FrameOfAttack RB = new FrameOfAttack(o =>
    {
        AttackFunctions.SimpleProjectileAttack(new FrameDataProjectile(
            Utils.Vec32Vec2(-o.transform.up), 
            Utils.Vec32Vec2(-o.transform.up)*5,
            Vector2.zero,
            o.GetComponent<CharacterData>().Prefabs[0],framesOfLife: 100)).Act(o);
        return true;
    });

    protected override void FixedUpdate()
    {
        if (RBIsBeingPressed && RBTimer > 10)
        {
            Attack.Push(RB);
            RBTimer = 0;
        }

        if (LeftJoystickPosition.magnitude > 0.7f)
            targetRotation = Utils.Vec22Degree(LeftJoystickPosition) + 180f;
            
        transform.Rotate(0,0,RotateGradually(targetRotation, 0.1f));

        if (LTPosition > 0.2f)
            Rigidbody.velocity += Utils.Vec32Vec2(-transform.up * (LTPosition * mvSpeed));

        RBTimer++;
        EvaluateAttacks(gameObject);
        
        Rigidbody.velocity *= airResistance;
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
}
