using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditorInternal;
using UnityEngine;

public class Spring
{
    public PhysicsPoint _a;
    public PhysicsPoint _b;
    public float restLength = 20.0f;
    public float springK = 0.1f;
    public float dampK = 1;
    public float breakForce = 10000;
    public bool broken = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Spring(PhysicsPoint a, PhysicsPoint b, float rest, float sk, float dk)
    {
        _a = a;
        _b = b;
        restLength = rest;
        springK = sk;
        dampK = dk;
    }

    // Update is called once per frame
    public void Move()
    {
        Vector3 spring = _b.Position.position - _a.Position.position;
        float currentLength = spring.magnitude;
        spring.Normalize();
        float x = currentLength - restLength;
        spring *= -springK * x;
        Vector3 dampForce = -dampK * (_b.Velocity - _a.Velocity);
        Vector3 dampForceProj = Vector3.Project(dampForce, spring);
        if (spring.magnitude > breakForce)
        {
            broken = true;
        }
        _a.addForce(-spring - dampForceProj);
        _b.addForce(spring + dampForceProj);
    }

}
