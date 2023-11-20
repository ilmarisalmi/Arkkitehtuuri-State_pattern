using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    protected float _Speed = 10.0f;

    float _Time = 0.0f;

    public abstract void Execute(Rigidbody rb);

    public abstract void Undo(Rigidbody rb);
}