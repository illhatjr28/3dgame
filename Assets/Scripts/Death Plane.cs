using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPlane : MonoBehaviour
{
    public GameObject player;
    public Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player detected - respawning at: " + respawnPoint.position);
            
            // Disable CharacterController if present
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false;
                player.transform.position = respawnPoint.position;
                cc.enabled = true;
            }
            else
            {
                // Fallback for Rigidbody-only
                player.transform.position = respawnPoint.position;
            }
        }
    }
}   