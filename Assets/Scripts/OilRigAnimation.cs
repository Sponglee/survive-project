using UnityEngine;

public class OilRigAnimation : MonoBehaviour
{
    [SerializeField] private Transform hammer;
    
    [Header("Hammer Settings")]
    [SerializeField] private float hammerSpeed = 2f;
    [SerializeField] private float hammerAngle = 30f;
    
    private float _hammerTime = 0f;
    private float _initialHammerRotationX;
    
    private void Start()
    {
        if (hammer != null)
        {
            _initialHammerRotationX = hammer.localRotation.eulerAngles.x;
        }
    }

    private void Update()
    {
        if (!hammer)
        {
            return;
        }
        
        _hammerTime += Time.deltaTime * hammerSpeed;
        var oscillation = Mathf.Sin(_hammerTime) * hammerAngle;
        var newXRotation = _initialHammerRotationX + oscillation;
        hammer.localRotation = Quaternion.Euler(newXRotation, 0f, 0f);
    }
}
