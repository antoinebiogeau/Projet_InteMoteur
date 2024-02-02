// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class DragAndDropPiece : MonoBehaviour
// {
//     private Vector3 startPosition;
//     private float startZPosition;
//     private float StartYPosition;
//     private bool isDragging = false;
//     [SerializeField] private bool isword;
//     //private TargetZone currentTargetZone = null;
//     [SerializeField] Rigidbody rb;
//
//     void Start()
//     {
//         Debug.Log(isword);
//         startZPosition = transform.position.z;
//         StartYPosition = transform.position.y;
//         rb = this.GetComponent<Rigidbody>();
//     }
//
//     void Update()
//     {
//         if (isDragging)
//         {
//             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//             if (Physics.Raycast(ray, out RaycastHit hit))
//             {
//                 Vector3 newPosition = hit.point;
//                 if (!isword)
//                 {
//                     newPosition.z = startZPosition;
//                 }
//                 else
//                 {
//                     newPosition.y = StartYPosition;
//                 }
//                 transform.position = newPosition;
//             }
//         }
//     }
//
//     void OnMouseDown()
//     {
//         Debug.Log("click");
//         startPosition = transform.position;
//         if (isword)
//         {
//             Debug.Log("eh t ki toi");
//             startPosition = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
//             StartYPosition = startPosition.y;
//             rb.useGravity = false;
//         }
//         isDragging = true;
//     }
//
//     void OnMouseUp()
//     {
//         isDragging = false;
//         if (currentTargetZone != null && currentTargetZone.IsPieceCorrectlyPlaced(gameObject) && !isword)
//         {
//             transform.position = currentTargetZone.GetPlacementPosition();
//         }
//         else if(isword)
//         {
//             Debug.Log("Placed");
//             rb.useGravity = true;
//         }
//         else
//         {
//             transform.position = startPosition;
//         }
//     }
//
//     void OnTriggerEnter(Collider other)
//     {
//         Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);
//         if (other.gameObject.CompareTag("TargetZone"))
//         {
//             currentTargetZone = other.GetComponent<TargetZone>();
//         }
//     }
//
//     void OnTriggerExit(Collider other)
//     {
//         Debug.Log("OnTriggerExit called with: " + other.gameObject.name);
//         if (other.gameObject.CompareTag("TargetZone"))
//         {
//             currentTargetZone = null;
//         }
//     }
// }
