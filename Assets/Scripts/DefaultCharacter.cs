using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCharacter : CharacterData
{
    public override AttackSet AttackSet { get; set; } = new AttackSet(new AttackGroup(
            new Attack[] { },
            new Attack[] { },
            new Attack[]
            {
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(2, 2), new Vector2(2, 0)),
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(4, 4), new Vector2(2, 0)),
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(2, 2), new Vector2(2, 0)),
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(4, 4), new Vector2(2, 0)),
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(2, 2), new Vector2(2, 0)),
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(4, 4), new Vector2(2, 0)),
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(2, 2), new Vector2(2, 0)),
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(4, 4), new Vector2(2, 0)),
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(2, 2), new Vector2(2, 0)),
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(4, 4), new Vector2(2, 0)),
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(2, 2), new Vector2(2, 0)),
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(4, 4), new Vector2(2, 0)),
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(2, 2), new Vector2(2, 0)),
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(4, 4), new Vector2(2, 0))
            },
            new Attack[] { }),
        new AttackGroup(),
        new AttackGroup(
            new Attack[] { },
            new Attack[] { },
            new Attack[]
            {
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(0, 0), new Vector2(0, 0)),
                new Attack(Vector2.up, 0.1f, 1f,
                    new Vector2(4, 4), new Vector2(0, 0)),
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(0, 0), new Vector2(0, 0)),
                new Attack(Vector2.up, 0.1f, 1f,
                    new Vector2(4, 4), new Vector2(0, 0)),
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(0, 0), new Vector2(0, 0)),
                new Attack(Vector2.up, 0.1f, 1f,
                    new Vector2(4, 4), new Vector2(0, 0)),
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(0, 0), new Vector2(0, 0)),
                new Attack(Vector2.up, 0.1f, 1f,
                    new Vector2(4, 4), new Vector2(0, 0)),
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(0, 0), new Vector2(0, 0)),
                new Attack(Vector2.up, 0.1f, 1f,
                    new Vector2(4, 4), new Vector2(0, 0)),
                new Attack(Vector2.right, 0.1f, 1f,
                    new Vector2(0, 0), new Vector2(0, 0)),
                new Attack(Vector2.up, 0.1f, 1f,
                    new Vector2(4, 4), new Vector2(0, 0)),
            },
            new Attack[] { }),
        new AttackGroup());
}