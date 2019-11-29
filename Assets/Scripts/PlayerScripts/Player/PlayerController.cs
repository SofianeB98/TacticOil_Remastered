using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Behavior")] 
    [SerializeField] private CharacterController controller;
    [SerializeField] private PlayerManager playerManager;
    
    [Header("Movement")]
    [SerializeField] private Vector2 speedRange = new Vector2(0.0f, 25.0f);
    [SerializeField] private float currentSpeed = 0.0f;
    [SerializeField, Range(0.1f, 15.0f)] private float increaseSpeedRapidity = 2.0f;
    public float CurrentSpeed => currentSpeed;
    
    [Header("Collision Force")]
    [SerializeField, Range(0.01f, 0.99f)] private float loseSpeedPercentAfterCollision = 0.95f;
    [SerializeField] private float loseForceSpeedReachMax = 1.0f;
    [SerializeField] private AnimationCurve forceCurve;
    private Coroutine decreaseForceCoroutine;
    private Vector3 collisionForceVelocity = Vector3.zero;
    
    [Header("Rotation")] 
    [SerializeField, Range(0.1f, 90.0f)] private float rotationSpeed = 2.0f;
    [SerializeField, Range(0.1f, 90.0f)] private float increaseRotationRapidity = 2.0f;
    [SerializeField] private float currentRotate = 0.0f;
    
    private void Start()
    {
        this.currentSpeed = 0.0f;
        this.currentRotate = 0.0f;
    }

    public void UpdateCustom()
    {
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, 
                                                Quaternion.Euler(0,this.currentRotate,0), 
                                                Time.deltaTime * this.rotationSpeed);

        this.controller.Move((this.transform.forward* (this.currentSpeed*this.playerManager.GetWeightCoeff()) + this.collisionForceVelocity) * Time.deltaTime);
    }

    #region Movement & Rotation

    public void UpdateMovementSpeed(float dir)
    {
        this.currentSpeed = Mathf.Clamp(this.currentSpeed + Time.deltaTime * this.increaseSpeedRapidity * dir,
            this.speedRange.x, this.speedRange.y);
        
        if (this.currentSpeed >= this.speedRange.y - 0.01f)
            this.currentSpeed = this.speedRange.y;

        if (this.currentSpeed <= this.speedRange.x + 0.01f)
            this.currentSpeed = this.speedRange.x;
    }

    public void DecreaseSpeedAfterCollision()
    {
        this.currentSpeed *= (1 - this.loseSpeedPercentAfterCollision);
    }

    public void ApplyForce(Vector3 velocity, float weight)
    {
        if(this.decreaseForceCoroutine != null)
            StopCoroutine(this.decreaseForceCoroutine);
        
        this.collisionForceVelocity += velocity * weight / this.playerManager.CurrentWeight;
        
        this.decreaseForceCoroutine = StartCoroutine(DecreaseCollisionForce());
    }

    private IEnumerator DecreaseCollisionForce()
    {
        float timer = 0.0f;
        while (!(timer >= 1.0f))
        {
            timer += Time.deltaTime/ this.loseForceSpeedReachMax;
            this.collisionForceVelocity *= this.forceCurve.Evaluate(timer + Time.deltaTime / this.loseForceSpeedReachMax) - this.forceCurve.Evaluate(timer);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        this.collisionForceVelocity = Vector3.zero;
        
        this.decreaseForceCoroutine = null;
        
        yield break;
    }
    
    public void UpdateRotation(float dir)
    {
        if (this.currentRotate >= 360.0f)
            this.currentRotate -= 360.0f;

        if (this.currentRotate < 0.0f)
            this.currentRotate += 360.0f;
        
        this.currentRotate += Time.deltaTime * this.increaseRotationRapidity/(this.currentSpeed >= 1 ? this.currentSpeed : 1) * this.playerManager.GetWeightCoeff() * dir;
    }
    
    #endregion
}
