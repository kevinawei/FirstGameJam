using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RopeSystem : MonoBehaviour
{
public LineRenderer ropeRenderer;

public Transform crosshair;
public SpriteRenderer crosshairSprite;
public PlayerMovement playerMovement;
private bool ropeAttached;
private Vector2 playerPosition;
public LayerMask ropeLayerMask;
private float ropeMaxCastDistance = 10f;
private bool distanceSet;
private bool isColliding;

public float boost;
private RaycastHit2D hit;

public DistanceJoint2D ropeJoint;
private List<Vector2> ropePositions = new List<Vector2>();

public Rigidbody2D rb;

public GameObject ropeHingeAnchor;
private Rigidbody2D ropeHingeAnchorRb;
private SpriteRenderer ropeHingeAnchorSprite;

public Vector2 minPower;
public Vector2 maxPower;


void Awake(){
     ropeJoint.enabled = false;
    playerPosition = transform.position;
    ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
    ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
}

void Update()
{
    var worldMousePosition =
        Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
    var facingDirection = worldMousePosition - transform.position;
    var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
    if (aimAngle < 0f)
    {
        aimAngle = Mathf.PI * 2 + aimAngle;
    }

    var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
    
    playerPosition = transform.position;

     if (!ropeAttached){
	    SetCrosshairPosition(aimAngle);
    }
    else {
	    crosshairSprite.enabled = false;
    }
    HandleInput(aimDirection);
}
private void SetCrosshairPosition(float aimAngle)
{
    if (!crosshairSprite.enabled)
    {
        crosshairSprite.enabled = true;
    }

    var x = transform.position.x + 1f * Mathf.Cos(aimAngle);
    var y = transform.position.y + 1f * Mathf.Sin(aimAngle);

    var crossHairPosition = new Vector3(x, y, 0);
    crosshair.transform.position = crossHairPosition;
}

private void HandleInput(Vector2 aimDirection)
{
    if (Input.GetMouseButton(0))
    {
        hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);
        

        if (hit.collider != null)
        {
            ropeAttached = true;
            var v2 = new Vector2(Mathf.Clamp(hit.point.x - transform.position.x, minPower.x, maxPower.x) , Mathf.Clamp(hit.point.y - transform.position.y, minPower.y, maxPower.y)); 
            Debug.Log(hit.point.y - transform.position.y);
            if (v2.y < 0){
                rb.gravityScale = 0f;
            }
           rb.AddRelativeForce(v2, ForceMode2D.Impulse);
            ropeHingeAnchorSprite.enabled = true;
            
        }
        else
        {           
            ropeRenderer.enabled = false;
             ropeAttached = false;
             ropeJoint.enabled = false;
        }
    }

    if (Input.GetMouseButtonUp(0))
    {
        crosshairSprite.enabled = true;
        if ((hit.point.y - transform.position.y) > 0){
            rb.AddForce(Vector2.up * boost, ForceMode2D.Impulse);
        }

        ResetRope();
    }
}


private void ResetRope()
{
     ropeJoint.enabled = false;
    ropeAttached = false;
    ropeRenderer.positionCount = 2;
    ropeRenderer.SetPosition(0, transform.position);
    ropeRenderer.SetPosition(1, transform.position);
    ropePositions.Clear();
    ropeHingeAnchorSprite.enabled = false;
    rb.gravityScale = 2.5f;
}


void OnTriggerStay2D(Collider2D colliderStay)
{
    isColliding = true;
}

private void OnTriggerExit2D(Collider2D colliderOnExit)
{
    isColliding = false;
}

}
