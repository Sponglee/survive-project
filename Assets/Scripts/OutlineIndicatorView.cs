using UnityEngine;

public class OutlineIndicatorView : MonoBehaviour
{
   [SerializeField] private Outline _outline;

   public void ToggleOutline(WorldTile obj)
   {
      _outline.enabled = obj != null && this.gameObject == obj.gameObject;
   }
}
