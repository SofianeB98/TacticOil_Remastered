using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

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
    
    private float currentXAngle = 45.0f;
    private float currentYAngle = 0.0f;
    
    void Start()
    {
        this.playerCam.SetParent(null);
    }

    void Update()
    {
        if (this.playerCam != null)
        {
            this.playerCam.position = this.player.position + this.playerCam.rotation *
                                      new Vector3(0, 0, -this.distanceFromPlayer);
        }
    }

    public void UpdateCameraAngle(Vector3 dir)
    {
        this.currentXAngle = this.currentXAngle +
                             dir.x * this.speedRotate * this.angleXSpeed * Time.deltaTime * (this.inverseX ? -1 : 1);
        
        this.currentYAngle = this.currentYAngle +
                             dir.y * this.speedRotate * this.angleYSpeed * Time.deltaTime * (this.inverseY ? -1 : 1);
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
