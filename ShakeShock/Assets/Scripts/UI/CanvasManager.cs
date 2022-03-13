// --------------------------------------------------------------
// Shake Shock - GameManager                            3/12/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    #region Static Fields

    public static CanvasManager main;

    #endregion

    #region Inspector Fields

    [Header("Dependencies")]
    [SerializeField]
    private GameObject canvasManager;
    [SerializeField]
    private GameObject gameStartStopPanel;
    [SerializeField]
    private Text gameStartStopText;

    [Header("Settings")]
    [SerializeField]
    private float fightTextDisplayTime;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Public Methods

    public void StartGame(float time)
    {
        StartCoroutine(WaitToStartGame(time));
    }

    public void GameOver(string winner)
    {
        gameStartStopText.text = "GAME OVER - Player " + winner + " Won!";
        gameStartStopPanel.SetActive(true);
    }

    public void WaitingForPlayers()
    {
        gameStartStopText.text = "Waiting for players...";
    }

    #endregion

    #region Coroutines

    private IEnumerator WaitToStartGame(float waitSeconds)
    {
        yield return new WaitForSeconds(0);
        float startTime = Time.time;

        while (Time.time - startTime < waitSeconds)
        {
            float seconds = (waitSeconds - (Time.time - startTime));
            gameStartStopText.text = "Game starting in " 
                + seconds.ToString("F2") 
                + " seconds";
            yield return new WaitForSeconds(0);
        }

        gameStartStopText.text = "Fight!";
        yield return new WaitForSeconds(fightTextDisplayTime);

        gameStartStopPanel.SetActive(false);
        GameManager.main.StartGame();
    }

    #endregion
}
