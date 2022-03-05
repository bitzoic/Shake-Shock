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

    [Header("General Settings")]
    [SerializeField]
    private int startGameWaitTime;

    #endregion

    #region Run-Time Fields

    private List<Player> players;
    private bool isRunning = false;

    #endregion

    #region Monobehaviors

    private void Awake()
    {
        if (main != null)
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


        StartCoroutine(WaitToStartGame());
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

    #endregion

    #region Private Methods

    private void GameStarted()
    {
        // Do stuff here to start the game
        isRunning = true;
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
