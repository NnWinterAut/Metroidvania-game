using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    public abstract List<GameObject> loots { get; protected set; }
    public abstract float visibleDistanceSphere { get; protected set; }
}
