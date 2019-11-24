using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBuilding : BuildingClass
{
    private void OnEnable()
    {
        if (this.transform.position.x > 0)
            this.playerManager.OnRightFire += Fire;
        else if (this.transform.position.x < 0)
            this.playerManager.OnLeftFire += Fire;
    }

    private void OnDisable()
    {
        if (this.transform.position.x > 0)
            this.playerManager.OnRightFire -= Fire;
        else if (this.transform.position.x < 0)
            this.playerManager.OnLeftFire -= Fire;
    }
    
    private void Fire()
    {
        Debug.Log("Fire + " + this.transform.position.x);
    }
}
