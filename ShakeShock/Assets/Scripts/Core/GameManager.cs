// --------------------------------------------------------------
// Shake Shock - GameManager                            3/05/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Load in players
        players = new List<Player>();

        // TODO:
        // Some NFT stuff here??? Who is playing!!!!

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

    public void EndGame()
    {
        isRunning = false;
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

    #endregion

    #region Coroutines

    private IEnumerator WaitToStartGame()
    {
        yield return new WaitForSeconds(startGameWaitTime);
        GameStarted();
    }

    #endregion
}
