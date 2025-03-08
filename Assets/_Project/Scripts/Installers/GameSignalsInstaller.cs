using Events;
using Zenject;

namespace Installers
{
    public class GameSignalsInstaller : Installer<GameSignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            InstallUISignals();
        }

        private void InstallUISignals()
        {
            Container.DeclareSignal<SetActiveTimerUISignal>().OptionalSubscriber();
            Container.DeclareSignal<SetActiveProductPopupSignal>().OptionalSubscriber();
            Container.DeclareSignal<SetActiveBuildingItemsPopupSignal>().OptionalSubscriber();
            Container.DeclareSignal<InstantiateConstructionSignal>().OptionalSubscriber();
            Container.DeclareSignal<CollectedProductSignal>().OptionalSubscriber();
        }
    }
}