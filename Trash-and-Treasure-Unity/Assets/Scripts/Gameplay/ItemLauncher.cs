using UnityEngine;

public class ItemLauncher : MonoBehaviour
{
    [Header("Launch Settings")]
    public float maxForce = 500f;
    public float maxPullDistance = 3f;
    
    [Header("Visual Feedback")]
    public LineRenderer trajectoryLine;
    public GameObject aimIndicator;
    
    private Rigidbody2D rb2d;
    private Camera cam;
    private bool isDragging = false;
    private Vector3 startPosition;
    private Vector3 startMouseWorldPos;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            Debug.LogError("Rigidbody2D component required! Adding one automatically.");
            rb2d = gameObject.AddComponent<Rigidbody2D>();
        }
        
        cam = Camera.main;
        
        // Create trajectory line if not assigned
        if (trajectoryLine == null)
        {
            GameObject lineObj = new GameObject("TrajectoryLine");
            trajectoryLine = lineObj.AddComponent<LineRenderer>();
            trajectoryLine.material = new Material(Shader.Find("Sprites/Default"));
            trajectoryLine.startColor = Color.red;
            trajectoryLine.endColor = Color.red;
            trajectoryLine.startWidth = 0.1f;
            trajectoryLine.endWidth = 0.1f;
            trajectoryLine.positionCount = 2;
            trajectoryLine.enabled = false;
            trajectoryLine.useWorldSpace = true;
        }
        
        // Create aim indicator if not assigned
        if (aimIndicator == null)
        {
            aimIndicator = new GameObject("AimIndicator");
            SpriteRenderer sr = aimIndicator.AddComponent<SpriteRenderer>();
            
            // Create a simple circle texture
            Texture2D texture = new Texture2D(32, 32);
            Color[] colors = new Color[32 * 32];
            Vector2 center = new Vector2(16, 16);
            
            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), center);
                    colors[y * 32 + x] = distance < 12 ? Color.yellow : Color.clear;
                }
            }
            
            texture.SetPixels(colors);
            texture.Apply();
            
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 100);
            sr.sprite = sprite;
            aimIndicator.transform.localScale = Vector3.one * 0.3f;
            aimIndicator.SetActive(false);
        }
    }
    
    void Update()
    {
        HandleMouseInput();
        
        if (isDragging)
        {
            Debug.Log("Dragging sprite");
            UpdateVisualFeedback();
        }
    }
    
    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0; // Ensure we're working in 2D
            
            // Check if mouse is over this sprite using 2D collision
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null && collider.OverlapPoint(mouseWorldPos))
            {
                StartDrag();
            }
            else
            {
                // Fallback: check distance if no collider
                float distance = Vector2.Distance(transform.position, mouseWorldPos);
                if (distance < 1f) // Adjust this value based on your sprite size
                {
                    StartDrag();
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            Launch();
        }
    }
    
    void StartDrag()
    {
        isDragging = true;
        startPosition = transform.position;
        startMouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        startMouseWorldPos.z = 0;
        
        // Stop any existing movement
        rb2d.linearVelocity = Vector2.zero;
        rb2d.angularVelocity = 0f;
        
        // Enable visual feedback
        trajectoryLine.enabled = true;
        aimIndicator.SetActive(true);
        
        Debug.Log("Started dragging sprite");
    }
    
    void UpdateVisualFeedback()
    {
        Vector3 currentMouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        currentMouseWorldPos.z = 0;
        
        // Calculate pull direction and distance
        Vector2 pullVector = startMouseWorldPos - currentMouseWorldPos;
        Vector2 pullDirection = pullVector.normalized;
        float pullDistance = Mathf.Min(pullVector.magnitude, maxPullDistance);
        
        // Update trajectory line (showing launch direction)
        Vector3 endPoint = startPosition + (Vector3)(pullDirection * pullDistance);
        
        trajectoryLine.SetPosition(0, startPosition);
        trajectoryLine.SetPosition(1, endPoint);
        
        // Update aim indicator
        aimIndicator.transform.position = endPoint;
        
        // Update line color based on power
        float powerRatio = pullDistance / maxPullDistance;
        Color lineColor = Color.Lerp(Color.yellow, Color.red, powerRatio);
        trajectoryLine.startColor = lineColor;
        trajectoryLine.endColor = lineColor;
    }
    
    void Launch()
    {
        Vector3 currentMouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        currentMouseWorldPos.z = 0;
        
        // Calculate launch force
        Vector2 pullVector = startMouseWorldPos - currentMouseWorldPos;
        Vector2 launchDirection = pullVector.normalized;
        float pullDistance = Mathf.Min(pullVector.magnitude, maxPullDistance);
        
        // Apply force proportional to pull distance
        float forceMultiplier = (pullDistance / maxPullDistance) * maxForce;
        rb2d.AddForce(launchDirection * forceMultiplier, ForceMode2D.Impulse);
        
        // Clean up
        isDragging = false;
        trajectoryLine.enabled = false;
        aimIndicator.SetActive(false);
        
        Debug.Log($"Launched 2D sprite with force: {forceMultiplier}, Direction: {launchDirection}");
    }
    
    void OnDrawGizmos()
    {
        if (isDragging)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, maxPullDistance);
        }
    }
}
