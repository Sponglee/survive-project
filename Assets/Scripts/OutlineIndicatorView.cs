using UnityEngine;
using UnityEngine.Serialization;

public class OutlineIndicatorView : MonoBehaviour
{
    [SerializeField] private Outline outline;

    public void ToggleOutline(bool toggle)
    {
        outline.enabled = toggle;
    }
}