// --------------------------------------------------------------
// Shake Shock - Player                                 3/05/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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
    private SpriteRenderer playerSpriteRenderer;
    [SerializeField]
    private Text playerAddressText;
    [SerializeField]
    private PhotonView playerView;

    [Header("Effects")]
    [SerializeField]
    private TrailRenderer playerTrail;
    [SerializeField]
    private ParticleSystem playerStrafeParticles;
    [SerializeField]
    private ParticleSystem playerDoubleJumpParticles;

    [Header("Scripts")]
    [SerializeField]
    private PlayerHealth playerHealthScript;
    [SerializeField]
    private PlayerMovement playerMovementScript;
    [SerializeField]
    private Shield playerShieldScript;
    [SerializeField]
    private PlayerThrow playerThrowScript;

    [Header("Settings")]
    [SerializeField]
    private float disableTime;

    #endregion

    #region Run-Time Fields

    private bool isGameRunning = false;
    private bool allowInput = true;
    private bool onGround = false;
    private int directionFacing = -1;
    private PlayerMetadata metadata;
    private Camera cam;

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

    public PhotonView GetPhotonView()
    {
        return playerView;
    }

    public void SetCamera(Camera cam1)
    {
        cam = cam1;
    }

    public Camera GetCamera()
    {
        return cam;
    }

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

    public PlayerHealth GetPlayerHealth()
    {
        return playerHealthScript;
    }

    public void LoadMetadata(PlayerMetadata meta)
    {
        metadata = meta;
        playerMovementScript.SetJumpMultiplier(meta.GetJumpMultiplier());
        playerMovementScript.SetSpeedMultiplier(meta.GetSpeedMultiplier());
        playerMovementScript.SetStrafeMultiplier(meta.GetDashMultiplier());
        playerHealthScript.SetMaxHealth(meta.GetHealth());
        playerHealthScript.SetArmour(meta.GetArmor());
        playerShieldScript.SetShieldTime(meta.GetShieldTime());
        playerThrowScript.SetThrowableType(meta.GetThrowableType());
        playerSpriteRenderer.sprite = meta.GetPlayerSprite();
        playerAddressText.text = meta.GetWallet();
    }

    public PlayerMetadata GetPlayerMetadata()
    {
        return metadata;
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
