using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightCommand : Command
{
    public override void Execute(Rigidbody rb)
    {
        rb.transform.position += rb.transform.right;
    }

    public override void Undo(Rigidbody rb)
    {
        rb.transform.position -= rb.transform.right;
    }
}