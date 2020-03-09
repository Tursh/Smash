﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	private void Hurt(GameObject player)
	{
		PlayerPhysics playerPhysics = player.gameObject.GetComponent<PlayerPhysics>();
		PlayerInfo playerInfo = player.gameObject.GetComponent<PlayerInfo>();
		playerPhysics.velocity += Direction * (Multiplier * (playerInfo.Damage + 1));
		playerInfo.Damage += Damage;
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		OnTriggerEnter2D(other);
	}
}