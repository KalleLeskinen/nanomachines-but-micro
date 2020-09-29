using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

// voidaanko käyttää CharacterControlleria Rigidbodyn ja erillisen colliderin sijasta?
[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class PlayerMotor : MonoBehaviour
{
    public struct State
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public bool IsGrounded;
    }

    private State state;
    private Rigidbody rb;
    private BoxCollider collider;
    
    [SerializeField]
    private float movingSpeed = 10f;
    [SerializeField]
    private float maxVelocity = 32f;
    [SerializeField]
    private Vector3 drag = new Vector3(1f, 0f, 1f);
    [SerializeField]
    LayerMask layerMask;
    
    

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        state = new State { Position = transform.localPosition };
    }

    public void SetState(Vector3 position, Vector3 velocity, bool isGrounded)
    {
        // Määritellään uusi state
        state.Position = position;
        state.Velocity = velocity;
        state.IsGrounded = isGrounded;
        
        // Määritellään uusi lokaalipositio
        rb.AddTorque(state.Position - transform.localPosition);
    }

    // Tänne tsekit jos auto ei ole maassa (atm ei toimi) tai muuten vain incapacitated.
    public void Move(Vector3 velocity)
    {
        rb.AddTorque(velocity * BoltNetwork.FrameDeltaTime, ForceMode.Acceleration);
        state.Position = transform.localPosition;
    }

    public State Move(bool forward, bool backward, bool left, bool right, float yaw)
    {
        var moving = false;
        var movingDirection = Vector3.zero;

        if (forward ^ backward)
            movingDirection.z = forward ? +1 : -1;
        if (left ^ right)
            movingDirection.x = right ? +1 : -1;
        if (movingDirection.x != 0 || movingDirection.z != 0)
        {
            moving = true;
            movingDirection = Vector3.Normalize(Quaternion.Euler(0, yaw, 0) * movingDirection);
        }

        if (moving)
            Move(movingDirection * movingSpeed);

        // Clamp velocity
        state.Velocity = Vector3.ClampMagnitude(state.Velocity, maxVelocity);

        // Movement
        Move(state.Velocity);

        DetectTunneling();

        return state;
    }

    private void DetectTunneling()
    {
        if (Physics.Raycast(collider.center, Vector3.down, out var hit, collider.center.y, layerMask))
            transform.position = hit.point;
    }
}
