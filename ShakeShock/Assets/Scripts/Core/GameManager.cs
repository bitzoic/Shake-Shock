// --------------------------------------------------------------
// Shake Shock - GameManager                            3/05/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("General Settings")]
    [SerializeField]
    private int startGameWaitTime;

    [Header("Debug")]
    [SerializeField]
    private bool debugMode;
    [SerializeField]
    private Player debugPlayer;

    #endregion

    #region Run-Time Fields

    private List<Player> players;
    private List<PlayerMetadata> playerMetadata;
    private bool isRunning = false;

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

        // We're in game
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            LoadGame();
        }
        // We're in main menu
        else if (SceneManager.GetActiveScene().buildIndex == 0)
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!debugMode)
        {
            debugGameObject.SetActive(false);
            StartCoroutine(WaitToStartGame());
        }
        else
        {
            isRunning = true;
            players.Add(debugPlayer);
            SetCameraTargets();
            debugPlayer.SetGameRunning(true);
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

        OnGameOver(loosingPlayer);
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

    #endregion

    #region Private Methods

    private void GameStarted()
    {
        // Do stuff here to start the game
        isRunning = true;
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

    private void LoadGame()
    {
        cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        spawnLocation1 = GameObject.Find("SpawnLocation1").transform;
        spawnLocation2 = GameObject.Find("SpawnLocation2").transform;

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
            GameObject playerGameObject1 = Instantiate(
                Resources.Load("Player"), 
                spawnLocation1.position, 
                Quaternion.identity
                ) as GameObject;
            Player player1Script = playerGameObject1.GetComponent<Player>();
            player1Script.LoadMetadata(playerMetadata[0]);
            players.Add(player1Script);
            cameraFollow.AddToTargetList(player1Script.GetTransform());
        }
        if (playerMetadata.Count > 1)
        {
            GameObject playerGameObject2 = Instantiate(
                Resources.Load("Player"), 
                spawnLocation2.position, 
                Quaternion.identity
                ) as GameObject;
            Player player2Script = playerGameObject2.GetComponent<Player>();
            player2Script.LoadMetadata(playerMetadata[2]);
            players.Add(player2Script);
            cameraFollow.AddToTargetList(player2Script.GetTransform());
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

        string winningWallet = winningPlayer.GetPlayerMetadata().GetWallet();
        string loosingWallet = loosingPlayer.GetPlayerMetadata().GetWallet();

        // Wallet stuff here


        // Once we're done, we want to go to the main menu
        GoToMainMenu();
    }

    private void GoToMainMenu()
    {
        players.Clear();
        SceneManager.LoadScene(0);
    }

    #endregion

    #region Coroutines

    private IEnumerator WaitToStartGame()
    {
        yield return new WaitForSeconds(startGameWaitTime);
        GameStarted();
    }

    #endregion
}
