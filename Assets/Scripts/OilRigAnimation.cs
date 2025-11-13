using UnityEngine;

public class OilRigAnimation : MonoBehaviour
{
    [SerializeField] private Transform hammer;
    
    [Header("Hammer Settings")]
    [SerializeField] private float hammerSpeed = 2f;
    [SerializeField] private float hammerAngle = 30f;
    
    private float hammerTime = 0f;
    private float initialHammerRotationX;
    
    private void Start()
    {
        if (hammer != null)
        {
            initialHammerRotationX = hammer.localRotation.eulerAngles.x;
        }
    }

    private void Update()
    {
        if (hammer == null)
        {
            return;
        }
        
        hammerTime += Time.deltaTime * hammerSpeed;
        var oscillation = Mathf.Sin(hammerTime) * hammerAngle;
        var newXRotation = initialHammerRotationX + oscillation;
        hammer.localRotation = Quaternion.Euler(newXRotation, 0f, 0f);
    }
}
