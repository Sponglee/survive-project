using System;
using SurviveProject;
using UnityEngine;

public class RoadView : BuildingView
{
    [SerializeField] private RoadVariant[] roadVariants;
    [SerializeField] private Transform rotationPivot;
    
    public void SetRoadType(RoadsService.RoadType roadType, float rotation)
    {
        foreach (var variant in roadVariants)
        {
            if (variant?.RoadObject != null)
            {
                variant.RoadObject.SetActive(false);
            }
        }

        var matchingVariant = Array.Find(roadVariants, v => v.RoadType == roadType);
        
        if (matchingVariant?.RoadObject != null)
        {
            matchingVariant.RoadObject.SetActive(true);
            
            rotationPivot.transform.localRotation = Quaternion.Euler(0f, rotation, 0f);
        }
    }
    
    [Serializable]
    public class RoadVariant
    {
        public RoadsService.RoadType RoadType;
        public GameObject RoadObject;
    }
}
