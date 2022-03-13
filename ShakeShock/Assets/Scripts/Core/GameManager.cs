// --------------------------------------------------------------
// Shake Shock - GameManager                            3/05/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    #region Static Fields

    public static GameManager main;

    #endregion

    #region Inspector Fields

    [Header("Dependencies")]
    [SerializeField]
    private CameraFollow cameraFollow;
    [SerializeField]
    private GameObject debugGameObject;
    [SerializeField]
    private GameObject gameManagerGameObject;
    [SerializeField]
    private Transform spawnLocation1;
    [SerializeField]
    private Transform spawnLocation2;
    [SerializeField]
    private GameObject playerPrefab;

    [Header("General Settings")]
    [SerializeField]
    private int startGameWaitTime;
    [SerializeField]
    private int endGameWaitTime;

    [Header("Debug")]
    [SerializeField]
    private bool debugMode;
    [SerializeField]
    private Player debugPlayer;
    [SerializeField]
    private bool muliplayerMode;

    #endregion

    #region Run-Time Fields

    private List<Player> players;
    private List<PlayerMetadata> playerMetadata;
    private bool isRunning = false;
    private bool gameStarted = false;
    private CanvasManager canvasManagerScript;
    private string walletAddress;
    private int lastScene;

    #endregion

    #region Monobehaviors

    private void Awake()
    {
        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(gameManagerGameObject);
        }
        else
        {
            Destroy(this);
        }

        playerMetadata = new List<PlayerMetadata>();
        players = new List<Player>();

        ReloadScene();

        lastScene = SceneManager.GetActiveScene().buildIndex;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (!debugMode)
            {
                debugGameObject.SetActive(false);
                //StartCoroutine(WaitToStartGame());
                if (PhotonNetwork.CountOfPlayers == 2)
                {
                    canvasManagerScript.StartGame(startGameWaitTime);
                }
                else
                {
                    WaitingForPlayers();
                }
            }
            else
            {
                players.Add(debugPlayer);
                SetCameraTargets();
                //debugPlayer.SetGameRunning(true);
                canvasManagerScript.StartGame(startGameWaitTime);
            }
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != lastScene)
        {
            lastScene = SceneManager.GetActiveScene().buildIndex;
            ReloadScene();
        }

        if (SceneManager.GetActiveScene().buildIndex == 2 && gameStarted == false)
        {
            if (PhotonNetwork.CountOfPlayers == 2 || debugMode)
            {
                canvasManagerScript.StartGame(startGameWaitTime);
                gameStarted = true;
            }
            else
            {
                canvasManagerScript.WaitingForPlayers();
            }
        }
    }

    #endregion

    #region Public Methods

    public List<Player> getPlayerList()
    {
        return players;
    }

    public void EndGame(Player loosingPlayer)
    {
        isRunning = false;
        Time.timeScale = 0;
        foreach (Player player in players)
        {
            player.SetGameRunning(false);
        }

        OnGameOver(loosingPlayer);
    }

    public void SetWalletAddress(string address)
    {
        walletAddress = address;
    }

    public bool IsGameRunning()
    {
        return isRunning;
    }

    public void AddNewPlayerMetadata(PlayerMetadata meta)
    {
        playerMetadata.Add(meta);
    }

    public List<PlayerMetadata> GetAllPlayerMetadata()
    {
        return playerMetadata;
    }

    public bool GetMultiplayerMode()
    {
        return muliplayerMode;
    }

    public PlayerMetadata GetPlayerMetadata(int index)
    {
        if (playerMetadata.Count >= index)
        {
            return null;
        }
        return playerMetadata[index];
    }

    public PlayerMetadata GetPlayerMetadata(string wallet)
    {
        foreach (PlayerMetadata player in playerMetadata)
        {
            if (wallet == player.GetWallet())
            {
                return player;
            }
        }

        return null;
    }

    public void DeletePlayerMetadata(PlayerMetadata metadata)
    {
        if (playerMetadata.Contains(metadata))
        {
            playerMetadata.Remove(metadata);
        }
    }

    public void StartGame()
    {
        // Do stuff here to start the game
        isRunning = true;
        foreach (Player player in players)
        {
            player.SetGameRunning(true);
        }
    }

    #endregion

    #region Private Methods

    private void ReloadScene()
    {
        // We're in game
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            LoadGame();
        }
        // We're in main menu
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {

        }
    }

    private void SetCameraTargets()
    {
        foreach (Player player in players)
        {
            cameraFollow.AddToTargetList(player.GetTransform());
        }
    }

    private void LoadMainMenu()
    {

    }

    private void WaitingForPlayers()
    {
        canvasManagerScript.WaitingForPlayers();
    }

    private void LoadGame()
    {
        Camera mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        spawnLocation1 = GameObject.Find("SpawnLocation1").transform;
        spawnLocation2 = GameObject.Find("SpawnLocation2").transform;
        canvasManagerScript = GameObject.Find("CanvasManager").GetComponent<CanvasManager>();

        // Load in players
        if (debugMode == false)
        {
            cameraFollow.ClearTargetList();
            GameObject debugGameObject = GameObject.Find("Debug");
            if (debugGameObject != null)
            {
                Destroy(debugGameObject);
            }
        }

        if (playerMetadata.Count > 0)
        {
            GameObject playerGameObject1 = null;
            if (muliplayerMode)
            {
                playerGameObject1 = PhotonNetwork.Instantiate(
                    "Player",
                    spawnLocation1.position,
                    Quaternion.identity
                    ); 
            }
            else
            {
                playerGameObject1 = Instantiate(
                    playerPrefab,
                    spawnLocation1.position,
                    Quaternion.identity
                    ) as GameObject;
            }
            Player player1Script = playerGameObject1.GetComponent<Player>();

            PlayerMetadata thisPlayerMetadata = null;

            foreach (PlayerMetadata data in playerMetadata)
            {
                if (data.GetWallet() == walletAddress)
                {
                    thisPlayerMetadata = data;
                }
            }

            cameraFollow.AddToTargetList(player1Script.GetTransform());
            player1Script.SetCamera(mainCamera);
            players.Add(player1Script);
            //player1Script.LoadMetadata(thisPlayerMetadata);
        }
    }

    private void OnGameOver(Player loosingPlayer)
    {
        Player winningPlayer = null;

        // Only 1 player there can't be a winning player. This should not happen
        if (players.Count == 1)
        {
            Debug.Log("ERROR: 2 Player were not in the game!");
            GoToMainMenu();
            return;
        }

        foreach (Player player in players)
        {
            if (player != loosingPlayer)
            {
                winningPlayer = player;
            }
        }

        if (winningPlayer == null)
        {
            Debug.Log("ERROR: Something went horribly wrong");
            GoToMainMenu();
            return;
        }

        int winnerIndex = players.IndexOf(winningPlayer) + 1;
        canvasManagerScript.GameOver(winnerIndex.ToString());

        string winningWallet = winningPlayer.GetPlayerMetadata().GetWallet();
        string loosingWallet = loosingPlayer.GetPlayerMetadata().GetWallet();

        // Wallet stuff here


        // Once we're done, we want to go to the main menu
        StartCoroutine(WaitToSwitchScenes());
    }

    private void GoToMainMenu()
    {
        players.Clear();
        SceneManager.LoadScene(0);
    }

    #endregion

    #region Coroutines

    private IEnumerator WaitToSwitchScenes()
    {
        yield return new WaitForSeconds(endGameWaitTime);
        GoToMainMenu();
    }

    #endregion
}
