// --------------------------------------------------------------
// Shake Shock - PlayerMovement                         3/05/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{

    #region Inspector Fields

    [Header("Dependencies")]
    [SerializeField]
    private Player player;

    [Header("Move")]
    [SerializeField]
    private float moveMinMultiplier;
    [SerializeField]
    private float moveMaxMultiplier;
    [SerializeField]
    private float moveRate;
    [SerializeField]
    private float moveResetThreshold;

    [Header("Strafe")]
    [SerializeField]
    private float strafeMultiplier;
    [SerializeField]
    private float strafeButtonTime;
    [SerializeField]
    private float strafeCoolDownTime;
    [SerializeField]
    private float trailTime;
    [SerializeField]
    private int trailEmissionRate;

    [Header("Jumping")]
    [SerializeField]
    private float jumpMaxMuliplier;
    [SerializeField]
    private float doubleJumpWaitTime;
    [SerializeField]
    private int doubleJumpParticleCount;

    #endregion

    #region Run-Time Fields

    private bool didDoubleJump = false;
    private bool didJump = false;
    private bool jumpReleased = false;

    private int currentDirectionMovementMultiplier = 0;
    private int lastDirection = -1;

    private float currentmoveMultiplier = 0;
    private float strafeLeftTime = 0;
    private float strafeRightTime = 0;
    private float strafeUsedTime = 0;
    private float jumpedTime;

    private float metaSpeedMultiplier = 1;
    private float metaJumpMultiplier = 1;
    private float metaStrafeMultiplier = 1;

    #endregion

    #region Monobehaviors

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.main.IsGameRunning() && player.GetAllowInput() && player.GetPhotonView().IsMine)
        {
            ProcessStrafing();
            ProcessJump();
            ProcessDirection();
        }
    }

    void FixedUpdate()
    {
        if (GameManager.main.IsGameRunning() && player.GetAllowInput() && player.GetPhotonView().IsMine)
        {
            ProcessMovement();
        }
    }

    #endregion

    #region Public methods

    public void ResetJump()
    {
        didDoubleJump = false;
        didJump = false;
    }

    public void SetStrafeMultiplier(float val)
    {
        metaStrafeMultiplier = val;
    }

    public void SetJumpMultiplier(float val)
    {
        metaJumpMultiplier = val;
    }

    public void SetSpeedMultiplier(float val)
    {
        metaSpeedMultiplier = val;
    }

    #endregion

    #region Private Methods
    private void ProcessMovement()
    {
        // Move Left
        if (Input.GetKey(KeyCode.A))
        {
            MoveLeft();
            currentDirectionMovementMultiplier = -1;
        }
        // Move right
        if (Input.GetKey(KeyCode.D))
        {
            MoveRight();
            currentDirectionMovementMultiplier = 1;
        }
        // If we are moving below a threshold then reset the moving multiplier
        if (Mathf.Abs(player.GetRigidbody2D().velocity.x) < moveResetThreshold)
        {
            currentDirectionMovementMultiplier = 0;
        }
    }

    private void ProcessDirection()
    {
        bool aPressed = false;
        bool dPressed = false;
        if (Input.GetKey(KeyCode.A))
        {
            aPressed = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dPressed = true;
        }

        if (aPressed && !dPressed)
        {
            player.SetDirectionFacing(-1);
        }
        else if (!aPressed && dPressed)
        {
            player.SetDirectionFacing(1);
        }
        else if (aPressed && dPressed)
        {
            if (player.GetRigidbody2D().velocity.x > 0)
            {
                player.SetDirectionFacing(1);
            }
            else if (player.GetRigidbody2D().velocity.x < 0)
            {
                player.SetDirectionFacing(-1);
            }
        }
    }

    private void ProcessStrafing()
    {
        // Strafe
        if (Input.GetKeyDown(KeyCode.A))
        {
            StrafeLeft();
            lastDirection = -1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            StrafeRight();
            lastDirection = 1;
        }
    }

    private void ProcessJump()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpReleased = true;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
    }

    private void StrafeLeft()
    {
        // If we just strafed don't do it again
        if (Time.time - strafeUsedTime <= strafeCoolDownTime)
        {
            return;
        }

        // If we pressed button quick enough
        if (Time.time - strafeLeftTime <= strafeButtonTime && lastDirection == -1)
        {
            player.GetRigidbody2D().AddForce(player.GetTransform().right * -strafeMultiplier * metaStrafeMultiplier);
            strafeUsedTime = Time.time;
            EnableTrail();
        }
        else
        {
            strafeLeftTime = Time.time;
        }
    }

    private void StrafeRight()
    {
        // If we just strafed don't do it again
        if (Time.time - strafeUsedTime <= strafeCoolDownTime)
        {
            return;
        }

        // If we pressed button quick enough
        if (Time.time - strafeRightTime <= strafeButtonTime && lastDirection == 1)
        {
            player.GetRigidbody2D().AddForce(player.GetTransform().right * strafeMultiplier * metaStrafeMultiplier);
            strafeUsedTime = Time.time;
            EnableTrail();
        }
        else
        {
            strafeRightTime = Time.time;
        }
    }

    private void MoveLeft()
    {
        if (currentDirectionMovementMultiplier != -1)
        {
            currentmoveMultiplier = moveMaxMultiplier;
        }

        if (currentmoveMultiplier > moveMinMultiplier)
        {
            currentmoveMultiplier -= moveRate;
        }
        else
        {
            currentmoveMultiplier = moveMinMultiplier;
        }

        player.GetRigidbody2D().AddForce(player.GetTransform().right * -currentmoveMultiplier * metaSpeedMultiplier);
    }

    private void MoveRight()
    {
        if (currentDirectionMovementMultiplier != 1)
        {
            currentmoveMultiplier = moveMaxMultiplier;
        }

        if (currentmoveMultiplier > moveMinMultiplier)
        {
            currentmoveMultiplier -= moveRate;
        }
        else
        {
            currentmoveMultiplier = moveMinMultiplier;
        }
        player.GetRigidbody2D().AddForce(player.GetTransform().right * currentmoveMultiplier * metaSpeedMultiplier);
    }

    private void Jump()
    {
        if ((!player.GetIsOnGround() && didDoubleJump == true) 
            || (!player.GetIsOnGround() && jumpReleased == false && didJump == true)
            || Time.time - jumpedTime < doubleJumpWaitTime)
        {
            return;
        }

        if (!player.GetIsOnGround() && didDoubleJump == false && didJump == true)
        {
            didDoubleJump = true;
            EnableDoubleJumpParticles();
        }

        player.GetRigidbody2D().AddForce(player.GetTransform().up * jumpMaxMuliplier * metaJumpMultiplier);
        jumpedTime = Time.time;
        jumpReleased = false;
        didJump = true;
    }

    private void EnableTrail()
    {
        player.GetTrailRenderer().time = trailTime;
        player.GetTrailRenderer().emitting = true;
        player.GetStrafeParticleSystem().emissionRate = trailEmissionRate;
        StartCoroutine(DisableTrail());
    }

    private void EnableDoubleJumpParticles()
    {
        player.GetDoubleJumpParticleSystem().Emit(doubleJumpParticleCount);
    }

    #endregion


    #region Coroutine

    private IEnumerator DisableTrail()
    {
        yield return new WaitForSeconds(trailTime);
        player.GetTrailRenderer().emitting = false;
        player.GetStrafeParticleSystem().emissionRate = 0;
    }

    #endregion
}
