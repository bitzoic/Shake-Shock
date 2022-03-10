// --------------------------------------------------------------
// Shake Shock - BasicThrowableApple                    3/06/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicThrowableApple : MonoBehaviour
{
    #region Inspector Fields

    [Header("Dependencies")]
    [SerializeField]
    private GameObject parentThrowable;
    [SerializeField]
    private Throwable parentThrowableScript;

    [Header("Settings")]
    [SerializeField]
    private float damage;
    [SerializeField]
    private bool allowCollisions;
    [SerializeField]
    private float lifeTime;

    #endregion

    #region Run-Time Fields

    private Coroutine decay;

    #endregion

    #region Monobehaviors


    #endregion

    #region Public Methods

    public float GetLifeTime()
    {
        return lifeTime;
    }

    public void OnCollisionEnterThrowable(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && parentThrowableScript.GetThrowPlayer() != collision.gameObject)
        {
            collision.gameObject.GetComponent<Player>().GetPlayerHealth().TakeDamage(damage);
            Destroy(parentThrowable);
        }
    }

    public void OnCollisionExitThrowable(Collision2D collision)
    {

    }

    public void OnTriggerEnterThrowable(Collider2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            decay = StartCoroutine(Decay());
        }
    }

    public void OnTriggerExitThrowable(Collider2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            if (decay != null)
            {
                StopCoroutine(decay);
                decay = null;
            }
        }
    }

    #endregion

    #region Coroutine

    private IEnumerator Decay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(parentThrowable);
    }

    #endregion
}
