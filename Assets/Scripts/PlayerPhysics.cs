﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;

public enum Direction { LEFT, RIGHT, UP, DOWN }

public class PlayerPhysics : MonoBehaviour
{
	[SerializeField]
	private float mvSpeed = 0.5f, airResistance = 0.9f, gravity = -0.1f;
	public Vector2 velocity;
	
	BoxCollider2D bc;
    Rigidbody2D rb;

    private bool onGround = false;
    
    private int playerLayer = 9, stateLayer = 8;
    
	private void Start()
	{
		bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
	    
	    //TODO: Do damage depending on the velocity
	    
	    if (other.contacts[0].normal.y != 0)
		    velocity.y = -gravity;
	    else
		    velocity.x = 0;

	    if (other.collider.gameObject.layer == stateLayer)
	    {
		    Vector2 position = transform.position;
		    position.y = (other.collider.bounds.max + bc.bounds.extents).y;
		    transform.position = position;
	    }
    }


    private void OnCollisionStay2D(Collision2D other)
	{
        //transform.Translate(bc.Distance(collision.collider).normal * bc.Distance(collision.collider).distance);
        //Debug.Log(bc.Distance(collision.collider).normal * bc.Distance(collision.collider).distance);
        
        if (other.collider.gameObject.layer == stateLayer && other.contacts[0].normal == new Vector2(0, 1))
        {
	        if(!onGround)
		        velocity.y = 0;
	        onGround = true;
        }
	}

    private void OnCollisionExit2D(Collision2D other)
    {
	    if (other.collider.gameObject.layer == stateLayer)
		    onGround = false;
    }

    public void Update()
	{
		velocity *= airResistance;
		if(!onGround)
		velocity.y += gravity;
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
		
		velocity.y = 1;
	}
}
