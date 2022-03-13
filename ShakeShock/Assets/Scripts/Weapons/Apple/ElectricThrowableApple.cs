// --------------------------------------------------------------
// Shake Shock - ElectricThrowableApple                 3/06/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ElectricThrowableApple : MonoBehaviour
{
    #region Inspector Fields

    [Header("Dependencies")]
    [SerializeField]
    private GameObject parentThrowable;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private Throwable parentThrowableScript;

    [Header("Settings")]
    [SerializeField]
    private float damage;
    [SerializeField]
    private bool allowCollisions;
    [SerializeField]
    private float timeToExplode;

    #endregion

    #region Run-Time Fields

    private Coroutine decay;

    #endregion

    #region Monobehaviors

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Decay());
    }

    #endregion

    #region Public Methods

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
       
    }

    public void OnTriggerExitThrowable(Collider2D collision)
    {

    }

    #endregion

    #region Private Methods

    private void ExplodeApple()
    {
        if (GameManager.main.GetMultiplayerMode())
        {
            PhotonNetwork.Instantiate("ElectricOrb", parentThrowable.transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(explosionPrefab, parentThrowable.transform.position, Quaternion.identity);
        }
        Destroy(parentThrowable);
    }

    #endregion

    #region Coroutine

    private IEnumerator Decay()
    {
        yield return new WaitForSeconds(timeToExplode);
        ExplodeApple();
    }

    #endregion
}

