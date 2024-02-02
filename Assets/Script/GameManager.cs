using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float totalTime = 120.0f;
    private float timeElapsed;

    public static GameManager Instance;
    public UIManager uiManager;
    public Image timerBar;
    public int totalPieces = 13;

    [System.Serializable]
    public class Player
    {
        public List<GameObject> cameras;
        public GameObject currentCamera;
        public List<GameObject> scenes;
        public int[] balloons = new int[2]; // 0: red, 1: blue
        public int piecesPlaced = 0;
        public bool[] puzzlesCompleted = new bool[4];
    }

    public Player player1 = new Player();
    public Player player2 = new Player();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePlayers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        timeElapsed = 0f;
    }

    void Update()
    {
        if (timeElapsed < totalTime)
        {
            timeElapsed += Time.deltaTime;
            UpdateTimerBar();
        }
        else
        {
            GameOver();
        }
    }

    void InitializePlayers()
    {
        SetActiveCamera(player1, 0);
        SetActiveCamera(player2, 0);
        player1.scenes.ForEach(scene => scene.SetActive(false));
        player2.scenes.ForEach(scene => scene.SetActive(false));
        player1.scenes[0].SetActive(true);
        player2.scenes[0].SetActive(true);
    }

    void GameOver()
    {
        Debug.Log("Temps écoulé! Jeu terminé.");
        uiManager.GameOver();
    }

    public void AddBalloon(int playerIndex, int colorIndex)
    {
        Player player = playerIndex == 1 ? player1 : player2;
        player.balloons[colorIndex]++;
        Debug.Log(player + " " + colorIndex + " " + player.balloons[colorIndex]);
    }

    public bool ValidateBloons(int playerIndex, int UIredBallons, int UIblueBallons)
    {
        Player player = playerIndex == 1 ? player1 : player2;
        if (player.balloons[0] == UIredBallons && player.balloons[1] == UIblueBallons)
        {
            player.puzzlesCompleted[0] = true;
            ChangeScene(playerIndex, 1);
            return true;
        }
        return false;
    }

    public void ChangeScene(int playerIndex, int sceneIndex)
    {
        Player player = playerIndex == 1 ? player1 : player2;
        if(sceneIndex < player.scenes.Count)
        {
            player.currentCamera.SetActive(false);
            player.currentCamera = player.cameras[sceneIndex];
            player.currentCamera.SetActive(true);

            player.scenes.ForEach(scene => scene.SetActive(false));
            player.scenes[sceneIndex].SetActive(true);
        }
    }

    void UpdateTimerBar()
    {
        if (timerBar != null)
        {
            timerBar.fillAmount = timeElapsed / totalTime;
        }
    }

    public void SwitchCamAnt(int playerIndex)
    {
        Player player = playerIndex == 1 ? player1 : player2;
        int currentCameraIndex = player.cameras.IndexOf(player.currentCamera);
        currentCameraIndex = (currentCameraIndex + 1) % player.cameras.Count;
        SetActiveCamera(player, currentCameraIndex);
    }

    private void SetActiveCamera(Player player, int index)
    {
        player.currentCamera = player.cameras[index];
        player.cameras.ForEach(cam => cam.SetActive(false));
        player.currentCamera.SetActive(true);
    }
    public Player GetPlayer(int playerIndex)
    {
        return playerIndex == 1 ? player1 : player2;
    }
    public void validateWord(int playerIndex)
    {
        Player player = playerIndex == 1 ? player1 : player2;
        player.puzzlesCompleted[2] = true;
        ChangeScene(playerIndex, 3);
    }
    public void validateElecs(int playerIndex)
    {
        Player player = playerIndex == 1 ? player1 : player2;
        player.puzzlesCompleted[3] = true;
        Debug.Log("Puzzle complété!");
    }
    public void addPiece(int playerIndex)
    {
        Player player = playerIndex == 1 ? player1 : player2;
        player.piecesPlaced++;
        Debug.Log(player.piecesPlaced);
        if (player.piecesPlaced == totalPieces)
        {
            player.puzzlesCompleted[1] = true;
            ChangeScene(playerIndex, 2);
        }
    }
}
