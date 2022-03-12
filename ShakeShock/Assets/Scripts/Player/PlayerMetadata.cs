// --------------------------------------------------------------
// Shake Shock - PlayerMetadata                         3/12/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMetadata : MonoBehaviour
{

    #region Run-Time Fields

    private float speedMultiplier;
    private float dashMultiplier;
    private float jumpMutiplier;
    private float shieldTime;
    private float health;
    private int armor;
    private Throwable.ThrowableType throwable;
    private Sprite playerSprite;
    private string wallet;
    private float tokens;

    #endregion

    #region Public Methods

    public void SetSpeedMuliplier(float speed)
    {
        speedMultiplier = speed;
    }

    public void SetDashMultiplier(float dash)
    {
        dashMultiplier = dash;
    }

    public void SetJumpMultiplier(float jump)
    {
        jumpMutiplier = jump;
    }

    public void SetShieldTime(float time)
    {
        shieldTime = time;
    }

    public void SetHealth(float health)
    {
        this.health = health;
    }

    public void SetArmor(int armor)
    {
        this.armor = armor;
    }

    public void SetThrowableType(Throwable.ThrowableType type)
    {
        throwable = type;
    }

    public void SetPlayerSprite(Sprite sprite)
    {
        playerSprite = sprite;
    }

    public void SetWallet(string wallet)
    {
        this.wallet = wallet;
    }

    public void SetTokens(float tokens)
    {
        this.tokens = tokens;
    }

    public float GetSpeedMultiplier()
    {
        return speedMultiplier;
    }

    public float GetDashMultiplier()
    {
        return dashMultiplier;
    }

    public float GetJumpMultiplier()
    {
        return jumpMutiplier;
    }

    public float GetShieldTime()
    {
        return shieldTime;
    }

    public float GetHealth()
    {
        return health;
    }

    public int GetArmor()
    {
        return armor;
    }

    public Throwable.ThrowableType GetThrowableType()
    {
        return throwable;
    }

    public Sprite GetPlayerSprite()
    {
        return playerSprite;
    }

    public string GetWallet()
    {
        return wallet;
    }

    public float GetTokens()
    {
        return tokens;
    }

    #endregion
}
