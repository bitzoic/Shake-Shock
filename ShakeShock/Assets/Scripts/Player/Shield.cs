// --------------------------------------------------------------
// Shake Shock - Shield                                 3/07/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    #region Inspector Fields

    [Header("Dependencies")]
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject shieldGameObject;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private SpriteRenderer shieldSprite;
    [SerializeField]
    private Collider2D shieldCollider;
    [SerializeField]
    private LineRenderer[] lineRenderers;
    [SerializeField]
    private GameObject coneGameObject;
    [SerializeField]
    private SpriteRenderer coneSprite;
    [SerializeField]
    private GameObject[] shieldWaveGameObjects;
    [SerializeField]
    private ParticleSystem shieldParticles;
    [SerializeField]
    private Slider shieldSlider;

    [Header("Settings")]
    [SerializeField]
    private float offsetDistance;
    [SerializeField]
    private int sheildParticlesRate;
    [SerializeField]
    private float shakeIntensity;
    [SerializeField]
    private float shakeDecay;

    [Header("Slider")]
    [SerializeField]
    private float maxShield;
    [SerializeField]
    private float shieldDecreaseRate;
    [SerializeField]
    private float shieldIncreaseRate;
    [SerializeField]
    private float shieldDisabletime;

    [Header("Waves")]
    [SerializeField]
    private float expansionRate;
    [SerializeField]
    private float maxSize;

    [Header("Lightning")]
    [SerializeField]
    private float range;
    [SerializeField]
    private float randomness;
    [SerializeField]
    private float angleRandomness;
    [SerializeField]
    private float offset;
    [SerializeField]
    private int linePositions;
    [SerializeField]
    private float changeIntensity;

    #endregion

    #region Run-Time Fields

    private Vector2 centerPoint;
    private bool shieldEnabled;
    private bool shieldBroken;
    private int lastDirection;
    private int currentChange;
    private float currentShield;

    #endregion

    #region Monobehaviors

    // Start is called before the first frame update
    void Start()
    {
        centerPoint = shieldGameObject.transform.localPosition;
        shieldEnabled = false;
        shieldCollider.enabled = false;
        shieldSprite.enabled = false;
        coneSprite.enabled = false;
        shieldParticles.emissionRate = 0;
        lastDirection = 1;
        currentShield = maxShield;
        shieldBroken = false;

        foreach (GameObject wave in shieldWaveGameObjects)
        {
            wave.SetActive(false);
        }

        foreach (LineRenderer line in lineRenderers)
        {
            line.positionCount = linePositions;
            line.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) && GameManager.main.IsGameRunning() && player.GetAllowInput() && currentShield > 0)
        {
            ProcessInput();
        }
        else if (shieldEnabled == true)
        {
            HideShield();
            DisableShield();
            shieldEnabled = false;
        }
        else
        {
            if (!shieldBroken)
            {
                IncreaseShield();
            }
        }

        UpdateSlider();
    }

    #endregion

    #region Public Methods

    public void SetShieldTime(float val)
    {
        maxShield = val;
    }

    #endregion

    #region Private Methods

    private void ProcessInput()
    {
        if (shieldEnabled == false)
        {
            ShowShield();
            EnableShield();
            shieldEnabled = true;
        }

        if (mainCamera == null)
        {
            mainCamera = player.GetCamera();
        }

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        Vector2 shieldDirection = new Vector2(
            mousePosition.x - player.GetTransform().position.x,
            mousePosition.y - player.GetTransform().position.y
            );
        shieldDirection.Normalize();

        float xPoint = centerPoint.x + (shieldDirection.x * offsetDistance * -player.GetDirectionFacing());
        float xPointUniDir = centerPoint.x + (shieldDirection.x * offsetDistance);
        float yPoint = centerPoint.y + (shieldDirection.y * offsetDistance);
        float angle = Mathf.Atan2(-shieldDirection.y, -shieldDirection.x) * Mathf.Rad2Deg;
        bool forceChange = false;

        MoveShield(shieldDirection, xPoint, yPoint, angle);
        ExpandWaves();
        DecreaseShield();

        CameraShake.main.ShakeCamera(shakeIntensity, shakeDecay);

        if (player.GetDirectionFacing() != lastDirection)
        {
            lastDirection = player.GetDirectionFacing();
            forceChange = true;
            FlipCone();
        }

        if (currentChange == changeIntensity || forceChange == true)
        {
            ChangeLines(shieldDirection, xPointUniDir, yPoint, forceChange);
            forceChange = false;
            currentChange = 0;
        }
        else
        {
            currentChange++;
        }
    }

    private void DecreaseShield()
    {
        currentShield -= Time.deltaTime * shieldDecreaseRate;
        if (currentShield <= 0)
        {
            StartCoroutine(ShieldBroken());
            shieldBroken = true;
            currentShield = 0;
        }
    }

    private void IncreaseShield()
    {
        currentShield += Time.deltaTime * shieldIncreaseRate;
        if (currentShield >= maxShield)
        {
            currentShield = maxShield;
        }
    }

    private void UpdateSlider()
    {
        shieldSlider.value = currentShield / maxShield;
    }

    private void ShowShield()
    {
        //shieldSprite.enabled = true;
        coneSprite.enabled = true;
        foreach (LineRenderer line in lineRenderers)
        {
            line.enabled = true;
        }

        foreach (GameObject wave in shieldWaveGameObjects)
        {
            wave.SetActive(true);
        }

        shieldParticles.emissionRate = sheildParticlesRate;
    }

    private void HideShield()
    {
        shieldSprite.enabled = false;
        coneSprite.enabled = false;
        foreach (LineRenderer line in lineRenderers)
        {
            line.enabled = false;
        }

        foreach (GameObject wave in shieldWaveGameObjects)
        {
            wave.SetActive(false);
        }

        shieldParticles.emissionRate = 0;
    }

    private void ExpandWaves()
    {
        foreach (GameObject wave in shieldWaveGameObjects)
        {
            wave.transform.localScale += new Vector3(expansionRate, expansionRate, 0);
            if (wave.transform.localScale.x > maxSize)
            {
                wave.transform.localScale = new Vector3(0, 0, 0);
            }
        }
    }

    private void EnableShield()
    {
        shieldCollider.enabled = true;
    }

    private void DisableShield()
    {
        shieldCollider.enabled = false;
    }

    private void FlipCone()
    {
        coneGameObject.transform.localScale = new Vector3(
            -coneGameObject.transform.localScale.x,
            coneGameObject.transform.localScale.y,
            coneGameObject.transform.localScale.z
        );
    }

    private void MoveShield(Vector2 shieldDirection, float xPoint, float yPoint, float angle)
    {
        shieldGameObject.transform.localPosition = new Vector2(xPoint, yPoint);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        coneGameObject.transform.localPosition = new Vector2(xPoint, yPoint);
        coneGameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void ChangeLines(Vector2 shieldDirection, float xPoint, float yPoint, bool forceChange)
    {
        Vector2 perp = Vector2.Perpendicular(shieldDirection);
        foreach (LineRenderer line in lineRenderers)
        {
            int change = Random.value > 0.5 ? 0 : 1;

            if (forceChange)
            {
                change = 1;
            }

            if (change == 1)
            {
                Vector2 newDir = new Vector2(
                    shieldDirection.x + (Random.Range(-angleRandomness, angleRandomness) * perp.x),
                    shieldDirection.y + (Random.Range(-angleRandomness, angleRandomness) * perp.y)
                    );
                newDir.Normalize();

                float[] percentages = new float[linePositions];
                float max = 0;
                percentages[0] = 0;

                for (int i = 1; i < linePositions; i++)
                {
                    percentages[i] = Random.Range(max, range);
                    if (percentages[i] > max)
                    {
                        max = percentages[i];
                    }
                }
                percentages[linePositions - 1] = range;

                line.SetPosition(
                    0,
                    new Vector2(
                        (xPoint + (offset * shieldDirection.x)) * -player.GetDirectionFacing(),
                        (yPoint + (offset * shieldDirection.y))
                        )
                    );

                for (int i = 1; i < line.positionCount - 1; i++)
                {
                    Vector2 pos = new Vector2(
                        (xPoint + (newDir.x * percentages[i]) + (Random.Range(-randomness, randomness) * perp.x * range) + (offset * shieldDirection.x)) * -player.GetDirectionFacing(),
                        (yPoint + (newDir.y * percentages[i]) + (Random.Range(-randomness, randomness) * perp.y * range) + (offset * shieldDirection.y))
                        );

                    line.SetPosition(i, pos);
                }

                line.SetPosition(
                    line.positionCount - 1,
                    new Vector2(
                        (xPoint + (offset * shieldDirection.x) + (range * newDir.x)) * -player.GetDirectionFacing(),
                        (yPoint + (offset * shieldDirection.y) + (range * newDir.y))
                        )
                    );
            }
        }
    }

    #endregion

    #region Coroutine

    private IEnumerator ShieldBroken()
    {
        yield return new WaitForSeconds(shieldDisabletime);
        shieldBroken = false;
    }

    #endregion
}