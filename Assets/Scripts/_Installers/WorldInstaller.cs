using System.Security;
using Zenject;

namespace SurviveProject.Installers
{
    public class WorldInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<WorldGenerator>().AsSingle().NonLazy();
            Container.Bind<TileManager>().AsSingle().NonLazy();
            
            Container.Bind<ITileFactory>().To<WorldTileFactory>().AsSingle();
            
            Container.Bind<MapContentItemFactory>().AsSingle();
            
            Container.Bind<BuildingFactory>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<TileInputService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<TileInputProvider>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<BuildingService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BuildingProvider>().AsSingle().NonLazy();
            
            Container.Bind<TileOutlineService>().AsSingle().NonLazy();

        }
    }
}