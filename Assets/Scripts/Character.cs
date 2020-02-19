using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterData : MonoBehaviour
{
    public abstract AttackSet AttackSet { get; set; }
}