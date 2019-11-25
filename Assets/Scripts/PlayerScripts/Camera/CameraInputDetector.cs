using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInputDetector : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerManager playerManager;
    
    [Header("Rotation Input")] 
    [SerializeField] private string rotationXAxis = "Mouse X";
    [SerializeField] private string rotationYAxis = "Mouse Y";
    [SerializeField, Range(0.01f, 1.0f)] private float deadZone = 0.1f;
    
    
    private void Awake()
    {
        if (this.cameraController == null)
            this.cameraController = GetComponent<CameraController>();

        if (this.playerManager == null)
            this.playerManager = GetComponent<PlayerManager>();
    }

    public void UpdateCustom()
    {
        if (this.cameraController != null && this.playerManager != null)
        {
            switch (this.playerManager.CurrentMode)
            {
                case PlayerMode.Normal:
                    float xAxis = Input.GetAxis(rotationXAxis);
                    float yAxis = Input.GetAxis(rotationYAxis);
            
                    if (Mathf.Abs(xAxis) > this.deadZone || Mathf.Abs(yAxis) > this.deadZone )
                    {
                        this.cameraController.UpdateCameraAngle(new Vector3(yAxis, xAxis, 0));
                    }
                    break;
                
                case PlayerMode.Constructor:
                    // --
                    break;
            }
            
            
        }
    }
}
