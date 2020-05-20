using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HurtboxComponent : MonoBehaviour
{
	public Vector2 Direction;
	public float Damage;
	public float Multiplier;
	public int FramesOfLife;
	public bool setKnockback;

	private void FixedUpdate()
	{
		if (FramesOfLife-- <= 0)
			Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == Layers.PLAYER)
		{
			Hurt(other.gameObject);
		}
		else if (other.gameObject.layer == Layers.DUMMY)
		{
			Hurt(other.gameObject.GetComponent<DummyComponent>().PlayerReference);
		}
	}

	protected virtual void Hurt(GameObject player)
	{
		CharacterData characterData = player.GetComponent<CharacterData>();
		if (characterData == null)
			characterData = player.transform.parent.GetComponent<CharacterData>();

		characterData.Hurt(Direction,Multiplier,Damage);
		Destroy(gameObject);
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		OnTriggerEnter2D(other);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		
	}
}