// --------------------------------------------------------------
// Shake Shock - CameraFollow                           3/05/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Inspector Fields

    [Header("Dependencies")]
    [SerializeField]
    private Camera mainCamera;

    [Header("Settings")]
    [SerializeField]
    private float dampTime;
    [SerializeField]
    private float screenEdgeBuffer;
    [SerializeField]
    private float minCameraSize;

    #endregion


    #region Run-Time Fields

    private List<Transform> targets = new List<Transform>();
    private Vector3 moveVelocity;
    private float zoomSpeed;
    private Vector3 desiredPosition;

    #endregion

    #region Monobehaviors

    private void FixedUpdate()
    {
        Move();
        Zoom();
    }

    #endregion

    #region Public Methods

    public void AddToTargetList(Transform newTarget)
    {
        targets.Add(newTarget);
    }

    #endregion

    #region Private Methods

    private void Move()
    {
        FindAveragePosition();

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime);
    }


    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        // Go through all the targets and add their positions together.
        foreach (Transform target in targets)
        {
            if (!target.gameObject.activeSelf)
                continue;

            averagePos += target.position;
            numTargets++;
        }

        if (numTargets > 0)
            averagePos /= numTargets;

        // Keep the same y value.
        averagePos.z = transform.position.z;

        // The desired position is the average position;
        desiredPosition = averagePos;
    }


    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, requiredSize, ref zoomSpeed, dampTime);
    }


    private float FindRequiredSize()
    {
        // Find the position the camera rig is moving towards in its local space.
        Vector3 desiredLocalPos = transform.InverseTransformPoint(desiredPosition);
        float size = 0f;

        foreach (Transform target in targets)
        {
            if (target.gameObject.activeSelf)
                continue;

            // find the position of the target in the camera's local space.
            Vector3 targetLocalPos = transform.InverseTransformPoint(target.position);

            // Find the position of the target from the desired position of the camera's local space.
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            // Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

            // Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / mainCamera.aspect);
        }

        // Add the edge buffer to the size.
        size += screenEdgeBuffer;
        size = Mathf.Max(size, minCameraSize);

        return size;
    }

    #endregion
}
