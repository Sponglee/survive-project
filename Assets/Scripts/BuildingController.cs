public class BuildingController
{
    private BuildingModel _model;
    private BuildingView _view;
    
    public BuildingController(BuildingView buildingView, BuildingModel buildingModel)
    {
        _model = buildingModel;
        _view = buildingView;
    }
}