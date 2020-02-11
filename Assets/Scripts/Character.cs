using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character
{
    public abstract AttackGroup[] LightGrounded { get; set; }
    public abstract AttackGroup[] HeavyGrounded { get; set; }
    public abstract AttackGroup[] LightAerial { get; set; }
    public abstract AttackGroup[] HeavyAerial { get; set; }
}
