using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    [Header("Structure Creation")]
    [SerializeField] private int structureHeight = 6;
    [SerializeField] private int structureWidth = 4;
    private BuildingType[,] structure;

    //[Header("Structure Building")]
    private BuildingClass[,] structureBuilding;
    private bool destructionWasCalled = false;
    
    #region Initialization
    // ----------------------------------------------------------------------------------------------------------
    
    /// <summary>
    /// Initialize the structure 2D array
    /// </summary>
    private void Awake()
    {
        this.structure = new BuildingType[this.structureHeight, this.structureWidth];
        this.structureBuilding = new BuildingClass[this.structureHeight,this.structureWidth];
        
        for (int i = 0; i < this.structureHeight; i++)
        {
            for (int j = 0; j < this.structureWidth; j++)
            {
                this.structure[i, j] = BuildingType.None;
            }
        }

        int midHeight = (this.structureHeight - 1) / 2;
        int midWidth = (this.structureWidth - 1) / 2;
        
        this.structure[midHeight, midWidth] = BuildingType.Center;
        
        this.structure[midHeight + 1, midWidth] = BuildingType.Foreuse;
        this.structure[midHeight + 1, midWidth + 1] = BuildingType.Foreuse;
        this.structure[midHeight + 1, midWidth - 1] = BuildingType.Foreuse;
        
        this.structure[midHeight - 1, midWidth] = BuildingType.City;
        this.structure[midHeight - 1, midWidth-1] = BuildingType.City;
        this.structure[midHeight - 1, midWidth+1] = BuildingType.City;
        
        this.structure[midHeight, midWidth+1] = BuildingType.Canon;
        this.structure[midHeight, midWidth-1] = BuildingType.Canon;
    }

    // ----------------------------------------------------------------------------------------------------------
    
    /// <summary>
    /// Initialize the structure building
    /// </summary>
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2.0f);
        InitializeStructure();
        yield break;
    }

    // ----------------------------------------------------------------------------------------------------------
    
    /// <summary>
    /// Called in Start()
    /// Initialize and instantiate the default structure
    /// </summary>
    private void InitializeStructure()
    {
        for (int i = 0; i < this.structureHeight; i++)
        {
            for (int j = 0; j < this.structureWidth; j++)
            {
                if(this.structure[i,j] == BuildingType.None)
                    continue;
                
                InitializeBuilding(i,j, this.structure[i,j]);
            }
        }
        InitializeNeighbour();
    }
    
    // ----------------------------------------------------------------------------------------------------------
    
    /// <summary>
    /// Called in InitializeStructure()
    /// Instantiate the given building at the given position
    /// </summary>
    /// <param name="coordonnees"></param>
    /// <param name="type"></param>
    private void InitializeBuilding(int i, int j, BuildingType type)
    {
        BuildingClass building;
        switch (type)
        {
            case BuildingType.Center:
                building = CenterPoolScript.Instance.WakeUp();
                this.structureBuilding[i, j] = building;
                
                building.WakeUp(new Vector3(j - this.structureWidth/2,0, i - this.structureHeight/2), this.transform,
                            null, null, null, null);
                
                building.SetPlayerManager(this);
                building.SetTabPosition(i, j);
                break;
            
            case BuildingType.City:
                building = CityPoolScript.Instance.WakeUp();
                this.structureBuilding[i, j] = building;
                
                building.WakeUp(new Vector3(j - this.structureWidth/2,0, i - this.structureHeight/2), this.transform,
                    null, null, null, null);
                
                building.SetPlayerManager(this);
                building.SetTabPosition(i, j);
                break;
            
            case BuildingType.Foreuse:
                building = ForeusePoolScript.Instance.WakeUp();
                this.structureBuilding[i, j] = building;
                
                building.WakeUp(new Vector3(j - this.structureWidth/2,0, i - this.structureHeight/2), this.transform,
                    null, null, null, null);
                
                building.SetPlayerManager(this);
                building.SetTabPosition(i, j);
                break;
            
            case BuildingType.Canon:
                building = CanonPoolScript.Instance.WakeUp();
                this.structureBuilding[i, j] = building;
                
                building.WakeUp(new Vector3(j - this.structureWidth/2,0, i - this.structureHeight/2), this.transform,
                    null, null, null, null);
                
                building.SetPlayerManager(this);
                building.SetTabPosition(i, j);
                break;
        }
        
    }
    
    // ----------------------------------------------------------------------------------------------------------

    private void InitializeNeighbour()
    {
        for (int i = 0; i < this.structureHeight; i++)
        {
            for (int j = 0; j < this.structureWidth; j++)
            {
                if (this.structureBuilding[i, j] == null)
                    continue;
                
                this.structureBuilding[i, j].SetLeftNeighbour(this.structureBuilding[i, j - 1] != null ? this.structureBuilding[i, j - 1] : null);
                this.structureBuilding[i, j].SetRightNeighbour(this.structureBuilding[i, j + 1] != null ? this.structureBuilding[i, j + 1] : null);
                this.structureBuilding[i, j].SetTopNeighbour(this.structureBuilding[i + 1, j] != null ? this.structureBuilding[i + 1,j] : null);
                this.structureBuilding[i, j].SetBottomNeighbour(this.structureBuilding[i - 1, j] != null ? this.structureBuilding[i - 1,j] : null);
            }
        }
    }
    
    // ----------------------------------------------------------------------------------------------------------
    #endregion

    #region Functions

    public void SetBuildingNeighbour(int i, int j)
    {
        this.structureBuilding[i, j].SetLeftNeighbour(this.structureBuilding[i, j - 1] != null ? this.structureBuilding[i, j - 1] : null);
        this.structureBuilding[i, j].SetRightNeighbour(this.structureBuilding[i, j + 1] != null ? this.structureBuilding[i, j + 1] : null);
        this.structureBuilding[i, j].SetTopNeighbour(this.structureBuilding[i + 1, j] != null ? this.structureBuilding[i + 1,j] : null);
        this.structureBuilding[i, j].SetBottomNeighbour(this.structureBuilding[i - 1, j] != null ? this.structureBuilding[i - 1,j] : null);
    }

    public void RemoveBuildingFromTab(int i, int j)
    {
        this.structure[i, j] = BuildingType.None;
        this.structureBuilding[i, j] = null;
    }
    
    #endregion
    
    #region Destruction

    public void LaunchCheckConnectedBuilding()
    {
        if (!this.destructionWasCalled)
        {
            for (int i = 0; i < this.structureHeight; i++)
            {
                for (int j = 0; j < this.structureWidth; j++)
                {
                    if(this.structureBuilding[i,j] == null)
                        continue;
                    
                    this.structureBuilding[i,j].SetConnectedToTheCenter(false, false);
                }
            }
            
            this.structureBuilding[this.structureHeight/2, this.structureWidth/2].SetConnectedToTheCenter(true,true);
            this.destructionWasCalled = true;
            StartCoroutine(CheckConnectedBuilding());
        }
    }

    private IEnumerator CheckConnectedBuilding()
    {
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < this.structureHeight; i++)
        {
            for (int j = 0; j < this.structureWidth; j++)
            {
                if(this.structureBuilding[i,j] == null)
                    continue;
                
                if(this.structureBuilding[i, j].IsConnectedToTheCenter)
                    continue;
                
                this.structureBuilding[i,j].Sleep();
            }
        }

        InitializeNeighbour();
        
        this.destructionWasCalled = false;
        
        yield break;
    }
    
    #endregion
}
