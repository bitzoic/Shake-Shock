// --------------------------------------------------------------
// Shake Shock - PlayerFeet                             3/05/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeet : MonoBehaviour
{
    #region Inspector Fields

    [Header("Dependencies")]
    [SerializeField]
    private Player player;
    [SerializeField]
    private PlayerMovement playerMovement;

    #endregion

    #region Monobehaviors

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ground")
        {
            player.SetOnGround(true);
            playerMovement.ResetJump();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ground")
        {
            player.SetOnGround(false);
        }
    }

    #endregion
}
