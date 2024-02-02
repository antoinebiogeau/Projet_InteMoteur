using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTarget : MonoBehaviour
{
    public int player;
    public GameObject correctPiece;
    private float validationThreshold = 0.8f;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == correctPiece)
        {
            Bounds pieceBounds = other.bounds;
            Bounds targetBounds = GetComponent<Collider>().bounds;

            Vector3 overlapSize = Vector3.Min(pieceBounds.max, targetBounds.max) -
                                  Vector3.Max(pieceBounds.min, targetBounds.min);
            Vector3 pieceSize = pieceBounds.size;
            float overlapVolume = overlapSize.x * overlapSize.y * overlapSize.z;
            float pieceVolume = pieceSize.x * pieceSize.y * pieceSize.z;
            float overlapPercentage = overlapVolume / pieceVolume;

            if (overlapPercentage >= validationThreshold)
            {
                Debug.Log("Pièce correctement placée");
                PlacePiece(other.gameObject);
            }
        }
    }

    private void PlacePiece(GameObject piece)
    {
        piece.transform.position = transform.position;
        piece.tag = "Placed";
        Rigidbody rb = piece.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            GameManager.Instance.addPiece(player);
            piece.transform.position = transform.position;
            piece.transform.rotation = transform.rotation;
        }

        Collider col = piece.GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }
}
