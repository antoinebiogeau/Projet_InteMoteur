using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallonSpawner : MonoBehaviour
{
    public int player = 0;
    private int compteurBallonType1 = 0;
    private int compteurBallonType2 = 0;
    public GameObject ballonType1;
    public GameObject ballonType2;
    public int nombreTotalDeBallons;
    public float delaiEntreBallons = 1.0f;
    public Vector3 zoneDeSpawnMin; 
    public Camera camera;
    public GameObject uiComptageBallons;
    public BallonManager ballonManager;
    void Start()
    {
        camera = GameManager.Instance.GetPlayer(player).currentCamera.GetComponent<Camera>();
        StartCoroutine(SpawnBallons());
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("click");
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            int layerMask = 1 << LayerMask.NameToLayer("UI");
            layerMask = ~layerMask;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log("Tag de l'objet touché: " + hit.collider.gameObject.tag);

                if (hit.collider.CompareTag("CountBloons"))
                {
                    Debug.Log("red" +compteurBallonType1 + "green" + compteurBallonType2);
                    uiComptageBallons.SetActive(true);
                }
            }
        }
    }

    IEnumerator SpawnBallons()
    {
        
        for (int i = 0; i < nombreTotalDeBallons; i++)
        {
            SpawnBallon();
            yield return new WaitForSeconds(delaiEntreBallons);
        }
    }

    void SpawnBallon()
    {
        GameObject ballon = Random.Range(0, 2) == 0 ? ballonType1 : ballonType2;
        if (ballon == ballonType1)
        {
            compteurBallonType1++;
            GameManager.Instance.AddBalloon(player, 0);
        }
        else
        {
            compteurBallonType2++;
            GameManager.Instance.AddBalloon(player, 1);
        }

        Vector3 positionDeSpawn = zoneDeSpawnMin;
        GameObject nouveauBallon = Instantiate(ballon, positionDeSpawn, Quaternion.identity);
        BallonMouvement ballonMouvement = nouveauBallon.GetComponent<BallonMouvement>();
        ballonMouvement.player = player;
        ballonMouvement.SetRandomTargetPosition();
        nouveauBallon.transform.parent = transform;
    }
    public int GetCompteurBallonType1()
    {
        return compteurBallonType1;
    }
    public int GetCompteurBallonType2()
    {
        return compteurBallonType2;
    }
    
    
}
