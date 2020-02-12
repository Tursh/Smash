using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerControllerComponent : MonoBehaviour
{
	[SerializeField]
	KeyCode left = KeyCode.A, right = KeyCode.D, up = KeyCode.W, down = KeyCode.S, jump = KeyCode.Space, light = KeyCode.J, heavy = KeyCode.K;

	private AttackManager AttackManager;
	private PlayerPhysics PlayerPhysics;

	private void Start()
	{
		AttackManager = GetComponent<AttackManager>();
		PlayerPhysics = GetComponent<PlayerPhysics>();
	}

	private void Update()
    {
		if (Input.GetKey(left))
			PlayerPhysics.Move(Direction.Left);
		if (Input.GetKey(right))
			PlayerPhysics.Move(Direction.Right);
		if (Input.GetKey(up))
			PlayerPhysics.Move(Direction.Up);
		if (Input.GetKey(down))
			PlayerPhysics.Move(Direction.Down);

		if (Input.GetKeyDown(left))
			PlayerPhysics.StartMovement(Direction.Left);
		if (Input.GetKeyDown(right))
			PlayerPhysics.StartMovement(Direction.Right);
		if (Input.GetKeyDown(up))
			PlayerPhysics.StartMovement(Direction.Up);
		if (Input.GetKeyDown(down))
			PlayerPhysics.StartMovement(Direction.Down);

		if (Input.GetKeyDown(jump))
			PlayerPhysics.Jump();

		if (Input.GetKeyDown(light))
			AttackManager.Light();
		if (Input.GetKeyDown(heavy))
			AttackManager.Heavy();
    }
}
