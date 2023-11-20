using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackwardCommand : Command
{
    public override void Execute(Rigidbody rb)
    {
        rb.transform.position -= rb.transform.forward;

    }
    public override void Undo(Rigidbody rb)
    {
        rb.transform.position += rb.transform.forward;
    }
}