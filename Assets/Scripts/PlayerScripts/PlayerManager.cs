using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int playerID = 1;
    public int PlayerID => playerID;

    [Header("Player Behavior")] 
    [SerializeField] private CameraController cameraController;
    [SerializeField] private CameraInputDetector cameraInputDetector;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerInputDetector playerInputDetector;
    
    [Header("Structure Creation")]
    [SerializeField] private int structureHeight = 6;
    [SerializeField] private int structureWidth = 4;
    private BuildingType[,] structure;
    private BuildingClass[,] structureBuilding;
    private bool destructionWasCalled = false;
    private int midHeight = 0;
    private int midWidth = 0;
    
    [Header("Structure Informations")] 
    [SerializeField] private float weightOffset = 5.0f;
    [SerializeField] private float minWeight = 10.0f;
    [SerializeField] private Vector2 weightCoeffRange = new Vector2(0.25f, 1.75f);
    private float currentWeight = 0.0f;

    [Header("Constructor Information")] 
    [SerializeField] private int defaultMatter = 200;
    [SerializeField] private ConstructorSelectedManager selectedBuildingVisual;
    private int currentMatter = 0;
    private BuildingType currentSelectedBuilding = BuildingType.City;
    private int currentSelectedBuildingAsInt = 0;
    private int currentX = 0;
    private int currentY = 0;
    
    [Header("Mode")] 
    [SerializeField] private PlayerMode currentMode = PlayerMode.Normal;
    public PlayerMode CurrentMode => currentMode;

    // -- Actions --
    public delegate void Fire();
    public event Fire OnRightFire; // Used to call every "Fire" function for each canon at the right of the center
    public event Fire OnLeftFire; // Used to call every "Fire" function for each canon at the left of the center
    
    #region Initialization
    // ----------------------------------------------------------------------------------------------------------
    
    /// <summary>
    /// Initialize the structure 2D array
    /// </summary>
    private void Awake()
    {
        InitializeStructure();

        if (this.playerInputDetector == null)
            this.playerInputDetector = GetComponent<PlayerInputDetector>();
        
        if (this.cameraInputDetector == null)
            this.cameraInputDetector = GetComponent<CameraInputDetector>();
        
        if (this.playerController == null)
            this.playerController = GetComponent<PlayerController>();

        if (this.cameraController == null)
            this.cameraController = GetComponent<CameraController>();

        this.currentX = this.midWidth;
        this.currentY = this.midHeight;
        this.currentMatter = this.defaultMatter;
    }

    // ----------------------------------------------------------------------------------------------------------
    
    /// <summary>
    /// Called in Awake()
    /// Initialize the initial structure
    /// </summary>
    private void InitializeStructure()
    {
        this.structure = new BuildingType[this.structureHeight, this.structureWidth];
        this.structureBuilding = new BuildingClass[this.structureHeight, this.structureWidth];

        for (int i = 0; i < this.structureHeight; i++)
        {
            for (int j = 0; j < this.structureWidth; j++)
            {
                this.structure[i, j] = BuildingType.None;
            }
        }

        midHeight = (this.structureHeight - 1) / 2;
        midWidth = (this.structureWidth - 1) / 2;

        this.structure[midHeight, midWidth] = BuildingType.Center;

        this.structure[midHeight + 1, midWidth] = BuildingType.Foreuse;
        this.structure[midHeight + 1, midWidth + 1] = BuildingType.Foreuse;
        this.structure[midHeight + 1, midWidth - 1] = BuildingType.Foreuse;

        this.structure[midHeight - 1, midWidth] = BuildingType.City;
        this.structure[midHeight - 1, midWidth - 1] = BuildingType.City;
        this.structure[midHeight - 1, midWidth + 1] = BuildingType.City;

        this.structure[midHeight, midWidth + 1] = BuildingType.Canon;
        this.structure[midHeight, midWidth - 1] = BuildingType.Canon;
    }

    // ----------------------------------------------------------------------------------------------------------
    
    /// <summary>
    /// Initialize the structure building
    /// </summary>
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        InitializeBuildingStructure();
        yield break;
    }

    // ----------------------------------------------------------------------------------------------------------
    
    /// <summary>
    /// Called in Start()
    /// Initialize and instantiate the default structure
    /// </summary>
    private void InitializeBuildingStructure()
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
        InitializeBuildingNeighbour();
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
                
                building.SetPlayerManager(this);
                
                building.WakeUp(new Vector3(j - this.midWidth,0, i - this.midHeight), this.transform);
                building.SetTabPosition(i, j);
                break;
            
            case BuildingType.City:
                building = CityPoolScript.Instance.WakeUp();
                this.structureBuilding[i, j] = building;
                building.SetPlayerManager(this);
                
                building.WakeUp(new Vector3(j - this.midWidth,0, i - this.midHeight), this.transform);
                building.SetTabPosition(i, j);
                break;
            
            case BuildingType.Foreuse:
                building = ForeusePoolScript.Instance.WakeUp();
                this.structureBuilding[i, j] = building;
                
                building.SetPlayerManager(this);
                
                building.WakeUp(new Vector3(j - this.midWidth,0, i - this.midHeight), this.transform);
                building.SetTabPosition(i, j);
                break;
            
            case BuildingType.Canon:
                building = CanonPoolScript.Instance.WakeUp();
                this.structureBuilding[i, j] = building;
                
                building.SetPlayerManager(this);
                
                building.WakeUp(new Vector3(j - this.midWidth,0, i - this.midHeight), this.transform);
                building.SetTabPosition(i, j);
                break;
        }
        
    }
    
    // ----------------------------------------------------------------------------------------------------------

    private void InitializeBuildingNeighbour()
    {
        for (int i = 0; i < this.structureHeight; i++)
        {
            for (int j = 0; j < this.structureWidth; j++)
            {
                if (this.structureBuilding[i, j] == null)
                    continue;
                
                SetBuildingNeighbour(i,j);

//                this.structureBuilding[i, j].SetLeftNeighbour(this.structureBuilding[i, j - 1 > 0 ? j - 1 : j] != null ? this.structureBuilding[i, j - 1 > 0 ? j - 1 : j] : null);
//                
//                this.structureBuilding[i, j].SetRightNeighbour(this.structureBuilding[i, j + 1 < this.structureWidth ? j + 1 : j] != null ? this.structureBuilding[i, j + 1 < this.structureWidth ? j + 1 : j] : null);
//                
//                this.structureBuilding[i, j].SetTopNeighbour(this.structureBuilding[i + 1 < this.structureHeight ? i + 1 : i, j] != null ? this.structureBuilding[i + 1 < this.structureHeight ? i + 1 : i,j] : null);
//                
//                this.structureBuilding[i, j].SetBottomNeighbour(this.structureBuilding[i - 1 > 0 ? i - 1 : i, j] != null ? this.structureBuilding[i - 1 > 0 ? i - 1 : i,j] : null);
            }
        }
    }
    
    // ----------------------------------------------------------------------------------------------------------
    #endregion

    #region Loop

    private void Update()
    {
        if (this.playerInputDetector != null)
            this.playerInputDetector.UpdateCustom();
        
        if(this.cameraInputDetector != null)
            this.cameraInputDetector.UpdateCustom();

        if (this.playerController != null)
            this.playerController.UpdateCustom();
        
        if(this.cameraController != null)
            this.cameraController.UpdateCustom();
    }

    #endregion

    #region Fire Functions

    public void LeftFire()
    {
        OnLeftFire();
    }

    public void RightFire()
    {
        OnRightFire();
    }
    
    #endregion
    
    #region Functions

    public void SetBuildingNeighbour(int i, int j)
    {
        if (j - 1 >= 0)
        {
            if (this.structureBuilding[i, j - 1] != null)
            {
                this.structureBuilding[i, j].SetLeftNeighbour(this.structureBuilding[i, j - 1]);
                this.structureBuilding[i, j - 1].SetRightNeighbour(this.structureBuilding[i, j]);
            }
            else
            {
                this.structureBuilding[i, j].SetLeftNeighbour(null);
            }
        }

        if (j + 1 < this.structureWidth)
        {
            if (this.structureBuilding[i, j + 1] != null)
            {
                this.structureBuilding[i, j].SetRightNeighbour(this.structureBuilding[i, j + 1]);
                this.structureBuilding[i, j + 1].SetLeftNeighbour(this.structureBuilding[i,j]);
            }
            else
            {
                this.structureBuilding[i, j].SetRightNeighbour(null);
            }
        }

        if (i + 1 < this.structureHeight)
        {
            if (this.structureBuilding[i + 1, j] != null)
            {
                this.structureBuilding[i, j].SetTopNeighbour(this.structureBuilding[i + 1, j]);
                this.structureBuilding[i + 1, j].SetBottomNeighbour(this.structureBuilding[i, j]);
            }
            else
            {
                this.structureBuilding[i, j].SetTopNeighbour(null);
            }
        }

        if (i - 1 >= 0)
        {
            if (this.structureBuilding[i - 1, j] != null)
            {
                this.structureBuilding[i, j].SetBottomNeighbour(this.structureBuilding[i - 1, j]);
                this.structureBuilding[i - 1, j].SetTopNeighbour(this.structureBuilding[i, j]);
            }
            else
            {
                this.structureBuilding[i, j].SetBottomNeighbour(null);
            }
            
        }
        
    }

    public void RemoveBuildingFromTab(int i, int j)
    {
        this.structure[i, j] = BuildingType.None;
        this.structureBuilding[i, j] = null;
    }

    public void SetPlayerMode(PlayerMode mode)
    {
        this.currentMode = mode;
    }

    public void UpdateStructureWeight(float value)
    {
        this.currentWeight += value;
    }
    
    public float GetWeightCoeff()
    {
        return Mathf.Clamp((this.minWeight + this.weightOffset) / this.currentWeight, this.weightCoeffRange.x, this.weightCoeffRange.y);
    }

    public void UpdateStructureTypeAt(int i, int j, int previousI, int previousJ, BuildingClass building)
    {
        this.structure[i, j] = building.Building;
        this.structureBuilding[i, j] = building;
        if (previousI != -1 && previousJ != -1)
        {
            this.structure[previousI, previousJ] = BuildingType.None;
            this.structureBuilding[previousI, previousJ] = null;
        }
    }
    
    #endregion

    #region Constructor Function

    public void SetActiveConstructorVisual(bool value)
    {
        this.currentX = this.midWidth;
        this.currentY = this.midHeight;
        
        this.selectedBuildingVisual.SetPosition(new Vector3(this.currentX - this.midWidth, this.selectedBuildingVisual.YOffset, this.currentY - this.midHeight));
        
        this.selectedBuildingVisual.gameObject.SetActive(value);
        
        if(value)
            this.selectedBuildingVisual.EnableSelectedBuilding(this.currentSelectedBuilding);
    }

    public void UpdateConstructorPosition(int dirX, int dirY)
    {
        int x = dirX + this.currentX;
        int y = dirY + this.currentY;

        if (y >= this.structureHeight)
            y = this.structureHeight - 1;
        else if (y < 0)
            y = 0;
        
        if (x >= this.structureWidth)
            x = this.structureWidth - 1;
        else if (x < 0)
            x = 0;

        if (this.structure[y, this.currentX] == BuildingType.None)
        {
            if (this.structure[y + 1 < this.structureHeight ? y + 1 : y, this.currentX] == BuildingType.None
                && this.structure[y - 1 >= 0 ? y - 1 : y, this.currentX] == BuildingType.None
                && this.structure[y, this.currentX + 1 < this.structureWidth ? this.currentX + 1 : this.currentX] == BuildingType.None
                && this.structure[y, this.currentX - 1 >= 0 ? this.currentX - 1 : this.currentX] == BuildingType.None)
            {
                Debug.Log("la structure au dessus//dessous est none et celle actuelle aussi");
                return;
            }
            
            if (this.structure[y, this.currentX + 1 < this.structureWidth ? this.currentX + 1 : this.currentX] ==
                BuildingType.Foreuse
                || this.structure[y, this.currentX - 1 > 0 ? this.currentX - 1 : this.currentX] == BuildingType.Foreuse)
            {
                Debug.Log("Foreuse pas loin Y");
                return;
            }
        }
        

        if (this.structure[this.currentY, x] == BuildingType.None)
        {
            if (this.structure[this.currentY + 1 < this.structureHeight ? this.currentY + 1 : this.currentY, x] == BuildingType.None
                && this.structure[this.currentY - 1 >= 0 ? this.currentY - 1 : this.currentY, x] == BuildingType.None
                && this.structure[this.currentY, x + 1 < this.structureWidth ? x + 1 : x] == BuildingType.None 
                && this.structure[this.currentY, x - 1 >= 0 ? x - 1 : x] == BuildingType.None )
            {
                Debug.Log("la structure a gauche//droite est none et celle actuelle aussi");
                return;
            }
        
            if(this.structure[this.currentY, x + 1 < this.structureWidth ? x + 1 : x] == BuildingType.Foreuse 
               || this.structure[this.currentY, x - 1 > 0 ? x - 1 : x] == BuildingType.Foreuse)
            {
                Debug.Log("Foreuse pas loin X");
                return;
            }

            if (this.structure[this.currentY - 1 > 0 ? this.currentY - 1 : this.currentY, x] == BuildingType.Foreuse)
            {
                Debug.Log("Foreuse en bas");
                return;
            }
        }
        
        if((this.structure[this.currentY,this.currentX] == BuildingType.Foreuse && y > this.currentY))
            return;
        
        if(this.structure[this.currentY, this.currentX] == BuildingType.Foreuse)
            if (this.structure[this.currentY, x] == BuildingType.None)
                return;

        Debug.Log("On change la position");
        
        this.currentX = x;
        this.currentY = y;
        
        if(this.currentSelectedBuildingAsInt == 0)
            UpdateSelectedBuilding(0);
        
        this.selectedBuildingVisual.SetPosition(new Vector3(this.currentX - this.midWidth, this.selectedBuildingVisual.YOffset, this.currentY - this.midHeight));
    }

    public void UpdateSelectedBuilding(int dir)
    {
        this.currentSelectedBuildingAsInt += dir;

        if (this.currentSelectedBuildingAsInt < 0)
            this.currentSelectedBuildingAsInt = 2;
        else if (this.currentSelectedBuildingAsInt > 2)
            this.currentSelectedBuildingAsInt = 0;

        switch (this.currentSelectedBuildingAsInt)
        {
            case 0:
                this.currentSelectedBuilding = BuildingType.City;
                
                if (this.structure[this.currentY, this.currentX] == BuildingType.City)
                    this.currentSelectedBuilding = BuildingType.Mastodonte;
                break;
            
            case 1:
                this.currentSelectedBuilding = BuildingType.Canon;
                break;
            
            case 2:
                this.currentSelectedBuilding = BuildingType.Centrale;
                break;
        }
        
        this.selectedBuildingVisual.EnableSelectedBuilding(this.currentSelectedBuilding);
    }
    
    public void BuildSelectedBuilding()
    {
        if (this.structure[this.currentY, this.currentX] == BuildingType.Center)
            return;

        if (this.structure[this.currentY, this.currentX] == BuildingType.Foreuse)
        {
            if(this.currentY + 1 == this.structureHeight)
                return;
            
            this.structure[this.currentY + 1, this.currentX] = BuildingType.Foreuse;
            BuildingClass foreuse = this.structureBuilding[this.currentY, this.currentX];
            this.structureBuilding[this.currentY + 1, this.currentX] = foreuse;
            foreuse.SetTabPosition(this.currentY + 1, this.currentX);
            foreuse.SetPosition(new Vector3(this.currentX - this.midWidth,0, this.currentY + 1 - this.midHeight));
        }
        
        BuildingClass building;
        switch (this.currentSelectedBuilding)
        {
            case BuildingType.City:
                building = CityPoolScript.Instance.WakeUp();
                
                this.structureBuilding[this.currentY, this.currentX] = building;
                
                building.SetPlayerManager(this);
                
                SetBuildingNeighbour(this.currentY, this.currentX);
                
                building.WakeUp(new Vector3(this.currentX - this.midWidth,0, this.currentY - this.midHeight), this.transform);
                
                building.SetTabPosition(this.currentY, this.currentX);
                break;
            
            case BuildingType.Canon:
                building = CanonPoolScript.Instance.WakeUp();
                
                this.structureBuilding[this.currentY, this.currentX] = building;
                
                SetBuildingNeighbour(this.currentY, this.currentX);
                
                building.SetPlayerManager(this);
                
                building.WakeUp(new Vector3(this.currentX - this.midWidth,0, this.currentY - this.midHeight), this.transform);
                
                building.SetTabPosition(this.currentY, this.currentX);
                break;
            
            case BuildingType.Centrale:
                building = CentralePoolScript.Instance.WakeUp();
                
                this.structureBuilding[this.currentY, this.currentX] = building;
                
                SetBuildingNeighbour(this.currentY, this.currentX);
                
                building.SetPlayerManager(this);
                
                building.WakeUp(new Vector3(this.currentX - this.midWidth,0, this.currentY - this.midHeight), this.transform);
                
                building.SetTabPosition(this.currentY, this.currentX);
                break;
            
            case BuildingType.Mastodonte:
                building = MastodontePoolScript.Instance.WakeUp();
                
                this.structureBuilding[this.currentY, this.currentX] = building;
                
                SetBuildingNeighbour(this.currentY, this.currentX);
                
                building.SetPlayerManager(this);
                
                building.WakeUp(new Vector3(this.currentX - this.midWidth,0, this.currentY - this.midHeight), this.transform);
                
                building.SetTabPosition(this.currentY, this.currentX);
                break;
        }
        
        this.structure[this.currentY, this.currentX] = this.currentSelectedBuilding;
        
        if(this.structure[this.currentY + 1, this.currentX] == BuildingType.Foreuse)
            SetBuildingNeighbour(this.currentY + 1, this.currentX);
        
        if(this.currentSelectedBuildingAsInt == 0)
            UpdateSelectedBuilding(0);
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

            if (this.structure[midHeight, midWidth] != BuildingType.None && this.structure[midHeight, midWidth] == BuildingType.Center)
            {
                Debug.Log("le centre est connecter a lui meme");
                this.structureBuilding[midHeight, midWidth].SetConnectedToTheCenter(true,true);
            }
            
            this.destructionWasCalled = true;
            
            StartCoroutine(CheckConnectedBuilding());
        }
    }

    private IEnumerator CheckConnectedBuilding()
    {
        yield return new WaitForSeconds(1.0f);
        
        Debug.Log("Destruction !");
        
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
        
        this.destructionWasCalled = false;
        
        yield break;
    }
    
    #endregion
}
