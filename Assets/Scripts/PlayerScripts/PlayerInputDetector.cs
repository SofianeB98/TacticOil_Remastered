using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputDetector : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private PlayerController playerController;
    
    [Header("Deplacement Input")] 
    [SerializeField] private string deplacementAxis = "Vertical";
    [SerializeField] private string rotationAxis = "Horizontal";

    [Header("Action Input")] 
    [SerializeField] private KeyCode fireButton = KeyCode.A;
    
    [Header("Construtor Mode Input")]
    [SerializeField] private KeyCode constructorModeButton = KeyCode.E;
    [SerializeField] private KeyCode validationButton = KeyCode.R;
    [SerializeField] private KeyCode switchRightSelectedBuildingButton = KeyCode.K;
    [SerializeField] private KeyCode switchLeftSelectedBuildingButton = KeyCode.L;

    private void Awake()
    {
        if (this.playerManager == null)
            this.playerManager = GetComponent<PlayerManager>();
        
        if (this.playerController == null)
            this.playerController = GetComponent<PlayerController>();
    }

    void Start()
    {
        //this.deplacementAxis += this.playerManager.PlayerID;
        //this.rotationAxis += this.playerManager.PlayerID;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.playerManager != null)
        {
            float yAxis = Input.GetAxis(this.deplacementAxis);
            float xAxis = Input.GetAxis(this.rotationAxis);
            
            this.playerController.UpdateMovementSpeed(yAxis);
            this.playerController.UpdateRotation(xAxis);
        }
    }
}
