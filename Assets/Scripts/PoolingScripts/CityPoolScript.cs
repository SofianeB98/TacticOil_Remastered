using UnityEngine;

public class CityPoolScript : MonoBehaviour
{
    public static CityPoolScript Instance;
    
    [Header("Pool Entity")] 
    [SerializeField] private CityBuilding poolEntity;
    
    [Header("Pool Size")] 
    [SerializeField] private int poolSize = 40;

    private CityBuilding[] pool = new CityBuilding[0];

    private void Awake()
    {
        CityPoolScript.Instance = this;
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

    public CityBuilding WakeUp()
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
        
        this.pool = new CityBuilding[this.poolSize];
    }
}
