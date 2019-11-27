using UnityEngine;

public class ForeusePoolScript : MonoBehaviour
{
    public static ForeusePoolScript Instance;
    
    [Header("Pool Entity")] 
    [SerializeField] private ForeuseBuilding poolEntity;
    
    [Header("Pool Size")] 
    [SerializeField] private int poolSize = 12;

    private ForeuseBuilding[] pool = new ForeuseBuilding[0];

    private void Awake()
    {
        ForeusePoolScript.Instance = this;
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

    public ForeuseBuilding WakeUp()
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
        
        this.pool = new ForeuseBuilding[this.poolSize];
    }
}
