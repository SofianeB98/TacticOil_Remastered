using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructorSelectedManager : MonoBehaviour
{
    [Header("Building Visual")] 
    [SerializeField] private GameObject cityBuilding;
    [SerializeField] private GameObject mastoBuilding;
    [SerializeField] private GameObject centraleBuilding;
    [SerializeField] private GameObject canonBuilding;

    [Header("Position information")] 
    [SerializeField] private float yOffset = 2.0f;
    public float YOffset => yOffset;

    public void EnableSelectedBuilding(BuildingType type)
    {
        cityBuilding.SetActive(false);
        mastoBuilding.SetActive(false);
        centraleBuilding.SetActive(false);
        canonBuilding.SetActive(false);
        
        switch (type)
        {
            case BuildingType.City:
                cityBuilding.SetActive(true);
                break;
            
            case BuildingType.Mastodonte:
                mastoBuilding.SetActive(true);
                break;
            
            case BuildingType.Centrale:
                centraleBuilding.SetActive(true);
                break;
            
            case BuildingType.Canon:
                canonBuilding.SetActive(true);
                break;
        }
    }

    public void SetPosition(Vector3 pos)
    {
        this.transform.localPosition = pos;
    }
}
