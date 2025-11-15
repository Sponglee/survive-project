using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurviveProject
{
    public class BuildingMenuItemView : MonoBehaviour
    {
        [SerializeField] private Button buyButton;
        [SerializeField] private TextMeshProUGUI buildingText;
        
        public Button BuyButton => buyButton;

        public void SetData(BuildingData modelBuildingData)
        {
            buildingText.text = modelBuildingData.Name;
        }
    }
}