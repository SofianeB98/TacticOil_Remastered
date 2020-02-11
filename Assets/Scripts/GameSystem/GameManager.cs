using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<PlayerManager> playersInLive;
    
    [SerializeField] private UnityEvent onGameStart;
    [SerializeField] private UnityEvent onGameIsStarted;
    [SerializeField] private UnityEvent onGameEnd;
    
    private bool isGameStarted = false;
    
    public void StartGame()
    {
        this.onGameStart.Invoke();
    }

    public void SetGameStarted(bool value)
    {
        this.isGameStarted = value;
    }
    
    public void StopGame()
    {
        this.onGameEnd.Invoke();
    }

    public void RegisterPlayer(PlayerManager player)
    {
        if(this.playersInLive.Contains(player))
            return;
        
        this.playersInLive.Add(player);
    }
    
    public void OnPlayerDie(PlayerManager player)
    {
        if(!this.playersInLive.Contains(player))
            return;
        
        this.playersInLive.Remove(player);
        
        if(this.playersInLive.Count == 1)
            StopGame();
    }
}
