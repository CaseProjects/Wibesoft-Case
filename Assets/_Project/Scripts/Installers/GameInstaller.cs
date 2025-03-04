using MainHandlers;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            GameSignalsInstaller.Install(Container);
            InstallMainBehaviors();
            InstallPlayerSettings();
        }


        private void InstallMainBehaviors()
        {
            Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle();
            Container.Bind<GameObservables>().AsSingle();
        }

        private void InstallPlayerSettings()
        {
        }

    }
}