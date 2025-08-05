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

    private Camera _cam;
    private Vector3 _currentPosition; 
    private Vector3 _mouseInWorldSpacePosition;

	private const float LEFT_EDGE = -300f;
	private const float RIGHT_EDGE = 300f; 

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        // Get mouse x position in world space 
		Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition); 
		float mouseX = mouseWorldPos.x;
        
        // Get current sprite position
        _currentPosition = transform.position;
        
        // Handle sprite flipping based on direction to mouse
        if (spriteRend)
        {	
			spriteRend.flipX = mouseX < _currentPosition.x;
        }
        
        // Calculate distance to mouse on X-axis only
        float currentDistanceToMouseX = Mathf.Abs(_currentPosition.x - mouseX);
        
        // ONLY move if distance exceeds threshold
        if (currentDistanceToMouseX > bufferZoneFromMouse) 
		{
        	// Calculate acceleration based on distance - further = faster, closer = slower 
			float t = (currentDistanceToMouseX - bufferZoneFromMouse) / rampUpDistance;
			t = Mathf.Clamp01(t);
			float speed = Mathf.Lerp(0, maxSpeed, t) * Time.deltaTime;
            // Move towards the exact mouse X position using MoveTowards, keeping Y and Z unchanged
            float desiredX = Mathf.MoveTowards(_currentPosition.x, mouseX, speed);
			float newX = Mathf.Clamp(desiredX, LEFT_EDGE, RIGHT_EDGE);
            transform.position = new Vector3(newX, _currentPosition.y, _currentPosition.z);
		}
    }
}