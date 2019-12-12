using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForeuseBuilding : BuildingClass
{
    [Header("Structure Dectection")] 
    [SerializeField] private float detectionDistance = 1.5f;
    [SerializeField] private float rayRadius = 1.0f;
    [SerializeField] private float additionalForce = 20.0f;
    private Transform lastHit;
    private RaycastHit hit;
    
    [Header("Detection Information")] 
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private List<PlayerController> playersDetected = new List<PlayerController>();
    
    private void Update()
    {
        
        if (Physics.SphereCast(this.transform.position, this.rayRadius, this.transform.forward, out this.hit,
            this.detectionDistance))
        {
            if (this.hit.transform.tag.Contains(this.playerTag) && !this.hit.transform.tag.Contains(this.playerManager.PlayerID.ToString()))
            {
                PlayerController pc = this.hit.transform.GetComponentInParent<PlayerController>();
                if (pc != null)
                {
                    if (this.playersDetected.Contains(pc))
                        return;
                    
                    this.playersDetected.Add(pc);
                    pc.ApplyForce(this.playerController.CurrentSpeed * this.additionalForce * this.transform.forward , this.playerManager.CurrentWeight);
                    this.playerController.DecreaseSpeedAfterCollision();
                    this.lastHit = this.hit.transform;
                }
            }
        }
        else
        {
            this.playersDetected.Clear();
        }
        
    }

    // this.collisionForceVelocity += velocity / 3 * weight / this.playerManager.CurrentWeight;

    // Apply force if a structure is detected

    // Set the velocity of this structure to : "velocity - 95%"

    private void CheckPlayersInRange()
    {
        
    }
}
