// --------------------------------------------------------------
// Shake Shock - PlayerMovement                         3/05/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Jumping")]
    [SerializeField]
    private float jumpMaxMuliplier;
    [SerializeField]
    private float doubleJumpWaitTime;

    #endregion

    #region Run-Time Fields

    private bool didDoubleJump = false;
    private bool jumpReleased = false;

    private int currentDirectionMovementMultiplier = 0;
    private int lastDirection = -1;

    private float currentmoveMultiplier = 0;
    private float strafeLeftTime = 0;
    private float strafeRightTime = 0;
    private float strafeUsedTime = 0;
    private float jumpedTime;

    #endregion

    #region Monobehaviors

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.main.IsGameRunning())
        {
            ProcessStrafing();
            ProcessJump();
            ProcessDirection();
        }
    }

    void FixedUpdate()
    {
        if (GameManager.main.IsGameRunning())
        {
            ProcessMovement();
        }
    }

    #endregion

    #region Public methods

    public void ResetDoubleJump()
    {
        didDoubleJump = false;
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
        else if (Input.GetKey(KeyCode.D))
        {
            MoveRight();
            currentDirectionMovementMultiplier = 1;
        }
        // If we are moving below a threshold then reset the moving multiplier
        else if (Mathf.Abs(player.GetRigidbody2D().velocity.x) < moveResetThreshold)
        {
            currentDirectionMovementMultiplier = 0;
        }
    }

    private void ProcessDirection()
    {
        player.SetDirectionFacing(lastDirection);
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
            player.GetRigidbody2D().AddForce(player.GetTransform().right * -strafeMultiplier);
            strafeUsedTime = Time.time;
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
            player.GetRigidbody2D().AddForce(player.GetTransform().right * strafeMultiplier);
            strafeUsedTime = Time.time;
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

        player.GetRigidbody2D().AddForce(player.GetTransform().right * -currentmoveMultiplier);
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
        player.GetRigidbody2D().AddForce(player.GetTransform().right * currentmoveMultiplier);
    }

    private void Jump()
    {
        if ((!player.GetOnGround() && didDoubleJump == true) 
            || (!player.GetOnGround() && jumpReleased == false)
            || Time.time - jumpedTime < doubleJumpWaitTime)
        {
            return;
        }

        if (!player.GetOnGround() && didDoubleJump == false)
        {
            didDoubleJump = true;
        }

        player.GetRigidbody2D().AddForce(player.GetTransform().up * jumpMaxMuliplier);
        jumpedTime = Time.time;
        jumpReleased = false;
    }

#endregion
}
