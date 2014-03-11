﻿using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

    public Vector3 MoveToPosition;
    public bool Walking = false;

	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveToPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            Walking = true;
        }

        if (Walking)
        {
            // move
            // Get the direction from the worker to the building and reset the z axis
            Vector3 Direction = MoveToPosition - transform.position;
            Direction = new Vector3(Direction.x, Direction.y, 0);

            // Calculate the movement for this frame and add this to the position
            Vector3 Movement = Direction.normalized * Time.deltaTime * 2;
            transform.position += Movement;

            // If the movement is bigger than the actual length to walk in total, switch to idle
            //  (-0.01f is a small margin to make sure we always switch)
            if (Mathf.Abs(Movement.x) > Mathf.Abs(Direction.x) - 0.01f && Mathf.Abs(Movement.y) > Mathf.Abs(Direction.y) - 0.01f)
            {
                Walking = false;
            }
        }
    }
}