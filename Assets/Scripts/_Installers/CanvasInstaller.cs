using UnityEngine;
using Zenject;

namespace SurviveProject.Installers
{
    public class CanvasInstaller : MonoInstaller
    {
        [SerializeField] private BuildMenuView _buildMenuCanvas;
        
        public override void InstallBindings()
        {
            Container.Bind<BuildMenuView>().FromInstance(_buildMenuCanvas).AsSingle();    
        }
    }
}