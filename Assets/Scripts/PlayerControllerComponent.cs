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

	private AttackManager am;
	private PlayerPhysics pp;

	private void Start()
	{
		am = GetComponent<AttackManager>();
		pp = GetComponent<PlayerPhysics>();
	}

	private void Update()
    {
		if (Input.GetKey(left))
			pp.Move(Direction.Left);
		if (Input.GetKey(right))
			pp.Move(Direction.Right);
		if (Input.GetKey(up))
			pp.Move(Direction.Up);
		if (Input.GetKey(down))
			pp.Move(Direction.Down);

		if (Input.GetKeyDown(left))
			pp.StartMovement(Direction.Left);
		if (Input.GetKeyDown(right))
			pp.StartMovement(Direction.Right);
		if (Input.GetKeyDown(up))
			pp.StartMovement(Direction.Up);
		if (Input.GetKeyDown(down))
			pp.StartMovement(Direction.Down);

		if (Input.GetKeyDown(jump))
			pp.Jump();

		if (Input.GetKeyDown(light))
			am.Light();
		if (Input.GetKeyDown(heavy))
			am.Heavy();
    }
}
