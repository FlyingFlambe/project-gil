using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCollision : MonoBehaviour {

    PlayerController playerController;

    public GameObject thisTrigger;

    /* // Raycasting Collision Detection (BROKEN) //
    RaycastOrigins raycastOrigins;
    public LayerMask collisionMask;
    public CollisionInfo collisions;

    const float skinWidth = .02f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;
    */

    void Start () {
        playerController = GetComponent<PlayerController>();
	}

    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Left Trigger
        if (other.gameObject.layer == 8 && thisTrigger.name == "LeftCollider")
            playerController.colLeft = true;
        else
            playerController.colLeft = false;

        // Right Trigger
        if (other.gameObject.layer == 8 && thisTrigger.name == "RightCollider")
            playerController.colRight = true;
        else
            playerController.colRight = false;

        // Above Trigger
        if (other.gameObject.layer == 8 && thisTrigger.name == "AboveCollider")
            playerController.colAbove = true;
        else
            playerController.colAbove = false;

        // Below Trigger
        if (other.gameObject.layer == 8 && thisTrigger.name == "BelowCollider")
            playerController.colBelow = true;
        else
            playerController.colBelow = false;
    }
}













    /* // Raycasting Collision Detection (BROKEN) //
    void Update()
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        CalculateRaySpacing();

        HorizontalCollisions();
        VerticalCollisions();
    }

    void HorizontalCollisions()
    {
        colLeft = collisions.left;
        colRight = collisions.right;

        float directionX = Mathf.Sign(playerController.transform.localScale.x);
        float rayLength = Mathf.Abs(playerController.transform.localScale.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.right * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
            //Debug.DrawRay(raycastOrigins.bottomLeft + Vector2.right * horizontalRaySpacing * i, Vector2.down * 2, Color.red);
            //Debug.DrawRay(raycastOrigins.topLeft + Vector2.right * horizontalRaySpacing * i, Vector2.up * 2, Color.red);

            if (hit)
            {
                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    void VerticalCollisions()
    {
        colBelow = collisions.below;
        colAbove = collisions.above;

        float directionY = Mathf.Sign(playerController.transform.localScale.y);
        float rayLength = Mathf.Abs(playerController.transform.localScale.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.up * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
            //Debug.DrawRay(raycastOrigins.bottomLeft + Vector2.up * verticalRaySpacing * i, Vector2.left * 2, Color.red);
            //Debug.DrawRay(raycastOrigins.bottomRight + Vector2.up * verticalRaySpacing * i, Vector2.right * 2, Color.red);

            if (hit)
            {
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = capsuleCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = capsuleCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.x / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.y / (verticalRayCount - 1);
    }

    struct RaycastOrigins {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }
    */
