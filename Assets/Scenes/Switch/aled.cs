using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;

public class aled : MonoBehaviour
{
    public int playerNumber = 1;
    private Camera currentCamera;
    public GameObject selectedObject;
    private Vector3 offset;
    private bool isDragging = false;
    private int layerMask;
    private Rigidbody selectedRigidbody;
    public Transform doorPieceTarget;
    public float moveSpeed = 4f;
    public RectTransform cursorUI; 
    private Vector3 worldCursorPos;
    private Vector2 cursorPosition;
    public BallonSpawner ballonSpawner;
    public GameObject uiComptageBallons;
    public GameObject uiPointer;
    public GameObject uiPointer2;
    GameObject previousCam;
    public RectTransform cursor;
    public RectTransform canvasRectTransform; 
    public float sensitivity = 100f;
    private List<Joycon> joycons;
    public RectTransform imageToMove;
    public float moveSpeedJ = 0.1f; 
    private Vector3 prevAccel;
    public float lowPassFilterFactor = 0.2f; 
    public Canvas canvas;
    float canvasHalfWidth;
    float canvasHalfHeight;
    public Vector3 gyro;
    public Vector3 accel;
    public Quaternion orientation;
    public Quaternion targetRotation = Quaternion.Euler(0, 90, 0);
    public float distanceFromCamera = 10.0f;
    public bool RotatePieceAL = false;
    public GameObject MovableObject;
    public float rotationSmoothness = 1.0f;
    public float snapStrength = 0.1f;
    private Quaternion lastJoyconRotation; 
    private bool hasLastJoyconRotation = false;
    private Quaternion currentJoyconRotation;
    private Vector3 targetPosition;
    private float liftHeight = 5f;
    private bool isMovingToObject = false;
    public int jc_ind = 0;

    void Start()
    {
        gyro = new Vector3(0, 0, 0);
        joycons = JoyconManager.Instance.j;
        if (joycons.Count == 0)
        {
            Debug.LogError("Aucun Joy-Con connecté !");
        }
        prevAccel = Vector3.zero;
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        canvasHalfWidth = canvasRect.sizeDelta.x / 2;
        canvasHalfHeight = canvasRect.sizeDelta.y / 2;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        currentCamera = GameManager.Instance.GetPlayer(playerNumber).currentCamera.GetComponent<Camera>();
        canvasRectTransform.GetComponent<Canvas>().worldCamera = currentCamera;
        previousCam = currentCamera.gameObject;
        if (currentCamera == null)
        {
            Debug.LogError("La caméra n'est pas assignée dans le GameManager.");
        }

        layerMask = 1 << LayerMask.NameToLayer("UI");
        layerMask = ~layerMask;
    }

    private void FixedUpdate()
    {
        if (joycons.Count > 0)
        {
            Joycon joycon = joycons[jc_ind];
            if (joycon.GetButtonDown(Joycon.Button.SHOULDER_2))
            {
                Debug.Log("Shoulder 2 pressed");
                Vector2 screenPosition = ConvertUIToScreenPosition(cursorUI);
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = screenPosition;
                RaycastUI(pointerEventData);
            }
            if (joycon.GetButtonDown(Joycon.Button.DPAD_DOWN)) 
            {
                Debug.Log("DPAD_DOWN pressed");
                Vector2 screenPosition = ConvertUIToScreenPosition(cursorUI);
                Ray ray = currentCamera.ScreenPointToRay(screenPosition);
                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 1f);

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    if (hit.collider.CompareTag("MovableObject"))
                    {
                        Debug.Log("MovableObject");
                        if (!isDragging)
                        {
                            selectedObject = hit.collider.gameObject;
                            TrySelectObject(selectedObject);
                        }
                        else
                        {
                            targetPosition = hit.collider.gameObject.transform.position;
                            isMovingToObject = true;
                        }
                    }

                    if (hit.collider.CompareTag("DropZone")&& selectedObject != null)
                    {
                        targetPosition = hit.collider.gameObject.transform.position;
                        isMovingToObject = true;
                    }

                    if (hit.collider.CompareTag("CountBloons"))
                    {
                        Debug.Log("red" + GameManager.Instance.GetPlayer(playerNumber).balloons[0] + "green" + GameManager.Instance.GetPlayer(playerNumber).balloons[1]);
                        uiComptageBallons.SetActive(true);
                    }
                }

                if (hit.collider.CompareTag("DoorPiece"))
                {
                    
                    selectedObject = hit.collider.gameObject;
                    Debug.Log("DoorPiece" + selectedObject.transform.rotation);
                    Debug.Log("DoorPiece");
                    selectedObject.GetComponent<Rigidbody>().useGravity = false;
                    selectedObject.transform.position = currentCamera.transform.position + currentCamera.transform.forward * distanceFromCamera;
                    Debug.Log("DoorPiece" + selectedObject.transform.rotation);
                    RotatePieceAL = true;

                }

                Part piece = hit.collider.GetComponent<Part>();
                if (piece != null)
                {
                    piece.RotatePiece();
                }
                else
                {
                    Debug.Log("piou");
                }
            }
        }
    }

    void Update()
    {
        Debug.Log(RotatePieceAL);
        if (joycons.Count > 0)
        {
            Joycon joycon = joycons[jc_ind];
            Vector3 currentAccel = joycon.GetAccel();
            orientation = joycon.GetVector();
             currentJoyconRotation = joycon.GetVector();
            if (RotatePieceAL == true)
            {
                RotatePieceALi();
            }
            if(isMovingToObject)
            {
                MoveObjectToTarget();
            }
            Vector3 filteredAccel = Vector3.Lerp(prevAccel, currentAccel, lowPassFilterFactor);

            Vector2 moveDelta = new Vector2(-filteredAccel.y, filteredAccel.x) * moveSpeed;
            Vector2 potentialPosition = imageToMove.anchoredPosition + moveDelta;
            potentialPosition.x = Mathf.Clamp(potentialPosition.x, -canvasHalfWidth, canvasHalfWidth);
            potentialPosition.y = Mathf.Clamp(potentialPosition.y, -canvasHalfHeight, canvasHalfHeight);
            imageToMove.anchoredPosition = potentialPosition;
            prevAccel = filteredAccel;

            currentCamera = GameManager.Instance.GetPlayer(playerNumber).currentCamera.GetComponent<Camera>();
            if (currentCamera.gameObject != previousCam)
            {
                previousCam = currentCamera.gameObject;
                uiPointer.GetComponent<Canvas>().worldCamera = currentCamera;
                uiPointer2.GetComponent<Canvas>().worldCamera = currentCamera;
                Debug.Log("Camera changed");
            }

            cursorPosition = cursorUI.anchoredPosition;
            worldCursorPos = currentCamera.ScreenToWorldPoint(new Vector3(cursorPosition.x, cursorPosition.y, currentCamera.nearClipPlane));
        }
    }
    private Vector2 ConvertUIToScreenPosition(RectTransform uiElement)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(currentCamera, uiElement.position);
        return screenPoint;
    }
    private void RaycastUI(PointerEventData pointerEventData)
    {
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<Button>() != null)
            {
                ExecuteEvents.Execute(result.gameObject, pointerEventData, ExecuteEvents.submitHandler);
                break;
            }
        }
    }
    private void RotatePieceALi()
    {
        
        if (!hasLastJoyconRotation)
        {
            lastJoyconRotation = currentJoyconRotation;
            hasLastJoyconRotation = true;
        }
        Quaternion rotationDelta = Quaternion.Inverse(lastJoyconRotation) * currentJoyconRotation;
        selectedObject.transform.rotation *= rotationDelta;
        lastJoyconRotation = currentJoyconRotation;
        Debug.Log("Delta Rotation Applied");
        float angleDiff = Quaternion.Angle(selectedObject.transform.rotation, Quaternion.Euler(0, 90, -90));
        transform.position = currentCamera.transform.position + currentCamera.transform.forward * distanceFromCamera;
        if (angleDiff < 20f)
        {
            selectedObject.transform.rotation = Quaternion.Lerp(selectedObject.transform.rotation, targetRotation, Time.deltaTime * snapStrength);
        }

        if (angleDiff < 20.0f)
        {
            Debug.Log("Rotation Finished");
            uiPointer.SetActive(true);
            selectedObject.GetComponent<DoorPieces>().GoToDoor(); 
            selectedObject.tag = "Untagged";
            Destroy(selectedObject.GetComponent<Rigidbody>());
            Destroy(selectedObject.GetComponent<MeshCollider>());
            selectedObject = null;
            RotatePieceAL = false;
            
        }
    }
    private void TrySelectObject(GameObject selectedObject)
    {
                selectedRigidbody = selectedObject.GetComponent<Rigidbody>();
                isDragging = true;
                selectedRigidbody.isKinematic = true;
                targetPosition = new Vector3(selectedObject.transform.position.x, liftHeight, selectedObject.transform.position.z);
                selectedObject.transform.position = Vector3.Lerp(selectedObject.transform.position, targetPosition, Time.deltaTime * 10f);
        
    }
    private void MoveObjectToTarget()
    {
        Vector3 newPosition = new Vector3(targetPosition.x, liftHeight, targetPosition.z);
        selectedObject.transform.position = Vector3.Lerp(selectedObject.transform.position, newPosition, Time.deltaTime * 10f);
        if (Vector3.Distance(selectedObject.transform.position, newPosition) < 0.1f)
        {
            isDragging = false;
            isMovingToObject = false;
            selectedRigidbody.isKinematic = false;
            selectedObject = null;
        }
    }


}
