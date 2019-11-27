using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralePoolScript : MonoBehaviour
{
    public static CentralePoolScript Instance;
    
    [Header("Pool Entity")] 
    [SerializeField] private CentraleBuilding poolEntity;
    
    [Header("Pool Size")] 
    [SerializeField] private int poolSize = 40;

    private CentraleBuilding[] pool = new CentraleBuilding[0];

    private void Awake()
    {
        CentralePoolScript.Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        ResetPool();
        for (int i = 0; i < this.poolSize; i++)
        {
            this.pool[i] = Instantiate(this.poolEntity);
            this.pool[i].Sleep();
        }
    }

    public CentraleBuilding WakeUp()
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
        
        this.pool = new CentraleBuilding[this.poolSize];
    }
}
