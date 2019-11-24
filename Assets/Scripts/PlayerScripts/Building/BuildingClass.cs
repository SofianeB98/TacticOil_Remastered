using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingClass : MonoBehaviour
{
    [SerializeField] protected PlayerManager playerManager;
    
    protected bool isConnectedToTheCenter = true;
    public bool IsConnectedToTheCenter => isConnectedToTheCenter;
    
    [Header("Neighbour")]
    [SerializeField] protected BuildingClass leftNeighbour;
    [SerializeField] protected BuildingClass rightNeighbour;
    [SerializeField] protected BuildingClass topNeighbour;
    [SerializeField] protected BuildingClass bottomNeighbour;

    [Header("Information")] 
    [SerializeField] protected float defaultResistance = 100.0f;
    private float currentResistance = 0.0f;

    private int posTabX, posTabY;
    
    #region Pooling

    public virtual void WakeUp(Vector3 position, Transform parent, 
        BuildingClass leftNeighbour, BuildingClass rightNeighbour, BuildingClass topNeighbour,  BuildingClass bottomNeighbour)
    {
        this.currentResistance = this.defaultResistance;
        
        this.isConnectedToTheCenter = true;
        
        this.transform.position = position;
        
        this.bottomNeighbour = bottomNeighbour;
        this.leftNeighbour = leftNeighbour;
        this.rightNeighbour = rightNeighbour;
        this.topNeighbour = topNeighbour;
        
        this.transform.SetParent(parent);
        this.gameObject.SetActive(true);
    }

    public virtual void Sleep()
    {
        this.leftNeighbour = null;
        this.rightNeighbour = null;
        this.topNeighbour = null;
        this.bottomNeighbour = null;
        this.isConnectedToTheCenter = false;
        
        if(this.playerManager != null)
            this.playerManager.RemoveBuildingFromTab(this.posTabX, this.posTabY);

        this.playerManager = null;
        
        this.transform.SetParent(null);
        this.gameObject.SetActive(false);
    }

    #endregion
    
    #region Setter

    public void SetPlayerManager(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }
    
    public void SetLeftNeighbour(BuildingClass neighbour = null)
    {
        this.leftNeighbour = neighbour;
    }

    public void SetRightNeighbour(BuildingClass neighbour = null)
    {
        this.rightNeighbour = neighbour;
    }
    
    public void SetTopNeighbour(BuildingClass neighbour = null)
    {
        this.topNeighbour = neighbour;
    }
    
    public void SetBottomNeighbour(BuildingClass neighbour = null)
    {
        this.bottomNeighbour = neighbour;
    }

    public void SetConnectedToTheCenter(bool value, bool launchNeighbourChecker)
    {
        this.isConnectedToTheCenter = value;
        
        if(launchNeighbourChecker)
            SetNeighbourConnectedToTheCenter();
    }

    public void SetTabPosition(int i, int j)
    {
        this.posTabX = i;
        this.posTabY = j;
    }
    
    #endregion

    public virtual void ApplyDamage(float damage)
    {
        this.currentResistance -= damage;
        if (this.currentResistance <= 0.0f)
        {
            Sleep();

            if (this.playerManager != null)
            {
                this.playerManager.LaunchCheckConnectedBuilding();
            }
        }
    }

    public virtual void SetNeighbourConnectedToTheCenter()
    {
        if(this.leftNeighbour != null)
            if(!this.leftNeighbour.IsConnectedToTheCenter)
                this.leftNeighbour.SetConnectedToTheCenter(true ,true);
        
        if(this.rightNeighbour != null)
            if(!this.rightNeighbour.IsConnectedToTheCenter)
                this.rightNeighbour.SetConnectedToTheCenter(true,true);
        
        if(this.bottomNeighbour != null)
            if(!this.bottomNeighbour.IsConnectedToTheCenter)
                this.bottomNeighbour.SetConnectedToTheCenter(true,true);
        
        if(this.topNeighbour != null)
            if(!this.topNeighbour.IsConnectedToTheCenter)
                this.topNeighbour.SetConnectedToTheCenter(true,true);
    }
}
