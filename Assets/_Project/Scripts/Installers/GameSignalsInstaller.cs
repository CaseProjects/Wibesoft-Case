using Events;
using Zenject;

namespace Installers
{
    public class GameSignalsInstaller : Installer<GameSignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            InstallPlayerSignals();
            InstallUtiliesSignal();
        }


        private void InstallPlayerSignals()
        {
            Container.DeclareSignalWithInterfaces<SignalChangeState>().OptionalSubscriber();
        }

        private void InstallUtiliesSignal()
        {
            Container.DeclareSignal<SignalSaveLevel>().OptionalSubscriber();
        }
    }
}