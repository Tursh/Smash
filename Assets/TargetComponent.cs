using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TargetComponent : CharacterData
{
	private PlayerInfo playerInfo;
	
	public void Start()
	{
		playerInfo = GetComponent<PlayerInfo>();
	}
	protected override void FixedUpdate()
	{
		Rigidbody.velocity *= airResistance;
	}

	public override void Hurt(Vector2 Direction, float Multiplier, float Damage, bool SetKnockback = false)
	{
		PlayerInfo.Damage += Damage;
		Rigidbody.velocity += Direction * (Multiplier * (SetKnockback ? 1 + PlayerInfo.Damage : 0));
	}
}
