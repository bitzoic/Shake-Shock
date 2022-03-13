// --------------------------------------------------------------
// Shake Shock - ExplosiveThrowableApple                3/06/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ExplosiveThrowableApple : MonoBehaviour
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

    private Coroutine explode;

    #endregion

    #region Monobehaviors

    // Start is called before the first frame update
    void Start()
    {
        explode = StartCoroutine(Explode());
    }

    #endregion

    #region Public Methods

    public void OnCollisionEnterThrowable(Collision2D collision)
    {

    }

    public void OnCollisionExitThrowable(Collision2D collision)
    {

    }

    public void OnTriggerEnterThrowable(Collider2D collision)
    {
       // if (collision.)
    }

    public void OnTriggerExitThrowable(Collider2D collision)
    {

    }

    #endregion

    #region Private Methods

    private void ExplodeApple()
    {
        GameObject explosion = PhotonNetwork.Instantiate("Explosion", parentThrowable.transform.position, Quaternion.identity);
        Explosion explosionScript = explosion.GetComponent<Explosion>();
        explosionScript.SetDamage(damage);
        explosionScript.SetPlayerGameobject(parentThrowableScript.GetThrowPlayer());

        Destroy(parentThrowable);
    }

    #endregion

    #region Coroutine

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(timeToExplode);
        ExplodeApple();
    }

    #endregion
}

