using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallonMouvement : MonoBehaviour
{
    public int player = 1;
    public float moveSpeed = 2.0f;
    private Vector3 targetPosition;
    private bool shouldMove = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (shouldMove)
        {
            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetRandomTargetPosition();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    public void SetRandomTargetPosition()
    {
        targetPosition = BallonManager.Instance.GetRandomPosition(player);
        shouldMove = true;
    }
}
