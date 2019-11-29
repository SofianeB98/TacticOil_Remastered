using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForeuseBuilding : BuildingClass
{
    [Header("Structure Dectection")] 
    [SerializeField] private float detectionDistance = 1.5f;
    [SerializeField] private float rayRadius = 1.0f;
    [SerializeField] private int foreuseNumber = 3;
    private RaycastHit hit;

    [Header("Detection Information")] 
    [SerializeField] private string playerTag = "Player";
    
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
                    pc.ApplyForce(this.playerController.CurrentSpeed / this.foreuseNumber * this.transform.forward , this.playerManager.CurrentWeight);
                    this.playerController.DecreaseSpeedAfterCollision();
                }
            }
        }
    }

    // this.collisionForceVelocity += velocity / 3 * weight / this.playerManager.CurrentWeight;

    // Apply force if a structure is detected

    // Set the velocity of this structure to : "velocity - 95%"


}
