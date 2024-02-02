using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropHandler : MonoBehaviour
{
    private Camera currentCamera;
    private GameObject selectedObject;
    private Vector3 offset;
    private bool isDragging = false;
    private int layerMask; 
    private Rigidbody selectedRigidbody;
    public Transform doorPieceTarget;
    public float moveSpeed = 2f;

    void Start()
    {
        currentCamera = GameManager.Instance.GetPlayer(1).currentCamera.GetComponent<Camera>();
        if (currentCamera == null)
        {
            Debug.LogError("La caméra n'est pas assignée dans le GameManager.");
        }

        layerMask = 1 << LayerMask.NameToLayer("UI");
        layerMask = ~layerMask; 
    }

    public void Update()
    {
        currentCamera = GameManager.Instance.GetPlayer(1).currentCamera.GetComponent<Camera>();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 1f);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.CompareTag("CountBloons"))
                {
                    //Debug.Log("red" + GameManager.Instance.redBallons + "blue" + GameManager.Instance.blueBallons);
                }
                
                if (hit.collider.CompareTag("MovableObject"))
                {
                    Debug.Log("Objet avec tag 'MovableObject' touché"+ hit.collider.gameObject.tag);
                    selectedObject = hit.collider.gameObject;
                    selectedRigidbody = selectedObject.GetComponent<Rigidbody>();
                    if (selectedRigidbody != null)
                    {
                        selectedRigidbody.useGravity = false; 
                    }
                    float distanceToObject = Vector3.Distance(selectedObject.transform.position, currentCamera.transform.position);
                    offset = selectedObject.transform.position - currentCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToObject));
                    isDragging = true;
                }

                if (hit.collider.CompareTag("DoorPiece"))
                {
                    Debug.Log("Objet avec tag 'MovableObject' touché"+ hit.collider.gameObject.tag);
                    selectedObject = hit.collider.gameObject;
                    selectedRigidbody = selectedObject.GetComponent<Rigidbody>();
                    if (selectedRigidbody != null)
                    {
                        selectedRigidbody.useGravity = false; 
                    }
                    float distanceToObject = Vector3.Distance(selectedObject.transform.position, currentCamera.transform.position);
                    offset = selectedObject.transform.position - currentCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToObject));
                    isDragging = true;
                }
                else
                {
                    Debug.Log("piou");
                }
            }
        }

        if (isDragging && selectedObject != null)
        {
            if (selectedObject.CompareTag("DoorPiece"))
            {
                Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    Vector3 mousePosition = new Vector3(Mathf.Lerp(selectedObject.transform.position.x,doorPieceTarget.position.x , moveSpeed * Time.deltaTime), hit.point.y, hit.point.z);
                    //Vector3 mousePosition = new Vector3(doorPieceTarget.position.x, hit.point.y, );
                    selectedObject.transform.position = mousePosition;
                }
            }
            else
            {
                Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    Vector3 mousePosition = new Vector3(selectedObject.transform.position.x, hit.point.y, hit.point.z);
                    selectedObject.transform.position = mousePosition;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectedRigidbody != null)
            {
                selectedRigidbody.useGravity = true;
                isDragging = false;
                selectedObject = null;
                selectedRigidbody = null;
            }
        }
    }
    
}
