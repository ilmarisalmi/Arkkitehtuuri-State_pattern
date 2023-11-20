using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //state enums for standing, jumping and ducking.
    enum State
    {
        Standing,
        Jumping,
        Ducking,
        Powerup
    }
    State my_state = State.Standing;

    

    Rigidbody _rigidbody;
    Vector3 _start_pos;
    float TimeAccu = 0.0f;
    public float powerUpDuration = 10.0f;
    Vector3 originalScale;

    bool bReplaying = false;

    // Commands:
    Command cmd_W = new MoveForwardCommand();
    Command cmd_A = new MoveLeftCommand();
    Command cmd_S = new MoveBackwardCommand();
    Command cmd_D = new MoveRightCommand();

    Command cmdNothing = new DoNothingCommand();
    Command cmdForward = new MoveForwardCommand();
    Command cmdBackward = new MoveBackwardCommand();
    Command cmdLeft = new MoveLeftCommand();
    Command cmdRight = new MoveRightCommand();

    //ref Command rcmd = ref cmdNothing;

    Command _last_command = null;

    // Stacks to store the commands
    Stack<Command> _undo_commands = new Stack<Command>();
    Stack<Command> _redo_commands = new Stack<Command>();
    Stack<Command> _replay_commands = new Stack<Command>();

    // Set a keybinding
    void SetCommand(ref Command cmd, ref Command new_cmd)
    {
        cmd = new_cmd;
    }

    void SwapCommands(ref Command A, ref Command B)
    {
        Command tmp = A;
        A = B;
        B = tmp;

        //    _undo_commands.Push();
        //    Command cmd = _undo_commands.Pop();
    }

    void ClearCommands()
    {
        SetCommand(ref cmd_W, ref cmdNothing);
        SetCommand(ref cmd_A, ref cmdNothing);
        SetCommand(ref cmd_S, ref cmdNothing);
        SetCommand(ref cmd_D, ref cmdNothing);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject); // Destroy the coin!
        if (other.CompareTag("PowerUp") && my_state != State.Powerup)
        {
            my_state = State.Powerup;
            transform.localScale = new Vector3(2, 2, 2);
            Invoke(nameof(ResetPowerUp), powerUpDuration);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _start_pos = transform.position;

    }

    IEnumerator Replay()
    {
        // Go through all the replay commands
        while (_replay_commands.Count > 0)
        {
            Command cmd = _replay_commands.Pop();
            _undo_commands.Push(cmd);
            cmd.Execute(_rigidbody);
            yield return new WaitForSeconds(.5f);
        }

        bReplaying = false;
    }

    // Update is called once per frame
    void Update()
    {



        if (bReplaying)
        {
            TimeAccu += Time.deltaTime;
            

        }
        else
        {
            //State machine for player states
            

            if (Input.GetKeyDown(KeyCode.R))
            {
                bReplaying = true;
                TimeAccu = 0.0f;
               
                while (_undo_commands.Count > 0)
                {
                    _replay_commands.Push(_undo_commands.Pop());
                }
                
                transform.position = _start_pos;

               
                StartCoroutine(Replay());

            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                cmd_W.Execute(_rigidbody);
                _undo_commands.Push(cmd_W);
                _redo_commands.Clear();
               
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                cmd_A.Execute(_rigidbody);
                _undo_commands.Push(cmd_A);
                _redo_commands.Clear();
               
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                cmd_S.Execute(_rigidbody);
                _undo_commands.Push(cmd_S);
                _redo_commands.Clear();
               
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                cmd_D.Execute(_rigidbody);
                _undo_commands.Push(cmd_D);
                _redo_commands.Clear();
                
            }
            //Standing State switch case.
            switch (my_state)
            {
                //Poweruop state. If collides with powerup set state to powerup. In powerup state scale of player is 2x.
                case State.Powerup:
                    Debug.Log("Powerup");
                    break;

                case State.Standing:
                    transform.localScale = new Vector3(1, 1, 1);
                    Debug.Log("Standing");

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        _rigidbody.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
                        my_state = State.Jumping;
                        Debug.Log("I want to jump");

                    }
                    if (Input.GetKeyDown(KeyCode.LeftControl))
                    {
                        transform.localScale = new Vector3(1, 0.5f, 1);
                        my_state = State.Ducking;
                        Debug.Log("I want to duck");

                    }
                    
                    break;
                //Jumping State switch case. If player collider collides with ground tag collider set state to standing.
                case State.Jumping:
                    Debug.Log ("Jumping");
                    

                    break;
                //Ducking State switch case. If player lets go of left control buttn set state to standing.
                case State.Ducking:
                    Debug.Log("Ducking");
                    if (Input.GetKeyUp(KeyCode.LeftControl))
                    {
                        
                        my_state = State.Standing;
                    }
                    
                    break;
            }
            

 

            if (Input.GetKeyDown(KeyCode.Z))
            {
                
                if (_undo_commands.Count > 0)
                {
                   
                    Command cmd = _undo_commands.Pop();
                    _redo_commands.Push(cmd);
                    cmd.Undo(_rigidbody);
                }
            }
            if (Input.GetKeyDown(KeyCode.X))
            {

                if (_redo_commands.Count > 0)
                {
                    Command cmd = _redo_commands.Pop();
                    _undo_commands.Push(cmd);
                    cmd.Execute(_rigidbody);
                }

            }

            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
               
            }

        }

    }
    //OnCollisionEnter method. Check if the collider has the ground tag and the state is jumping, then set state to standing.
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground") 
        {
            my_state = State.Standing;
            Debug.Log("Landed");
        } 
    }
    void ResetPowerUp()
    {
        my_state = State.Standing;
        transform.localScale = originalScale;
        Destroy(GameObject.FindWithTag("PowerUp"));
    }
}