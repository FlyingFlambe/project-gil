using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public float camSpeed;

    void LateUpdate()
    {
        // Create vector for the camera to follow.
        Vector3 v3 = transform.position;

        // Calculate vector.
        v3.x = Mathf.Lerp(v3.x, target.position.x, camSpeed * Time.deltaTime);
        v3.y = Mathf.Lerp(v3.y, target.position.y, camSpeed * Time.deltaTime);

        // Change camera's position based on vector.
        transform.position = v3;
    }
}
