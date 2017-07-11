using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCollision : MonoBehaviour {

    // Input
    public Transform leftCheck;
    public Transform rightCheck;
    public Transform belowCheck;
    public Transform aboveCheck;

    public Vector2 tallBoxSize;                     // Size of left/right collision boxes.
    public Vector2 wideBoxSize;                     // Size of below/above collision boxes.

    private float angle = 0;                        // Rotates box. Leave this at 0 degrees.
    public LayerMask sideDetect;                    // LayerMask should collide with every entity for wide use. Implement a check in that entity's specific script for desired outcome.

    // Output
    public bool leftCollision;
    public bool rightCollision;
    public bool belowCollision;
    public bool aboveCollision;

    void Start()
    {
        tallBoxSize = new Vector2(0.1f, 1.3f);
        wideBoxSize = new Vector2(0.5f, 0.1f);
    }

    void Update () {

        leftCollision = Physics2D.OverlapBox(leftCheck.position, tallBoxSize, angle, sideDetect);
        rightCollision = Physics2D.OverlapBox(rightCheck.position, tallBoxSize, angle, sideDetect);
        belowCollision = Physics2D.OverlapBox(belowCheck.position, wideBoxSize, angle, sideDetect);
        aboveCollision = Physics2D.OverlapBox(aboveCheck.position, wideBoxSize, angle, sideDetect);

    }
}
