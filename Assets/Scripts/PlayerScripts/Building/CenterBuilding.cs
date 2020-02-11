using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterBuilding : BuildingClass
{
    public void OnDisable()
    {
        if(this.playerManager != null)
            this.playerManager.DestroyPlayer();
    }
}
