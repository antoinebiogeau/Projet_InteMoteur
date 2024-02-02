using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                PieceRotation piece = hit.collider.GetComponent<PieceRotation>();
                if (piece != null)
                {
                    piece.RotatePiece();
                }
            }
        }
    }
}