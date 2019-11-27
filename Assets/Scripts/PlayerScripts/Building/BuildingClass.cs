using System;
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

    [Header("Informations")] 
    [SerializeField] protected BuildingType building;
    [SerializeField] protected float buildingWeight = 1.0f;
    [SerializeField] protected float defaultResistance = 100.0f;
    private float currentResistance = 0.0f;
    public float BuildingWeight => buildingWeight;
    public BuildingType Building => building;

    private int posTabX, posTabY;

    private void Update()
    {
        if (this.defaultResistance <= 0)
        {
            ApplyDamage(200);
            this.defaultResistance = 100;
        }
    }

    #region Pooling

    public virtual void WakeUp(Vector3 position, Transform parent)
    {
        this.currentResistance = this.defaultResistance;
        
        this.isConnectedToTheCenter = true;
        
        this.transform.position = position;

        this.playerManager.UpdateStructureWeight(this.buildingWeight);
        
        this.transform.SetParent(parent);
        this.gameObject.SetActive(true);
    }

    public virtual void Sleep()
    {
        this.gameObject.SetActive(false);
        this.isConnectedToTheCenter = false;
        
        if (this.leftNeighbour != null)
            this.leftNeighbour.SetRightNeighbour(null);

        if (this.rightNeighbour != null)
            this.rightNeighbour.SetLeftNeighbour(null);
        
        if(this.topNeighbour != null)
            this.topNeighbour.SetBottomNeighbour(null);
        
        if(this.bottomNeighbour != null)
            this.bottomNeighbour.SetTopNeighbour(null);
        
        if (this.playerManager != null)
        {
            this.playerManager.UpdateStructureWeight(-this.buildingWeight);
            this.playerManager.RemoveBuildingFromTab(this.posTabY, this.posTabX);

            if (this.topNeighbour != null)
            {
                if (this.topNeighbour.Building == BuildingType.Foreuse)
                {
                    this.playerManager.UpdateStructureTypeAt(this.posTabY, this.posTabX, this.posTabY + 1, this.posTabX,
                        this.topNeighbour);
                    this.topNeighbour.SetTabPosition(this.posTabY, this.posTabX);
                    this.topNeighbour.SetPosition(this.transform.localPosition);
                }
            }
            
            this.leftNeighbour = null;
            this.rightNeighbour = null;
            this.topNeighbour = null;
            this.bottomNeighbour = null;
            
            this.playerManager.LaunchCheckConnectedBuilding();
        }

        this.playerManager = null;
        
        this.transform.SetParent(null);
        
    }

    #endregion
    
    #region Setter

    public void SetPlayerManager(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }
    
    public void SetLeftNeighbour(BuildingClass neighbour)
    {
        this.leftNeighbour = neighbour;
    }

    public void SetRightNeighbour(BuildingClass neighbour)
    {
        this.rightNeighbour = neighbour;
    }
    
    public void SetTopNeighbour(BuildingClass neighbour)
    {
        this.topNeighbour = neighbour;
    }
    
    public void SetBottomNeighbour(BuildingClass neighbour)
    {
        this.bottomNeighbour = neighbour;
    }

    public void SetConnectedToTheCenter(bool value, bool launchNeighbourChecker)
    {
        this.isConnectedToTheCenter = value;

        if (launchNeighbourChecker)
        {
            Debug.Log("on set les voisin connecter au centre");
            SetNeighbourConnectedToTheCenter();
        }
    }

    public void SetTabPosition(int i, int j)
    {
        this.posTabX = j;
        this.posTabY = i;
    }

    public void SetPosition(Vector3 pos)
    {
        this.transform.localPosition = pos;
    }
    #endregion

    public virtual void ApplyDamage(float damage)
    {
        this.currentResistance -= damage;
        if (this.currentResistance <= 0.0f)
        {
            Sleep();
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
