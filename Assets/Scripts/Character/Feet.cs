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
    private List<GameObject> EnteredPlatfroms = new List<GameObject>();

    private CharacterData Parent;

    private void Awake()
    {
        Parent = transform.parent.GetComponent<CharacterData>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int otherLayer = other.gameObject.layer;
        if (otherLayer == Layers.STAGE || otherLayer == Layers.SEMI_STAGE)
        {
            CharacterData parent = transform.parent.GetComponent<CharacterData>();
            EnteredPlatfroms.Add(other.gameObject);

            //If the other platform come from higher
            if (EnteredPlatfroms.Count > 1 && other.transform.position.y > parent.GroundPlatform.transform.position.y)
                return;

            parent.GroundPlatform = other.gameObject;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == Layers.STAGE ||
            other.gameObject.layer == Layers.SEMI_STAGE)
        {
            EnteredPlatfroms.Remove(other.gameObject);

            if (EnteredPlatfroms.Count == 0)
            {
                Parent.GroundPlatform = null;
            }
            else if (EnteredPlatfroms.Count == 1)
                Parent.GroundPlatform = EnteredPlatfroms[0];
        }
    }
}