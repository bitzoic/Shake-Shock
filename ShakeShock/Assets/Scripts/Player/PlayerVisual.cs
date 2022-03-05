// --------------------------------------------------------------
// Shake Shock - PlayerVisual                           3/05/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    #region Inspector Fields

    [Header("Dependencies")]
    [SerializeField]
    private Player player;
    [SerializeField]
    private SpriteRenderer playerSprite;

    #endregion

    #region Run-Time Fields

    private int lastDirection = -1;

    #endregion

    #region Monobehaviors

    // Update is called once per frame
    void Update()
    {
        if (lastDirection != player.GetDirectionFacing())
        {
            ProcessDirection();
            lastDirection = player.GetDirectionFacing();
        }
    }

    #endregion

    #region Private Methods

    private void ProcessDirection()
    {
        player.GetTransform().localScale = new Vector3(-player.GetTransform().localScale.x, player.GetTransform().localScale.y, player.GetTransform().localScale.z);
    }

    #endregion
}
