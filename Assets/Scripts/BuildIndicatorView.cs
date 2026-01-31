using UnityEngine;

namespace SurviveProject
{
    public class BuildIndicatorView : MonoBehaviour
    {
        [SerializeField] private Transform buildingIndicatorPanel;
        
        public Transform BuildingIndicatorPanel => buildingIndicatorPanel;

        public void ToggleIndicator(bool isIndicatorActive)
        {
           buildingIndicatorPanel.gameObject.SetActive(isIndicatorActive);
        }
    }
}