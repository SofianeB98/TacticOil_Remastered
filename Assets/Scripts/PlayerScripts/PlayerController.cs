using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Behavior")] 
    [SerializeField] private CharacterController controller;
    
    [Header("Movement")]
    [SerializeField] private Vector2 speedRange = new Vector2(0.0f, 25.0f);
    [SerializeField] private float currentSpeed = 0.0f;
    [SerializeField, Range(0.1f, 15.0f)] private float increaseSpeedRapidity = 2.0f;

    [Header("Rotation")] 
    [SerializeField, Range(0.1f, 15.0f)] private float rotationSpeed = 2.0f;
    [SerializeField, Range(0.1f, 15.0f)] private float increaseRotationRapidity = 2.0f;
    [SerializeField] private float currentRotate = 0.0f;
    
    void Start()
    {
        this.currentSpeed = 0.0f;
        this.currentRotate = 0.0f;
    }

    void Update()
    {
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, 
                                                Quaternion.Euler(0,this.currentRotate,0), 
                                                Time.deltaTime * this.rotationSpeed);

        this.controller.Move(this.transform.forward * this.currentSpeed * Time.deltaTime);
    }

    #region Movement & Rotation

    public void UpdateMovementSpeed(float dir)
    {
        if (this.currentSpeed >= 14.95f)
            this.currentSpeed = 15.0f;

        if (this.currentSpeed <= 0.05f)
            this.currentSpeed = 0.0f;
        
        this.currentSpeed = Mathf.Clamp(this.currentSpeed + Time.deltaTime * this.increaseSpeedRapidity * dir,
            this.speedRange.x, this.speedRange.y);
    }

    public void UpdateRotation(float dir)
    {
        if (this.currentRotate >= 360.0f)
            this.currentRotate -= 360.0f;

        if (this.currentRotate < 0.0f)
            this.currentRotate += 360.0f;
        
        this.currentRotate += Time.deltaTime * this.increaseRotationRapidity * dir;
    }
    
    #endregion
}
