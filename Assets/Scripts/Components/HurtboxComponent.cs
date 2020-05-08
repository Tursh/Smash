using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HurtboxComponent : MonoBehaviour
{
	public Vector2 Direction;
	public float Damage;
	public float Multiplier;
	public int FramesOfLife;

	private void FixedUpdate()
	{
		if (FramesOfLife-- <= 0)
			Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == 9)
		{
			Hurt(other.gameObject);
		}
		else if (other.gameObject.layer == 11)
		{
			Hurt(other.gameObject.GetComponent<DummyComponent>().PlayerReference);
		}
	}

	protected virtual void Hurt(GameObject player)
	{
		player.GetComponent<CharacterData>().Hurt(Direction,Multiplier,Damage);
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		OnTriggerEnter2D(other);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		
	}
}