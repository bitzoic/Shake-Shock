// --------------------------------------------------------------
// Shake Shock - Throwable                              3/06/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerThrow : MonoBehaviour
{
    #region Inspector Fields

    [Header("Dependencies")]
    [SerializeField]
    private GameObject throwablePrefab;
    [SerializeField]
    private GameObject playerThrowableGameObject;
    [SerializeField]
    private Player player;
    [SerializeField]
    private Camera mainCamera;

    [Header("Settings")]
    [SerializeField]
    private float throwForce;
    [SerializeField]
    private Throwable.ThrowableType type;

    #endregion


    #region Monobehaviors

    // Update is called once per frame
    void Update()
    {
        if (GameManager.main.IsGameRunning() && player.GetAllowInput())
        {
            ProcessInput();
        }
    }

    #endregion

    #region Public Methods

    public void SetThrowableType(Throwable.ThrowableType newType)
    {
        type = newType;
    }

    #endregion

    #region Private Methdods

    private void ProcessInput()
    {
        if (Input.GetMouseButtonDown(0) && (
            !GameManager.main.GetMultiplayerMode()
            || (player.GetPhotonView().IsMine && GameManager.main.GetMultiplayerMode())))
        {
            if (mainCamera == null)
            {
                mainCamera = player.GetCamera();
            }

            Vector2 clickPoint = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
            Vector2 throwDirection = new Vector2(
                clickPoint.x - player.GetTransform().position.x, 
                clickPoint.y - player.GetTransform().position.y
                );
            throwDirection.Normalize();

            Vector2 spawnPoint = new Vector2(
                playerThrowableGameObject.transform.position.x + (1 * throwDirection.x), 
                playerThrowableGameObject.transform.position.y + (1 * throwDirection.y)
                );

            GameObject throwable;
            if (GameManager.main.GetMultiplayerMode())
            {
                throwable = PhotonNetwork.Instantiate("Throwable", spawnPoint, Quaternion.identity);
            }
            else
            {
                throwable = Instantiate(Resources.Load("Throwable"), spawnPoint, Quaternion.identity) as GameObject;
            }
            Throwable throwableScript = throwable.GetComponent<Throwable>();

            throwableScript.SetThrowableType(type);
            throwableScript.SetThrowForce(throwForce);
            throwableScript.SetThrowDirection(throwDirection);
            throwableScript.SetThrowPlayer(player.GetGameObject());
        }
    }

    #endregion
}
