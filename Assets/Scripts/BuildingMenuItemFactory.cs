using Zenject;

namespace SurviveProject
{
    public class BuildingMenuItemFactory
    {
        private readonly DiContainer _container;
        private readonly UIElementsPreset _uiElementsPreset;
        private readonly BuildMenuView _buildMenuView;
        
        public BuildingMenuItemFactory(
            DiContainer container,
            UIElementsPreset uiElementsPreset,
            BuildMenuView buildMenuView)
        {
            _container = container;
            _uiElementsPreset = uiElementsPreset;
            _buildMenuView = buildMenuView;
        }
  
        public BuildingMenuItemView CreateItemView()
        {
            var prefab = _uiElementsPreset.buyMenuItemPrefab;
            var menuItem = _container.InstantiatePrefabForComponent<BuildingMenuItemView>(prefab);

            var menuItemHolder = _buildMenuView.BuildButtonsHolder;
            menuItem.transform.SetParent(menuItemHolder);
            return menuItem;
        }
    }
}