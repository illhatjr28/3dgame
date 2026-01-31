using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameRespawn : MonoBehaviour
{
    public float threshold;
    private CharacterController controller;
    private SimpleFirstPersonController playerController;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerController = GetComponent<SimpleFirstPersonController>();
    }
    
    void Update()
    {
        if(transform.position.y < threshold)
        {
            if(controller != null)
            {
                controller.enabled = false;
                transform.position = new Vector3(-2.426f, 2.247f, -7.055f);
                controller.enabled = true;
            }
            else
            {
                transform.position = new Vector3(-2.426f, 2.247f, -7.055f);
            }
            
            // Reset camera angle
            if(playerController != null)
            {
                playerController.ResetCamera();
            }
        }
    }
    
    public void TriggerRespawn()
    {
        if(controller != null)
        {
            controller.enabled = false;
            transform.position = new Vector3(-2.426f, 2.247f, -7.055f);
            controller.enabled = true;
        }
        else
        {
            transform.position = new Vector3(-2.426f, 2.247f, -7.055f);
        }
        
        // Reset camera angle
        if(playerController != null)
        {
            playerController.ResetCamera();
        }
    }
}
