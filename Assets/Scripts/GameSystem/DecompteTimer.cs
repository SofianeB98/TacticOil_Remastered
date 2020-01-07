using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DecompteTimer : MonoBehaviour
{
    [Header("Parameter")] 
    [SerializeField] private float decompteTime = 3.0f;
    private float currentDecompteTimer = 0.0f;
    
    [Header("Events")] 
    [SerializeField] private FloatEvent OnSecondElapsed_DecompteTimer;
    [SerializeField] private UnityEvent OnTimeElapsed_DecompteTimer;
    
    // ------------------
    private Coroutine timer;
    private WaitForSeconds oneSecond;
    
    private void Awake()
    {
        this.oneSecond = new WaitForSeconds(1.0f);
    }
    
    #region Timer Functions

    public void StartDecompteTimer()
    {
        if(timer == null)
            timer = StartCoroutine(nameof(DecompteTimerClock));
    }

    public void StopDecompteTimer()
    {
        StopCoroutine(timer);
        timer = null;
    }

    #endregion
    
    #region Utils Functions
    
    public void SetDecompteTimer(float time)
    {
        this.currentDecompteTimer = time;
    }
    
    public void AddTimeToDecompteTimer(float time)
    {
        this.currentDecompteTimer += time;
    }

    public void RemoveTimeToDecompteTimer(float time)
    {
        this.currentDecompteTimer -= time;
    }

    public void ResetDecompteTimer()
    {
        this.currentDecompteTimer = this.decompteTime;
    }
    
    #endregion
    
    private IEnumerator DecompteTimerClock()
    {
        while (this.currentDecompteTimer > 0.0f)
        {
            yield return oneSecond;
            this.currentDecompteTimer -= 1.0f;
            this.OnSecondElapsed_DecompteTimer.Invoke(this.currentDecompteTimer);
        }
        
        this.OnTimeElapsed_DecompteTimer.Invoke();
        
        yield break;
    }
}
