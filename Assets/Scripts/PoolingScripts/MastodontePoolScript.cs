using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MastodontePoolScript : MonoBehaviour
{
    public static MastodontePoolScript Instance;
    
    [Header("Pool Entity")] 
    [SerializeField] private MastodonteBuilding poolEntity;
    
    [Header("Pool Size")] 
    [SerializeField] private int poolSize = 40;

    private MastodonteBuilding[] pool = new MastodonteBuilding[0];

    private void Awake()
    {
        MastodontePoolScript.Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        ResetPool();
        for (int i = 0; i < this.poolSize; i++)
        {
            this.pool[i] = Instantiate(this.poolEntity);
            this.pool[i].Sleep(false);
        }
    }

    public MastodonteBuilding WakeUp()
    {
        for (int i = 0; i < this.poolSize; i++)
        {
            if(this.pool[i].gameObject.activeSelf)
                continue;

            return this.pool[i];
        }
        
        return null;
    }

    public void ResetPool()
    {
        if (this.pool.Length > 0)
        {
            for (int i = 0; i < this.poolSize; i++)
            {
                Destroy(this.pool[i].gameObject);
            }
        }
        
        this.pool = new MastodonteBuilding[this.poolSize];
    }
}
