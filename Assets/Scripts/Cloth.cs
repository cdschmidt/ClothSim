using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloth : MonoBehaviour
{
    private PhysicsPoint[,] clothPoints;

    private List<Spring> clothConnections;
    // Start is called before the first frame update

    private GameObject[,] _spheres;

    public float restLeng = 1;
    public float sk = 500;
    public float dk = 10;

    public int gridSize = 10;
    // Start is called before the first frame update
    void Awake()
    {
        _spheres = new GameObject[gridSize, gridSize];
        clothPoints = new PhysicsPoint[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                _spheres[i,j] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                _spheres[i,j].AddComponent<PhysicsPoint>();
                Rigidbody rb = _spheres[i,j].AddComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;
                _spheres[i,j].transform.position = new Vector3(0, i, j);
                _spheres[i, j].transform.localScale = new Vector3(.5f, .5f, .5f);
                clothPoints[i,j] = _spheres[i,j].GetComponent<PhysicsPoint>();
            }
            
        }

        int clothConnectionsSize = ((gridSize * (gridSize - 1)) * 2) + (2 * (gridSize - 1) * (gridSize - 1));
        clothConnections = new List<Spring>();
        int clothConnectionsIndex = 0;
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize-1; j++)
            {
                clothConnections.Add(new Spring(clothPoints[i,j], clothPoints[i, j+1], restLeng, sk, dk));
            }
        }
        
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize-1; j++)
            {
                clothConnections.Add(new Spring(clothPoints[j,i], clothPoints[j+1, i], restLeng, sk, dk));
            }
        }

        float diagonalRestLength = (float)Math.Sqrt(Math.Pow(restLeng, 2) + Math.Pow(restLeng, 2));
        
        for (int i = 0; i < gridSize-1; i++)
        {
            for (int j = 0; j < gridSize-1; j++)
            {
                clothConnections.Add(new Spring(clothPoints[i, j], clothPoints[i + 1, j + 1], diagonalRestLength, sk, dk));
            }
        }
        
        for (int i = 0; i < gridSize-1; i++)
        {
            for (int j = 1; j < gridSize; j++)
            {
                clothConnections.Add(new Spring(clothPoints[i, j], clothPoints[i+1, j-1], diagonalRestLength, sk, dk));
            }
        }

        clothPoints[gridSize-1, 0].isAnchor = true;
        clothPoints[gridSize-1, gridSize-1].isAnchor = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(clothConnections == null) return;
        foreach (var spring in clothConnections)
        {
            Gizmos.DrawLine(spring._a.Position.position, spring._b.Position.position);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (var spring in clothConnections)
        {
            if (spring.broken) clothConnections.Remove(spring);
            spring.Move();
        }
    }
}
