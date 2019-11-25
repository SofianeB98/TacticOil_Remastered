using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform player;
    
    [Header("Position Information")] 
    [SerializeField] private float distanceFromPlayer = 10.0f;
    
    [Header("Rotation Information")]
    [SerializeField] private Vector2 angleXLimits = new Vector2(75.0f, 20.0f);
    [SerializeField, Range(2.0f, 20.0f)] private float speedRotate = 5.0f;
    [SerializeField, Range(1.0f, 100.0f)] private float angleXSpeed = 25.0f;
    [SerializeField] private bool inverseX = false;
    [SerializeField, Range(1.0f, 100.0f)] private float angleYSpeed = 25.0f;
    [SerializeField] private bool inverseY = false;

    [Header("Constructor Information")] 
    [SerializeField] private float yOffset = 5.0f;
    
    private float currentXAngle = 45.0f;
    private float currentYAngle = 0.0f;
    
    private void Start()
    {
        this.playerCam.SetParent(null);
    }

    public void UpdateCustom()
    {
        if (this.playerCam != null)
        {
            switch (this.playerManager.CurrentMode)
            {
                case PlayerMode.Normal:
                    this.playerCam.rotation =  Quaternion.Euler(this.currentXAngle, this.currentYAngle, 0.0f);
            
                    this.playerCam.position = this.player.position + this.playerCam.rotation *
                                              new Vector3(0, 0, -this.distanceFromPlayer);
                    break;
                
                case PlayerMode.Constructor:
                    // --- To be update 
                    this.playerCam.position = Vector3.Lerp(this.playerCam.position,
                        this.player.position + new Vector3(0.0f, this.yOffset, 0.0f), Time.deltaTime*5.0f);
                    this.playerCam.rotation = Quaternion.Lerp(this.playerCam.rotation, Quaternion.Euler(90.0f, 0.0f, 0.0f), Time.deltaTime * 7.0f);
                    break;
            }
        }
    }

    public void UpdateCameraAngle(Vector3 dir)
    {
        this.currentXAngle = this.currentXAngle +
                             dir.x * this.speedRotate * this.angleXSpeed * Time.deltaTime * (this.inverseX ? -1 : 1);
        this.AngleXVerification();
        
        this.currentYAngle = this.currentYAngle +
                             dir.y * this.speedRotate * this.angleYSpeed * Time.deltaTime * (this.inverseY ? -1 : 1);
        this.AngleYVerification();
    }

    private void AngleYVerification() 
    {
        if (this.currentYAngle > 360) 
        {
            this.currentYAngle = this.currentYAngle - 360;
        } 
        else if(this.currentYAngle < 0) 
        {
            this.currentYAngle = this.currentYAngle + 360;
        }
    }
	
    private void AngleXVerification() 
    {
        if (this.currentXAngle > this.angleXLimits.x) 
        {
            this.currentXAngle = this.angleXLimits.x;
        } 
        else if(this.currentXAngle < this.angleXLimits.y) 
        {
            this.currentXAngle = this.angleXLimits.y;
        }
    }
    
}
