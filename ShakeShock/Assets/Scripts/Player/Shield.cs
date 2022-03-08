using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Settings")]
    [SerializeField]
    private float offsetDistance;

    [Header("Lightning")]
    [SerializeField]
    private float rangeX;
    [SerializeField]
    private float rangeY;
    [SerializeField]
    private float randomness;
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
    private int currentChange;

    #endregion

    #region Monobehaviors

    // Start is called before the first frame update
    void Start()
    {
        centerPoint = shieldGameObject.transform.localPosition;
        shieldEnabled = false;
        shieldCollider.enabled = false;
        shieldSprite.enabled = false;

        foreach (LineRenderer line in lineRenderers)
        {
            line.positionCount = linePositions;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) && GameManager.main.IsGameRunning() && player.GetAllowInput())
        {
            ProcessInput();
        }
        else if (shieldEnabled == true)
        {
            HideShield();
            DisableShield();
            shieldEnabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
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

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        Vector2 shieldDirection = new Vector2(
            mousePosition.x - player.GetTransform().position.x,
            mousePosition.y - player.GetTransform().position.y
            );
        shieldDirection.Normalize();

        float xPoint = centerPoint.x + (shieldDirection.x * offsetDistance * -player.GetDirectionFacing());
        float yPoint = centerPoint.y + (shieldDirection.y * offsetDistance);
        float angle = Mathf.Atan2(-shieldDirection.y, -shieldDirection.x) * Mathf.Rad2Deg;

        MoveShield(shieldDirection, xPoint, yPoint, angle);

        if (currentChange == changeIntensity)
        {
            ChangeLines(shieldDirection, xPoint, yPoint);
            currentChange = 0;
        }
        else
        {
            currentChange++;
        }
    }

    private void ShowShield()
    {
        shieldSprite.enabled = true;
    }

    private void HideShield()
    {
        shieldSprite.enabled = false;
    }

    private void EnableShield()
    {
        shieldCollider.enabled = true;
    }

    private void DisableShield()
    {
        shieldCollider.enabled = false;
    }

    private void MoveShield(Vector2 shieldDirection, float xPoint, float yPoint, float angle)
    {
        shieldGameObject.transform.localPosition = new Vector2(xPoint, yPoint);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void ChangeLines(Vector2 shieldDirection, float xPoint, float yPoint)
    {
        Vector2 perp = Vector2.Perpendicular(shieldDirection);

        foreach (LineRenderer line in lineRenderers)
        {
            int change = Random.value > 0.5 ? 0 : 1;

            if (change == 1)
            {
                float[] percentages = new float[linePositions];
                float max = 0;
                percentages[0] = -1;

                for (int i = 1; i < linePositions; i++)
                {
                    percentages[i] = Random.Range(-1, 1);
                    if (percentages[i] > max)
                    {
                        max = percentages[i];
                    }
                }
                percentages[linePositions - 1] = 1;

                for (int i = 0; i < line.positionCount; i++)
                {
                    Vector2 pos = new Vector2(
                        xPoint + (percentages[i] * perp.x * rangeX  * Random.Range(-randomness, randomness)) + (offset * shieldDirection.x),
                        yPoint + (percentages[i] * perp.y * rangeY  * Random.Range(-randomness, randomness)) + (offset * shieldDirection.y)
                        );

                    line.SetPosition(i, pos);
                }
            }
        }
    }

    #endregion

}