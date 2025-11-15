
using System.Collections.Generic;

namespace SurviveProject
{
    public class BuildMenuModel
    {
        public List<BuildingMenuItemController> BuildingMenuItems { get; } = new();
        
        public BuildMenuModel(List<BuildingMenuItemController> buildingMenuItems)
        {
            foreach (var item in buildingMenuItems)
            {
                BuildingMenuItems.Add(item);
            }
        }
    }
}