using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    
    [Header("Bullet Information")] 
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float givenDamages = 20.0f;
    [SerializeField] private float expulseForce = 30.0f;

    [Header("Detection Information")] 
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string videTag = "Vide";

    private void Awake()
    {
        if (this.rb == null)
            this.rb = GetComponent<Rigidbody>();
    }
    
    #region Pooling

    public void WakeUp(PlayerManager playerManager, Vector3 position, Quaternion rotation)
    {
        this.playerManager = playerManager;

        this.transform.position = position;
        this.transform.rotation = rotation;
        
        this.gameObject.SetActive(true);
    }

    public void Sleep()
    {
        this.playerManager = null;
        
        this.rb.velocity = Vector3.zero;
        this.rb.angularVelocity = Vector3.zero;
        
        this.gameObject.SetActive(false);
    }

    #endregion

    public void AddForce(Vector3 dir)
    {
        this.rb.AddForce(dir.normalized * this.expulseForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains(this.playerTag) && !other.tag.Contains(this.playerManager.PlayerID.ToString()))
        {
            BuildingClass bc = other.GetComponent<BuildingClass>();
            if (bc != null)
            {
                bc.ApplyDamage(this.givenDamages);
                this.Sleep();
            }
        }
        else if (other.tag.Contains(this.videTag))
        {
            this.Sleep();
        }
        
    }
}
