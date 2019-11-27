using UnityEngine;

public class CanonPoolScript : MonoBehaviour
{
    public static CanonPoolScript Instance;
    
    [Header("Pool Entity")] 
    [SerializeField] private CanonBuilding poolEntity;
    
    [Header("Pool Size")] 
    [SerializeField] private int poolSize = 30;

    private CanonBuilding[] pool = new CanonBuilding[0];

    private void Awake()
    {
        CanonPoolScript.Instance = this;
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

    public CanonBuilding WakeUp()
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
        
        this.pool = new CanonBuilding[this.poolSize];
    }
}
