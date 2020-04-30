using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class SemiPlatform : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == Layers.PLAYER)
            gameObject.layer = other.transform.position.y > transform.position.y ? Layers.STAGE : Layers.SEMI_STAGE;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == Layers.PLAYER)
        {
            gameObject.layer =
                other.contacts[0].normal.y < 0 && other.transform.position.y > transform.position.y
                    ? Layers.STAGE : Layers.SEMI_STAGE;
        }
    }
}
