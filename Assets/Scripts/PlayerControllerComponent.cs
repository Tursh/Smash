using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerControllerComponent : MonoBehaviour
{
	[SerializeField]
	KeyCode left = KeyCode.A, right = KeyCode.D, up = KeyCode.W, down = KeyCode.S, jump = KeyCode.Space, attack1 = KeyCode.J, attack2 = KeyCode.K;

	private PlayerPhysics pp;

	private void Start()
	{
		pp = GetComponent<PlayerPhysics>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		
	}

	private void Update()
    {
		if (Input.GetKey(left))
			pp.Move(Direction.LEFT);
		if (Input.GetKey(right))
			pp.Move(Direction.RIGHT);
		if (Input.GetKey(up))
			pp.Move(Direction.UP);
		if (Input.GetKey(down))
			pp.Move(Direction.DOWN);

		if (Input.GetKeyDown(left))
			pp.StartMovement(Direction.LEFT);
		if (Input.GetKeyDown(right))
			pp.StartMovement(Direction.RIGHT);
		if (Input.GetKeyDown(up))
			pp.StartMovement(Direction.UP);
		if (Input.GetKeyDown(down))
			pp.StartMovement(Direction.DOWN);

		if (Input.GetKeyDown(jump))
			pp.Jump();
	}
}
