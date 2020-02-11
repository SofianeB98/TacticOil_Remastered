using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Structure : MonoBehaviour
{
    [Header("Capture Parameter")]
    [SerializeField] private float captureSpeed = 5.0f;
    [SerializeField] private float unacaptureSpeed = 10.0f;
    [SerializeField] private float captureTarget = 100.0f;

    [Header("Event")] 
    [SerializeField] private PlayerManagerEvent onStructureCaptured;
    
    private List<PlayerManager> capturedByThesePlayers;
    private float currentPercentCapture = 0.0f;
    private float step = 0.0f;
    
    private void Update()
    {
        if (this.capturedByThesePlayers.Count != 1)
            this.step = this.step > 0.0f ? this.step - Time.deltaTime * this.unacaptureSpeed / this.captureTarget : 0.0f;
        else
            this.step = this.step < 1.0f ? this.step + Time.deltaTime * this.captureSpeed / this.captureTarget : 1.0f;
        
        this.currentPercentCapture = Mathf.Lerp(0.0f, this.captureTarget, this.step);
        
        if (this.currentPercentCapture >= this.captureTarget)
        {
            this.onStructureCaptured.Invoke(this.capturedByThesePlayers[0]);
        }
        
    }

    #region Capture Detection

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager pm = other.GetComponent<PlayerManager>() ?? other.GetComponentInParent<PlayerManager>();
        
        if(pm != null)
            if (!this.capturedByThesePlayers.Contains(pm))
                this.capturedByThesePlayers.Add(pm);
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerManager pm = other.GetComponent<PlayerManager>() ?? other.GetComponentInParent<PlayerManager>();
        
        if(pm != null)
            if (this.capturedByThesePlayers.Contains(pm))
                this.capturedByThesePlayers.Remove(pm);
    }

    #endregion

}
