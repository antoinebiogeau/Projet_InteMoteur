using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPieces : MonoBehaviour
{
    public int player = 0;
    public Transform door;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void GoToDoor()
    {
        //lerp vers la porte
        transform.position = door.position;
        transform.rotation = door.rotation;
        GameManager.Instance.addPiece(player);
        Debug.Log("GoToDoor");
    }
}
