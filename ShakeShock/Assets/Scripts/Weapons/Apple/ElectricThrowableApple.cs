// --------------------------------------------------------------
// Shake Shock - ElectricThrowableApple                 3/06/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricThrowableApple : MonoBehaviour
{
    #region Inspector Fields

    [Header("Dependencies")]
    [SerializeField]
    private GameObject parentThrowable;
    [SerializeField]
    private GameObject explosionPrefab;

    [Header("Settings")]
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
        Instantiate(explosionPrefab, parentThrowable.transform.position, Quaternion.identity);
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

