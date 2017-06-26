using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject target;
    public float followAhead;
    public float cameraSmooth;

    private Vector3 targetPos;

    void LateUpdate()
    {

        targetPos = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);

        // Shift camera target slightly depending on where the player is looking.
        if (target.transform.localScale.x > 0f)
        {
            targetPos = new Vector3(targetPos.x + followAhead, targetPos.y, targetPos.z);
        }
        else
        {
            targetPos = new Vector3(targetPos.x - followAhead, targetPos.y, targetPos.z);
        }

        // Smooth the camera's movement.
        transform.position = Vector3.Lerp(transform.position, targetPos, cameraSmooth * Time.deltaTime);

    }
}
