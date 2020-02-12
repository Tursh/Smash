using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxComponent : MonoBehaviour
{
    [SerializeField] public Vector2 Direction;
    [SerializeField] public float Damage;
    [SerializeField] public float Multiplier;
    [SerializeField] public bool Enabled;
}
