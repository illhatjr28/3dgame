using UnityEngine;

public class TiltingPlatform : MonoBehaviour
{
    [SerializeField] private float maxTiltAngle = 15f;
    [SerializeField] private float tiltSpeed = 2f;
    [SerializeField] private float returnSpeed = 1f;
    [SerializeField] private float detectionHeight = 3f;
    [SerializeField] private float detectionRadius = 5f;

    private Transform player;
    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.rotation;
        
        // Find the player in the scene
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            Debug.Log("Player found: " + player.name);
        }
        else
        {
            Debug.LogWarning("No player found with 'Player' tag!");
        }
    }

    void Update()
    {
        if (player == null)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, returnSpeed * Time.deltaTime);
            return;
        }

        // Check if player is on this platform
        bool isPlayerOnPlatform = IsPlayerOnPlatform();

        if (!isPlayerOnPlatform)
        {
            // Return to flat position
            transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, returnSpeed * Time.deltaTime);
            return;
        }

        // Get player position relative to platform center
        Vector3 localPlayerPos = transform.InverseTransformPoint(player.position);
        
        // Calculate tilt based on player's local position (X and Z)
        float tiltX = Mathf.Clamp(localPlayerPos.z, -1f, 1f) * maxTiltAngle;
        float tiltZ = Mathf.Clamp(-localPlayerPos.x, -1f, 1f) * maxTiltAngle;
        
        // Create target rotation
        Quaternion targetRotation = startRotation * Quaternion.Euler(tiltX, 0, tiltZ);
        
        // Smoothly tilt the platform
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
    }

    bool IsPlayerOnPlatform()
    {
        // Simple distance check from platform center
        float horizontalDistance = Vector2.Distance(
            new Vector2(player.position.x, player.position.z),
            new Vector2(transform.position.x, transform.position.z)
        );
        
        float verticalDistance = Mathf.Abs(player.position.y - transform.position.y);
        
        bool onPlatform = horizontalDistance < detectionRadius && verticalDistance < detectionHeight;
        
        return onPlatform;
    }
}