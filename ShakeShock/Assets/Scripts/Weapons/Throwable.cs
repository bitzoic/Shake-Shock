// --------------------------------------------------------------
// Shake Shock - Throwable                              3/06/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    #region enum

    public enum ThrowableType
    {
        basicApple,
        explosiveApple,
        electricApple
    };

    #endregion

    #region Inspector Fields

    [Header("Dependencies")]
    [SerializeField]
    private Rigidbody2D rigidbody2D;
    [SerializeField]
    private Collider2D throwableCollider;
    [SerializeField]
    private GameObject throwableGameObject;

    [Header("Basic Apple")]
    [SerializeField]
    private GameObject basicApple;
    [SerializeField]
    private BasicThrowableApple basicAppleScript;

    [Header("Explosive Apple")]
    [SerializeField]
    private GameObject explosiveApple;
    [SerializeField]
    private ExplosiveThrowableApple explosiveAppleScript;

    [Header("Electric Apple")]
    [SerializeField]
    private GameObject electricApple;
    [SerializeField]
    private ElectricThrowableApple electricAppleScript;

    [Header("Settings")]
    [SerializeField]
    private float timeToEnableCollider;

    #endregion

    #region Run-Time Fields

    private float throwForce;
    private Vector2 throwDirection;
    private ThrowableType throwableType;
    private Coroutine decay;

    #endregion


    #region Monobehaviors

    private void Awake()
    {
        throwableCollider.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        StartCoroutine(WaitToEnableCollider());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (throwableType)
        {
            case ThrowableType.basicApple:
                basicAppleScript.OnCollisionEnterThrowable(collision);
                break;
            case ThrowableType.explosiveApple:
                explosiveAppleScript.OnCollisionEnterThrowable(collision);
                break;
            case ThrowableType.electricApple:
                electricAppleScript.OnCollisionEnterThrowable(collision);
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        switch (throwableType)
        {
            case ThrowableType.basicApple:
                basicAppleScript.OnCollisionExitThrowable(collision);
                break;
            case ThrowableType.explosiveApple:
                explosiveAppleScript.OnCollisionExitThrowable(collision);
                break;
            case ThrowableType.electricApple:
                electricAppleScript.OnCollisionExitThrowable(collision);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (throwableType)
        {
            case ThrowableType.basicApple:
                basicAppleScript.OnTriggerEnterThrowable(collision);
                break;
            case ThrowableType.explosiveApple:
                explosiveAppleScript.OnTriggerEnterThrowable(collision);
                break;
            case ThrowableType.electricApple:
                electricAppleScript.OnTriggerEnterThrowable(collision);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (throwableType)
        {
            case ThrowableType.basicApple:
                basicAppleScript.OnTriggerExitThrowable(collision);
                break;
            case ThrowableType.explosiveApple:
                explosiveAppleScript.OnTriggerExitThrowable(collision);
                break;
            case ThrowableType.electricApple:
                electricAppleScript.OnTriggerExitThrowable(collision);
                break;
        }
    }

    #endregion

    #region Public Methods

    public void SetThrowForce(float force)
    {
        throwForce = force;
    }

    public void SetThrowDirection(Vector2 direction)
    {
        throwDirection = direction;
    }

    public void SetThrowableType(ThrowableType type)
    {
        throwableType = type;

        switch (throwableType)
        {
            case ThrowableType.basicApple:
                basicApple.SetActive(true);
                break;
            case ThrowableType.explosiveApple:
                explosiveApple.SetActive(true);
                break;
            case ThrowableType.electricApple:
                electricApple.SetActive(true);
                break;
            default:
                Debug.Log("ERROR: No throwable type defined");
                break;
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator WaitToEnableCollider()
    {
        yield return new WaitForSeconds(timeToEnableCollider);
        throwableCollider.enabled = true;
    }

    #endregion
}
