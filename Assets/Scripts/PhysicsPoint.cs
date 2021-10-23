using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PhysicsPoint : MonoBehaviour
{
    private Transform position;
    private Vector3 velocity;
    private Vector3 acceleration;
    public float mass;
    public bool isAnchor = false;
    void Start()
    {
        position = GetComponent<Transform>();
        velocity = new Vector3(0, 0, 0);
        acceleration = new Vector3(0, 0, 0);
        mass = 1;
    }

    public Transform Position
    {
        get
        {
            return position;
        }
        set
        {
        position = value;
        }
    }

    public Vector3 Velocity
    {
        get
        {
            return velocity;
        }
    } 

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAnchor) return;
        
        velocity += acceleration * Time.fixedDeltaTime;
        position.position += velocity * Time.fixedDeltaTime;
        acceleration *= 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 norm = Position.position - other.transform.position;
        float rad = norm.magnitude;
        norm.Normalize();

        Position.position = other.transform.position + ((norm * (rad + .1f)) * 1.01f);

        Vector3 velNormal = norm * Vector3.Dot(velocity, norm);
        velocity -= velNormal * 1.0f;
    }
    

    public void addForce(Vector3 force)
    {
        //gravity
        acceleration += Physics.gravity;
        
        Vector3 f = force;
        acceleration += f;
        
        if (Input.GetMouseButton(0))
        {
            acceleration += Vector3.left;
        }

    }
}
