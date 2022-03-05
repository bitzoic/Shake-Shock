// --------------------------------------------------------------
// Shake Shock - Player                                 3/05/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Inspector Fields

    [Header("Dependencies")]
    [SerializeField]
    private Rigidbody2D playerRigidbody2D;
    [SerializeField]
    private GameObject playerGameObject;
    [SerializeField]
    private Transform playerTransform;

    #endregion

    #region Run-Time Fields

    private bool isGameRunning = false;
    private bool onGround = false;
    private int directionFacing = -1;

    #endregion

    #region Monobehaviors

    // Start is called before the first frame update
    void Start()
    {
        // Load all NFT data here
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Public Methods

    public void SetGameRunning(bool status)
    {
        isGameRunning = status;
    }

    public Rigidbody2D GetRigidbody2D()
    {
        return playerRigidbody2D;
    }

    public GameObject GetGameObject()
    {
        return playerGameObject;
    }

    public Transform GetTransform()
    {
        return playerTransform;
    }

    public bool GetOnGround()
    {
        return onGround;
    }

    public void SetOnGround(bool status)
    {
        onGround = status;
    }

    public void SetDirectionFacing(int direction)
    {
        directionFacing = direction;
    }

    public int GetDirectionFacing()
    {
        return directionFacing;
    }

    #endregion

    #region Private Methods



    #endregion
}
