using UnityEngine;

public class FollowMouseX : MonoBehaviour
{
    [Header("Following Settings")] 
    [SerializeField] 
    private float maxSpeed = 400f;
    [SerializeField]
    private float bufferZoneFromMouse = 20f; 
    [SerializeField]
	private float rampUpDistance = 100f; 
    
    [Header("Sprite to Flip")] 
    [SerializeField] 
    private SpriteRenderer spriteRend;

    private Camera cam;
    private Vector3 spritePosition; 
    private Vector3 mouseInWorldSpacePosition;

	private const float LEFT_EDGE = -300f;
	private const float RIGHT_EDGE = 300f; 

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Get mouse x position in world space 
		Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition); 
		float mouseX = mouseWorldPos.x;
        
        // Get current sprite position
        spritePosition = transform.position;
        
        // Handle sprite flipping based on direction to mouse
        if (spriteRend != null)
        {	
			spriteRend.flipX = mouseX < spritePosition.x;
        }
        
        // Calculate distance to mouse on X-axis only
        float currentDistanceToMouseX = Mathf.Abs(spritePosition.x - mouseX);
        
        // ONLY move if distance exceeds threshold
        if (currentDistanceToMouseX > bufferZoneFromMouse) 
		{
        	// Calculate acceleration based on distance - further = faster, closer = slower 
			float t = (currentDistanceToMouseX - bufferZoneFromMouse) / rampUpDistance;
			t = Mathf.Clamp01(t);
			float currentSpeed = Mathf.Lerp(0, maxSpeed, t) * Time.deltaTime;
            // Move towards the exact mouse X position using MoveTowards, keeping Y and Z unchanged
            float desiredX = Mathf.MoveTowards(spritePosition.x, mouseX, currentSpeed);
			float newX = Mathf.Clamp(desiredX, LEFT_EDGE, RIGHT_EDGE);
            transform.position = new Vector3(newX, spritePosition.y, spritePosition.z);
        }
    }
}