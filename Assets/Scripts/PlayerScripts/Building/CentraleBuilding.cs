using UnityEngine;

public class CentraleBuilding : BuildingClass
{
    [SerializeField] private float explodeDamage = 20.0f;

    public override void Sleep(bool isDestroyed)
    {
        if (isDestroyed)
        {
            if (this.leftNeighbour != null)
                this.leftNeighbour.ApplyDamage(this.explodeDamage);

            if (this.rightNeighbour != null)
                this.rightNeighbour.ApplyDamage(this.explodeDamage);
        
            if(this.topNeighbour != null)
                this.topNeighbour.ApplyDamage(this.explodeDamage);
        
            if(this.bottomNeighbour != null)
                this.bottomNeighbour.ApplyDamage(this.explodeDamage);
        }        
        
        base.Sleep(isDestroyed);
    }
}