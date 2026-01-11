using UnityEngine;

public class BuildingView : MonoBehaviour
{
    [SerializeField] private OutlineIndicatorView outlineIndicatorView;

    public void SetIsBuildable(bool valid)
    {
       outlineIndicatorView.ToggleOutline(!valid);
    }
}