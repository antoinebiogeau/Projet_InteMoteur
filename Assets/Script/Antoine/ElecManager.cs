using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecManager : MonoBehaviour
{
    public int player = 0;
    public GameObject[] puzzlePieces; 
    public bool isCompleted = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Camera camera1 = GameManager.Instance.GetPlayer(player).currentCamera.GetComponent<Camera>();
            Ray ray = camera1.ScreenPointToRay(Input.mousePosition);
            
            int layerMask = 1 << LayerMask.NameToLayer("UI");
            layerMask = ~layerMask;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log("Tag de l'objet touché: " + hit.collider.gameObject.tag);
                Part piece = hit.collider.GetComponent<Part>();
                if (piece != null)
                {
                    Debug.Log("rotate");
                    piece.RotatePiece();
                }
            }
        }
        if (!isCompleted && CheckIfPuzzleCompleted())
        {
            isCompleted = true;
            OnPuzzleComplete();
        }
    }

    bool CheckIfPuzzleCompleted()
    {
        foreach (var piece in puzzlePieces)
        {
            Part rotationScript = piece.GetComponent<Part>();
            if (rotationScript == null || !rotationScript.IsInCorrectPosition())
            {
                return false;
            }
        }
        return true;
    }

    void OnPuzzleComplete()
    {
        Debug.Log("Puzzle complété!");
        GameManager.Instance.validateElecs(player);
    }
}
