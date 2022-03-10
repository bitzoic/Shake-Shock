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
    [SerializeField]
    private Collider2D playerCollider;
    [SerializeField]
    private TrailRenderer playerTrail;
    [SerializeField]
    private ParticleSystem playerStrafeParticles;
    [SerializeField]
    private ParticleSystem playerDoubleJumpParticles;

    [Header("Settings")]
    [SerializeField]
    private float disableTime;

    #endregion

    #region Run-Time Fields

    private bool isGameRunning = false;
    private bool allowInput = true;
    private bool onGround = false;
    private int directionFacing = -1;

    #endregion

    #region Monobehaviors

    // Start is called before the first frame update
    void Start()
    {
        // Load all NFT data here
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ElectricOrb")
        {
            if (allowInput == true)
            {
                allowInput = false;
                StartCoroutine(ReEnableControls());
            }
        }
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

    public bool GetIsOnGround()
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

    public Collider2D GetCollider2D()
    {
        return playerCollider;
    }

    public bool GetAllowInput()
    {
        return allowInput;
    }

    public TrailRenderer GetTrailRenderer()
    {
        return playerTrail;
    }

    public ParticleSystem GetStrafeParticleSystem()
    {
        return playerStrafeParticles;
    }

    public ParticleSystem GetDoubleJumpParticleSystem()
    {
        return playerDoubleJumpParticles;
    }

    #endregion

    #region Private Methods



    #endregion

    #region Coroutines

    private IEnumerator ReEnableControls()
    {
        yield return new WaitForSeconds(disableTime);
        allowInput = true;
    }

    #endregion
}
