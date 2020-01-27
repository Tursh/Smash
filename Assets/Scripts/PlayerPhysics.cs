using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { LEFT, RIGHT, UP, DOWN }

public class PlayerPhysics : MonoBehaviour
{
	[SerializeField]
	private float mvSpeed = 0.5f, airResistance = 0.9f;
	public Vector2 velocity;
	private BoxCollider2D bc;
	private void Start()
	{
		GameObject.Find("GameManager").GetComponent<TickSystem>().TickEvent += Tick;
		bc = GetComponent<BoxCollider2D>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		
	}


	public void Tick(object sender, TickEventArgs args)
	{
		velocity *= airResistance;
		velocity.y -= 0.01f;
		transform.Translate(velocity);
	}

	public void Move(Direction direction)
	{
		switch (direction)
		{
			case Direction.LEFT:
				velocity += Vector2.left * mvSpeed;
				break;
			case Direction.RIGHT:
				velocity += Vector2.right * mvSpeed;
				break;
			case Direction.UP:

				break;
			case Direction.DOWN:
				velocity += Vector2.down * mvSpeed;
				break;
		}
	}
	public void StartMovement(Direction direction)
	{
		switch (direction)
		{
			case Direction.LEFT:
				velocity += Vector2.left * mvSpeed * 10;
				break;
			case Direction.RIGHT:
				velocity += Vector2.right * mvSpeed * 10;
				break;
			case Direction.UP:

				break;
			case Direction.DOWN:
				velocity += Vector2.down * mvSpeed * 10;
				break;
		}
	}

	public void Jump()
	{

	}
}
