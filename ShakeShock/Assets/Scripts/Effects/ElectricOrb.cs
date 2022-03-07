// --------------------------------------------------------------
// Shake Shock - ElectricOrb                            3/06/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricOrb : MonoBehaviour
{
    #region Inspector Fields

    [Header("Dependencies")]
    [SerializeField]
    private GameObject electricOrbGameObject;
    [SerializeField]
    private LineRenderer[] lineRenderers;
    [SerializeField]
    private Transform[] shockWaves;

    [Header("Lightning")]
    [SerializeField]
    private float radius;
    [SerializeField]
    private float randomness;
    [SerializeField]
    private int linePositions;
    [SerializeField]
    private float changeIntensity;

    [Header("Shock Waves")]
    [SerializeField]
    private float maxRadius;
    [SerializeField]
    private float decreaseRate;

    [Header("Settings")]
    [SerializeField]
    private float lifeTime;

    #endregion

    #region Run-Time Fields

    private bool allowChange;
    private int currentChange;

    #endregion

    #region Monobehaviors

    // Start is called before the first frame update
    void Start()
    {
        foreach (LineRenderer line in lineRenderers)
        {
            line.positionCount = linePositions;
        }

        StartCoroutine(Decay());
    }

    // Update is called once per frame
    void Update()
    {
        if (currentChange == changeIntensity)
        {
            ChangeLines();
            currentChange = 0;
        }
        else
        {
            currentChange++;
        }

        MoveShockWaves();
    }

    #endregion

    #region Private Methods

    private void ChangeLines()
    {
        foreach (LineRenderer line in lineRenderers)
        {
            int change = Random.value > 0.5 ? 0 : 1;

            if (change == 1)
            {
                Vector2 dir = Random.insideUnitCircle;
                dir.Normalize();

                float[] percentages = new float[linePositions];
                float max = 0;
                percentages[0] = 0;

                for (int i = 1; i < linePositions; i++)
                {
                    percentages[i] = Random.Range(max, radius);
                    if (percentages[i] > max)
                    {
                        max = percentages[i];
                    }
                }
                percentages[linePositions - 1] = radius;

                line.SetPosition(0, new Vector2(0, 0));

                for (int i = 1; i < line.positionCount; i++)
                {
                    Vector2 pos = new Vector2(
                        line.GetPosition(i -1).x + ((dir.x * percentages[i]) + Random.Range(-randomness, randomness)),
                        line.GetPosition(i - 1).y + ((dir.y * percentages[i]) + Random.Range(-randomness, randomness))
                        );

                    

                    line.SetPosition(i, pos);
                }
            }
        }
    }

    private void MoveShockWaves()
    {
        foreach (Transform shockWave in shockWaves)
        {
            shockWave.localScale -= new Vector3(decreaseRate, decreaseRate, 0);
            if (shockWave.localScale.x < 0)
            {
                shockWave.localScale = new Vector3(maxRadius, maxRadius, 0);
            }
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator Decay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(electricOrbGameObject);
    }

    #endregion

}
