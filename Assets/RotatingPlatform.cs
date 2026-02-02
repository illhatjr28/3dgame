using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float rotationSpeed = 90f;
    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.rotation;
    }

    void Update()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform, true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}   