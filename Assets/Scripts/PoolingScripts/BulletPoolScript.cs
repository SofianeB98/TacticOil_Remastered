using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolScript : MonoBehaviour
{
    public static BulletPoolScript Instance;
    
    [Header("Pool Entity")] 
    [SerializeField] private BulletScript poolEntity;
    
    [Header("Pool Size")] 
    [SerializeField] private int poolSize = 30;

    private BulletScript[] pool = new BulletScript[0];

    private void Awake()
    {
        BulletPoolScript.Instance = this;
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

    public BulletScript WakeUp()
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
        
        this.pool = new BulletScript[this.poolSize];
    }
}
