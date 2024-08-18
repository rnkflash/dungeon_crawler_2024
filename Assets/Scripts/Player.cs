using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum State
    {
        Idle, ExecutingCommand
    }

    enum Command
    {
        Idle, MoveForward, MoveBack, MoveLeft, MoveRight, RotateLeft, RotateRight
    }

    [SerializeField] private Transform freeLook;

    private Command command = Command.Idle;
    private State state = State.Idle;
    
    private float walkDuration = 0.25f;
    private float walkDistance = 4.0f;
    private float castDistance = 4.0f;
    private float bounceRange = 0.5f;
    private float bounceDuration = 0.25f;
    private float rotateDuration = 0.25f;
    
    private Vector3 curPos;
    private Vector3 newPos;
    
    private int commandQueueSize = 1;
    private Queue<Command> commandQueue = new();

    private int xMinLimit = -80;
    private int xMaxLimit = 80;
    private int yMinLimit = -70;
    private int yMaxLimit = 70;
    private float xSpeed = 5.0f;
    private float ySpeed = 5.0f;
    private float zoomDampening = 12.0f;
    private bool isFreeLooking;
    private float xAngle;
    private float yAngle;
    private Quaternion curRot;
    private Quaternion desiredRot;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            AddToQueue(Command.MoveForward);    
        if (Input.GetKeyDown(KeyCode.S))
            AddToQueue(Command.MoveBack);
        if (Input.GetKeyDown(KeyCode.D))
            AddToQueue(Command.MoveRight);
        if (Input.GetKeyDown(KeyCode.A))
            AddToQueue(Command.MoveLeft);
        if (Input.GetKeyDown(KeyCode.Q))
            AddToQueue(Command.RotateLeft);
        if (Input.GetKeyDown(KeyCode.E))
            AddToQueue(Command.RotateRight);
        
        ExecuteNextCommand();
        
        if (Input.GetMouseButton(1))
        {
            if (!isFreeLooking)
            {
                Cursor.lockState = CursorLockMode.Locked;
                isFreeLooking = true;
            }
            FreeLook();
        }
        else
        {
            if (isFreeLooking)
            {
                Cursor.lockState = CursorLockMode.None;
                isFreeLooking = false;
            }
        }
    }
    
    void AddToQueue(Command command)
    {
        commandQueue.Enqueue(command);
        while (commandQueue.Count > commandQueueSize)
            commandQueue.Dequeue();
    }

    void ExecuteNextCommand()
    {
        if (state == State.ExecutingCommand || commandQueue.Count == 0) return;

        command = commandQueue.Dequeue();
        state = State.ExecutingCommand;

        if (!isFreeLooking && !isResettingFreeLook && freeLook.localRotation != Quaternion.identity)
            StartCoroutine(ResetFreeLook(walkDuration));
        
        switch (command)
        {
            case Command.Idle:
                break;
            case Command.MoveForward:
                MoveCommand(Vector3.forward);
                break;
            case Command.MoveBack:
                MoveCommand(Vector3.back);
                break;
            case Command.MoveLeft:
                MoveCommand(Vector3.left);
                break;
            case Command.MoveRight:
                MoveCommand(Vector3.right);
                break;
            case Command.RotateLeft:
                RotateCommand(-90);
                break;
            case Command.RotateRight:
                RotateCommand(90);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    bool CheckObstacles(Vector3 dir)
    {
        Debug.DrawRay(transform.position + Vector3.up * 1.0f, dir, Color.green, 1.0f);
        if (Physics.Raycast(transform.position + Vector3.up * 1.0f, dir, out var hit, castDistance))
        {
            Debug.Log(hit.collider.gameObject.name);
            return true;
        }
        return false;
    }
    
    void MoveCommand(Vector3 dir)
    {
        //TODO: get waypoint of direction from global map
        //var waypoint = Map.getWaypoint(myTile, direction);
        
        if (CheckObstacles(transform.TransformDirection(dir)))
        {
            StartCoroutine(BouncePlayer());
        }
        else
        {
            curPos = transform.position;
            newPos = curPos + transform.TransformDirection(dir) * walkDistance;
            StartCoroutine(Move(newPos));
        }
    }
    
    void RotateCommand(float newAngle)
    {
        StartCoroutine(Rotate(new Vector3(0, newAngle, 0), rotateDuration));
    }
    
    IEnumerator BouncePlayer()
    {
        float lerpStartTime;
        float lerpElapsedTime;
        float lerpComplete;

        curPos = transform.position;
        var direction = command switch
        {
            Command.MoveBack => Vector3.back,
            Command.MoveLeft => Vector3.left,
            Command.MoveRight => Vector3.right,
            _ => Vector3.forward
        };
        var oldPos = transform.position; 
        newPos = curPos + transform.TransformDirection(direction) * bounceRange;
        
        lerpStartTime = Time.time;
        lerpComplete = 0f;
        while (lerpComplete < 1.0f)
        {
            lerpElapsedTime = Time.time - lerpStartTime;
            lerpComplete = lerpElapsedTime / (bounceDuration / 2f);
            transform.position = Vector3.Lerp(curPos, newPos, lerpComplete * 2);

            yield return null;
        }
        
        lerpStartTime = Time.time;
        lerpComplete = 0f;
        while (lerpComplete < 1.0f)
        {
            lerpElapsedTime = Time.time - lerpStartTime;
            lerpComplete = lerpElapsedTime / (bounceDuration / 2f);
            transform.position = Vector3.Lerp(newPos, curPos, lerpComplete * 2);

            yield return null;
        }

        transform.position = oldPos;
        state = State.Idle;
    }

    IEnumerator Move(Vector3 endPosition)
    {
        float lerpStartTime;
        float lerpElapsedTime;
        float lerpComplete;

        lerpStartTime = Time.time;
        lerpComplete = 0f;

        while (lerpComplete < 1.0f)
        {
            lerpElapsedTime = Time.time - lerpStartTime;
            lerpComplete = lerpElapsedTime / walkDuration;

            transform.position = Vector3.Lerp(curPos, endPosition, lerpComplete);

            yield return null;
        }

        state = State.Idle;
    }
    
    IEnumerator Rotate(Vector3 eulerAngles, float duration)
    {
        float lerpStartTime;
        float lerpElapsedTime;
        float lerpComplete;

        lerpStartTime = Time.time;
        lerpComplete = 0f;

        Vector3 currentRot = transform.rotation.eulerAngles;
        Vector3 newRot = transform.rotation.eulerAngles + eulerAngles;

        while (lerpComplete < 1.0f)
        {
            lerpElapsedTime = Time.time - lerpStartTime;
            lerpComplete = lerpElapsedTime / duration;

            transform.eulerAngles = Vector3.Lerp(currentRot, newRot, lerpComplete);

            yield return null;
        }
        
        state = State.Idle;
    }
    
    private void FreeLook()
    {
        xAngle += Input.GetAxis("Mouse X") * (xSpeed);
        yAngle -= Input.GetAxis("Mouse Y") * (ySpeed);

        xAngle = ClampAngle(xAngle, xMinLimit, xMaxLimit);
        yAngle = ClampAngle(yAngle, yMinLimit, yMaxLimit);

        desiredRot = Quaternion.Euler(yAngle, xAngle, 0);
        curRot = freeLook.localRotation;

        freeLook.localRotation = Quaternion.Lerp(curRot, desiredRot, Time.deltaTime * zoomDampening);
    }

    private bool isResettingFreeLook;

    IEnumerator ResetFreeLook(float duration)
    {
        Debug.Log("resetting free look");
        
        float lerpStartTime;
        float lerpElapsedTime;
        float lerpComplete;

        lerpStartTime = Time.time;
        lerpComplete = 0f;

        Quaternion currentRot = freeLook.localRotation;

        while (lerpComplete < 1.0f)
        {
            lerpElapsedTime = Time.time - lerpStartTime;
            lerpComplete = lerpElapsedTime / duration;

            freeLook.localRotation = Quaternion.Lerp(currentRot, Quaternion.Euler(0, 0, 0), lerpElapsedTime / duration);

            yield return null;
        }

        xAngle = 0;
        yAngle = 0;

        isResettingFreeLook = false;
    }
    
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
    
}
