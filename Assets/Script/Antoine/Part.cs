using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    public int correctClickCount; 
    private int currentClickCount = 0; 

    public void RotatePiece()
    {
        currentClickCount = (currentClickCount + 1) % 4; 
        transform.Rotate(0, 0, 90); 
    }

    public bool IsInCorrectPosition()
    {
        return currentClickCount == correctClickCount;
    }
}
