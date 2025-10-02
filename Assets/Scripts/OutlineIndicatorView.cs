using UnityEngine;

public class OutlineIndicatorView : MonoBehaviour
{
    [SerializeField] private Outline _outline;

    public void ToggleOutline(bool toggle)
    {
        _outline.enabled = toggle;
    }
}