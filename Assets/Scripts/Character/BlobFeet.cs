using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobFeet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            transform.parent.GetComponent<BlobCharacter>().OnGround();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            transform.parent.GetComponent<BlobCharacter>().InAir();
        }
    }
}
