using UnityEngine;

public class RoadElementView : MonoBehaviour
{
    [SerializeField] private LineRenderer roadRenderer;
    
    private void Awake()
    {
        if (roadRenderer != null)
        {
            // Use world space so positions are absolute
            roadRenderer.useWorldSpace = true;
        }
    }
    
    public void SetConnectionPoints(Vector3 startPoint, Vector3 endPoint)
    {
        if (roadRenderer == null)
        {
            return;
        }
        
        roadRenderer.positionCount = 2;
        roadRenderer.SetPosition(0, startPoint);
        roadRenderer.SetPosition(1, endPoint);
    }
    
    public void UpdateConnectionPoints(Vector3 startPoint, Vector3 endPoint)
    {
        SetConnectionPoints(startPoint, endPoint);
    }
}