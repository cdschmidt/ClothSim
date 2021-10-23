using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Rope : MonoBehaviour
{
    private Spring[] _springs;

    private GameObject[] _spheres;

    private PhysicsPoint[] _physicsPoints;

    private int numPoints = 10;
    // Start is called before the first frame update
    void Awake()
    {
        _spheres = new GameObject[numPoints];
        _physicsPoints = new PhysicsPoint[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            _spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _spheres[i].AddComponent<PhysicsPoint>();
            _spheres[i].transform.position = new Vector3(0, i, 0);
            _physicsPoints[i] = _spheres[i].GetComponent<PhysicsPoint>();
        }

        _spheres[numPoints-1].GetComponent<PhysicsPoint>().isAnchor = true;

        _springs = new Spring[numPoints-1];

        for (int i = 0; i < numPoints-1; i++)
        {
            _springs[i] = new Spring(_physicsPoints[i], _physicsPoints[i + 1], 3, 50, 10);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < numPoints-1; i++)
        {
            _springs[i].Move();
        }
    }

    /*private void OnDrawGizmos()
    {
        if (_physicsPoints.Length != 0)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < _physicsPoints.Length - 1; i++)
            {
                Gizmos.DrawLine(_physicsPoints[i].Position.position, _physicsPoints[i + 1].Position.position);
            }
        }
    }*/
}
