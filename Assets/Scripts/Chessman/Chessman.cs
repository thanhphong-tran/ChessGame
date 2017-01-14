﻿using UnityEngine;
using System.Collections;
using System;

public abstract class Chessman : MonoBehaviour {
    public int CurrentX { get; set; }
    public int CurrentY { get; set; }
    public bool isWhite;

    public abstract string Annotation();

    private Quaternion originRotation;
    private float speed = 1f;
    private float incSpeed = 0.01f;

    private void Start()
    {
        originRotation = transform.rotation;
    }

    private void Update()
    {
        if (newPosition != Vector3.zero)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);

            speed += incSpeed;

            if (transform.position == newPosition)
            {
                newPosition = Vector3.zero;
                speed = 1f;
            }
                
        }
    }

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool[,] PossibleMove()
    {
        return new bool[8, 8];
    }

    public virtual bool[,] PossibleEat()
    {
        return new bool[8, 8];
    }

    public bool CanGo(int x, int y)
    {
        bool[,] possible = this.PossibleMove();

        return possible[x, y];
    }

    internal void RotateEach(float seconds)
    {
        InvokeRepeating("Rotate", 0f, seconds);
    }

    private int t = 5;
    private int dt = 5;
    private void Rotate()
    {
        transform.Rotate(Vector3.up * t);
        t += dt;
    }

    internal void DestroyAfter(float seconds)
    {
        Invoke("DestroyGameObject", seconds);
    }

    private void DestroyGameObject()
    {
        BoardManager.Instance.GetAllChessmans().Remove(gameObject);
        Destroy(gameObject);
    }

    private Vector3 newPosition = Vector3.zero;
    internal void MoveAfter(float seconds, Vector3 position)
    {
        newPosition = position;
        Invoke("Move", seconds);
    }

    private void Move()
    {
        //transform.position = newPosition;

        // stop rotation
        // CancelInvoke("Rotate");

        // restore origin rotation
        transform.rotation = originRotation;

        // restore rotate speed
        t = 10;
    }
}