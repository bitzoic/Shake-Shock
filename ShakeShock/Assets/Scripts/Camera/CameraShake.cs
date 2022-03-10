// --------------------------------------------------------------
// Shake Shock - CameraShake                            3/05/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    #region Static Fields

    public static CameraShake main;

    #endregion

    #region InspectorFields

    [Header("Dependencies")]
    [SerializeField]
    private Transform cameraTransform;

    [Header("Settings")]
    [SerializeField]
    private float distortion;
    [SerializeField]
    private bool xDirection;
    [SerializeField]
    private bool yDirection;
    [SerializeField]
    private bool zDirection;
    [SerializeField]
    private bool wDirection;

    [Header("Debug")]
    [SerializeField]
    private bool debugTest;
    [SerializeField]
    private float debugIntensity;
    [SerializeField]
    private float debugLength;

    #endregion

    #region Run-Time Fields

    private Quaternion originRotation;
    private bool fixPos = false;
    private float shake_intensity = 0;
    private float shake_decay = 0;

    #endregion

    #region Monobehaviors

    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShakeCamera();

        if (debugTest)
        {
            debugTest = false;
            ShakeCamera(debugIntensity, debugLength);
        }
    }

    #endregion

    #region Public Methods

    public void ShakeCamera(float intensity, float length)
    {
        originRotation = Quaternion.Euler(0, 0, 0);
        shake_intensity = intensity;
        shake_decay = length;
    }

    #endregion

    #region Private Methods

    private void ShakeCamera()
    {
        if (shake_intensity > 0)
        {
            cameraTransform.rotation = new Quaternion(
            originRotation.x + (Random.Range(-shake_intensity, shake_intensity) * distortion) * (xDirection == true ? 1 : 0),
            originRotation.y + (Random.Range(-shake_intensity, shake_intensity) * distortion) * (yDirection == true ? 1 : 0),
            originRotation.z + (Random.Range(-shake_intensity, shake_intensity) * distortion) * (zDirection == true ? 1 : 0),
            originRotation.w + (Random.Range(-shake_intensity, shake_intensity) * distortion) * (wDirection == true ? 1 : 0));

            shake_intensity -= shake_decay;
            fixPos = true;
        }
        else
        {
            if (fixPos == true)
            {
                cameraTransform.rotation = Quaternion.Euler(0, 0, 0);
                fixPos = false;
            }
        }
    }

    #endregion
}
