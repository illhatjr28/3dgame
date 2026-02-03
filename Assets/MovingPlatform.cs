using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour
{
    [Header("Movement")]
    public Vector3 moveOffset = new Vector3(5, 0, 0);
    public float speed = 3.0f;
    public float waitTime = 1.0f;

    [Header("Settings")]
    public string playerTag = "Player";

    private Vector3 startPos;
    private Vector3 endPos;
    private bool movingToEnd = true;
    private float waitTimer;

    private Rigidbody rb;
    private Transform passenger;
    private CharacterController passengerCC; // Reference for FPS/TPS controllers

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // SAFETY SETTINGS: Prevent falling
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        startPos = transform.position;
        endPos = startPos + moveOffset;
    }

    void FixedUpdate()
    {
        // 1. Wait Logic
        if (waitTimer > 0)
        {
            waitTimer -= Time.fixedDeltaTime;
            return;
        }

        // 2. Calculate Platform Movement
        Vector3 target = movingToEnd ? endPos : startPos;
        float step = speed * Time.fixedDeltaTime;

        Vector3 currentPos = rb.position;
        Vector3 newPos = Vector3.MoveTowards(currentPos, target, step);

        // CALCULATE THE EXACT SHIFT (DELTA)
        Vector3 movementDelta = newPos - currentPos;

        // 3. Move Platform
        rb.MovePosition(newPos);

        // 4. Move Passenger (The Fix)
        if (passenger != null)
        {
            // Option A: If Player has a CharacterController (FPS/TPS)
            if (passengerCC != null)
            {
                // We use .Move() so it respects collisions (walls, etc)
                passengerCC.Move(movementDelta);
            }
            // Option B: Basic Objects (Crates, simple players)
            else
            {
                passenger.position += movementDelta;
            }
        }

        // 5. Turn Around
        if (Vector3.Distance(rb.position, target) < 0.01f)
        {
            movingToEnd = !movingToEnd;
            waitTimer = waitTime;
        }
    }

    // -----------------------------------------------------------
    //  TRIGGER LOGIC
    // -----------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            passenger = other.transform;
            // Try to find a CharacterController
            passengerCC = other.GetComponent<CharacterController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            passenger = null;
            passengerCC = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPos, endPos);
    }
}