using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBuilding : BuildingClass
{
    [Header("Reloading Info")] 
    [SerializeField] private float additiveReloading = 0.5f;
    
    [Header("Fire Information")] 
    [SerializeField] private Transform bulletSpawnTransform;
    
    private void OnEnable()
    {
        if (this.transform.position.x > 0)
        {
            this.transform.localRotation = Quaternion.Euler(0.0f, 90.0f,0.0f);
            this.playerManager.OnRightFire += Fire;
            this.playerManager.AddAdditiveReloading(this.additiveReloading, false);
        }
        else if (this.transform.position.x < 0)
        {
            this.transform.localRotation = Quaternion.Euler(0.0f, -90.0f,0.0f);
            this.playerManager.OnLeftFire += Fire;
            this.playerManager.AddAdditiveReloading(this.additiveReloading, true);
        }
    }

    private void OnDisable()
    {
        if (this.transform.position.x > 0)
        {
            this.playerManager.OnRightFire -= Fire;
            this.playerManager.AddAdditiveReloading(-this.additiveReloading, false);
        }
        else if (this.transform.position.x < 0)
        {
            this.playerManager.OnLeftFire -= Fire;
            this.playerManager.AddAdditiveReloading(-this.additiveReloading, true);
        }
    }
    
    private void Fire()
    {
        Debug.Log("Fire + " + this.transform.position.x);
        BulletScript bullet = BulletPoolScript.Instance.WakeUp();
        bullet.WakeUp(this.playerManager, this.bulletSpawnTransform.position, this.bulletSpawnTransform.rotation);
        bullet.AddForce(this.bulletSpawnTransform.forward);
    }
}
