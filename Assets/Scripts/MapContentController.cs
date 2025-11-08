
namespace SurviveProject
{
    public class MapContentController : IMapContent
    {
        private MapContentView _contentView;
        private MapContentModel _model;

        public MapContentType MapContentType => _model.MapContentType;
        
        public MapContentController(MapContentView contentView, MapContentModel model)
        {
            _contentView = contentView;
            _model = model;
        }
    }
}