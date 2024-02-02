using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropW : MonoBehaviour
{
    private Vector3 startPosition;
    private bool isDragging = false;

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, startPosition.z - Camera.main.transform.position.z);
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector3(newPosition.x, newPosition.y, startPosition.z);
        }
    }

    void OnMouseDown()
    {
        startPosition = transform.position;
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }
}
