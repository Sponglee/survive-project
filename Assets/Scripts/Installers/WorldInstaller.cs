using Zenject;

namespace DefaultNamespace.Installers
{
    public class WorldInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<WorldGenerator>().AsSingle().NonLazy();
            Container.Bind<TileManager>().AsSingle().NonLazy();
            
            Container.Bind<ITileFactory>()
                .To<WorldTileFactory>()
                .AsSingle();
            
            Container.Bind<TileInputService>().AsSingle().NonLazy();

            Container.Bind<TileOutlineService>().AsSingle().NonLazy();

        }
    }
}