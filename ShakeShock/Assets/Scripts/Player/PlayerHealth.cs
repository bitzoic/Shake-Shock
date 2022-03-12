// --------------------------------------------------------------
// Shake Shock - PlayerHealth                           3/10/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    #region Inspector Fields

    [Header("Dependencies")]
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Player player;
    [SerializeField]
    private Transform canvas;

    [Header("Settings")]
    [SerializeField]
    private float deafultMaxHealth;

    #endregion

    #region Run-Time Fields

    private float maxHealth;
    private float currentHealth;
    private float amour;
    private int lastDirection = -1;

    #endregion

    #region Monobehaviors

    // Start is called before the first frame update
    void Start()
    {
        if (maxHealth == 0)
        {
            currentHealth = deafultMaxHealth;
            maxHealth = deafultMaxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthSlider();

        if (player.GetDirectionFacing() != lastDirection)
        {
            lastDirection = player.GetDirectionFacing();
            FlipCanvas();
        }
    }

    #endregion

    #region Public Methods

    public void SetMaxHealth(float health)
    {
        maxHealth = health;
    }
    
    public void SetArmour(float armour)
    {
        this.amour = armour;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= (damage - (damage * amour));
        if (currentHealth < 0)
        {
            currentHealth = 0;
            PlayerDeath();
        }
    }

    #endregion

    #region Private Methods

    private void PlayerDeath()
    {
        GameManager.main.EndGame(player);
    }

    private void UpdateHealthSlider()
    {
        healthSlider.value = currentHealth / maxHealth;
    }

    private void FlipCanvas()
    {
        canvas.localScale = new Vector3(
            -canvas.localScale.x,
            canvas.localScale.y,
            canvas.localScale.z
            );
    }

    #endregion
}
