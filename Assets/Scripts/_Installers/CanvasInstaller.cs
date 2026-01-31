using UnityEngine;
using Zenject;

namespace SurviveProject.Installers
{
    public class CanvasInstaller : MonoInstaller
    {
        [SerializeField] private BuildMenuView _buildMenuCanvas;
        [SerializeField] private BuildIndicatorView _builIndicatorView;

        public override void InstallBindings()
        {
            Container.Bind<BuildMenuView>().FromInstance(_buildMenuCanvas).AsSingle();    
            Container.Bind<BuildIndicatorView>().FromInstance(_builIndicatorView).AsSingle();    

        }
    }
}