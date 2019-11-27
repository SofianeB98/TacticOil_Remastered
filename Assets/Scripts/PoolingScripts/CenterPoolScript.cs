using UnityEngine;

public class CenterPoolScript : MonoBehaviour
{
    public static CenterPoolScript Instance;
    
    [Header("Pool Entity")] 
    [SerializeField] private CenterBuilding poolEntity;
    
    [Header("Pool Size")] 
    [SerializeField] private int poolSize = 6;

    private CenterBuilding[] pool = new CenterBuilding[0];

    private void Awake()
    {
        CenterPoolScript.Instance = this;
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

    public CenterBuilding WakeUp()
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
        
        this.pool = new CenterBuilding[this.poolSize];
    }
}
