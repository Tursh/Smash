using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using Utils;

public class Feet : MonoBehaviour
{
    /// <summary>
    /// If the foot just change from a platform to another, don't execute de landing animation!
    /// </summary>
    private bool JustChangedPlatform = false;

    //private bool isSemiStage = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        int otherLayer = other.gameObject.layer;
        if (otherLayer == Layers.STAGE || otherLayer == Layers.SEMI_STAGE)
        {
            CharacterData parent = transform.parent.GetComponent<BlobCharacter>();
            JustChangedPlatform = parent.GroundPlatform != null;

            //Execute de landing animation
            if (!JustChangedPlatform)
                parent.OnGround();
            else
            {
                //If the platform come from higher
                if (other.transform.position.y > parent.GroundPlatform.transform.position.y)
                    return;
            }

            parent.GroundPlatform = other.gameObject;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (!JustChangedPlatform && other.gameObject.layer == Layers.STAGE ||
            other.gameObject.layer == Layers.SEMI_STAGE)
        {
            CharacterData parent = transform.parent.GetComponent<BlobCharacter>();

            if (parent.GroundPlatform == other.gameObject)
                parent.InAir();

            parent.GroundPlatform = null;
        }
    }
}