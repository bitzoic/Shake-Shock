// --------------------------------------------------------------
// Shake Shock - ExplosionDebug                         3/06/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ExplosionDebug : MonoBehaviour
{

    #region Inspector Fields

    [Header("Dependencies")]
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private GameObject debugGameObject;
    [SerializeField]
    private Camera mainCamera;

    [Header("Settings")]
    [SerializeField]
    private bool enabled;

    #endregion

    private void Start()
    {
        if (!enabled)
        {
            Destroy(debugGameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PhotonNetwork.Instantiate("Explosion", mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1)), Quaternion.identity);
        }
    }
}
