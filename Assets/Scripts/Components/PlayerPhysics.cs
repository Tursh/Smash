using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;

public enum Direction
{
    Left,
    Right,
    Up,
    Down
}

public enum PlayerState
{
    OnGround,
    InAir,
    OnEdge
}


public class OnGroundEnventArgs
{
}

public class PlayerPhysics : MonoBehaviour
{
    [SerializeField] public PlayerState PlayerState = PlayerState.InAir;

    private BoxCollider2D BoxCollider;
    private CharacterData CharacterData;

    public float MagicNumber;
    
    public Vector2 Facing = Vector2.left;
    public Vector2 velocity;
    
    private int playerLayer = 9, stageLayer = 8, semiPlatformLayer = 12;

    public EventHandler<OnGroundEnventArgs> OnGroundEventHandler;

    private void Start()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
        CharacterData = GetComponent<CharacterData>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //TODO: Do damage depending on the velocity
        Vector3 normal = other.contacts[0].normal;

        if (other.otherCollider.gameObject.layer != playerLayer)
            return;

        if (other.collider.gameObject.layer == stageLayer)
        {
            //Check if normal is pointed downwards
            if (normal == new Vector3(0, 1, 0))
            {
                PlayerState = PlayerState.OnGround;
                transform.SetParent(other.transform);
                OnGroundEventHandler?.Invoke(this, new OnGroundEnventArgs());
            }

            if (normal.y != 0)
            {
                Vector2 position = transform.position;
                position.y =
                    (PlayerState == PlayerState.OnGround ? other.collider.bounds.max : other.collider.bounds.min).y
                    + (BoxCollider.bounds.extents.y - BoxCollider.offset.y/MagicNumber) * normal.y;
                transform.position = position;

                velocity.y = 0;
            }
            else
            {
                velocity = Vector2.zero;

                PlayerState = PlayerState.OnEdge;
                transform.SetParent(other.transform);

                Bounds stageBounds = other.collider.bounds;

                Vector2 edgePosition;
                edgePosition.x = (normal.x > 0 ? stageBounds.max.x : stageBounds.min.x) +
                                 BoxCollider.bounds.extents.x * normal.x;
                edgePosition.y = stageBounds.max.y - BoxCollider.bounds.extents.y;

                transform.position = edgePosition;
            }
        }
        else if (other.gameObject.layer == semiPlatformLayer && normal == new Vector3(0, 1, 0))
        {
            if (PlayerState != PlayerState.OnGround)
            {
                //Set the player on ground
                PlayerState = PlayerState.OnGround;
                transform.SetParent(other.transform);
                OnGroundEventHandler?.Invoke(this, new OnGroundEnventArgs());

                //Set player to the top of the platform
                Vector2 position = transform.position;
                position.y = other.collider.bounds.max.y + BoxCollider.bounds.extents.y;
                transform.position = position;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.otherCollider.gameObject.layer != playerLayer)
            return;
        if (other.collider.gameObject.layer == stageLayer)
        {
            PlayerState = PlayerState.InAir;
            transform.SetParent(null);
        }
    }

    public void FixedUpdate()
    {
        velocity *= CharacterData.airResistance;

        if (PlayerState == PlayerState.InAir)
            velocity.y += CharacterData.gravity;

        transform.Translate(velocity);
    }


}