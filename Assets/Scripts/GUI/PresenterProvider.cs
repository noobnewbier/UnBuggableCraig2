using TurretGUI;
using DialogueGUI;
using Engine.Support.EventAggregator;
using HealthBarGUI;

public static class PresenterProvider
{
    public static ITurretPanelPresenter GetTurretPanelPresenter(ITurretPanelView view)
    {
        return new TurretPanelPresenter(
                new TurretModel(ServiceLocator.Instance.ShopManager.AvailableTurretsPrefabs),
                ServiceLocator.Instance.ShopManager,
                ServiceLocator.Instance.TurretPlacer,
                view
            );
    }

    public static IDialoguePresenter GetDialoguePresenter(IDialogueView view)
    {
        return new DialoguePresenter(
                view,
                ServiceLocator.EventAggregator
            );
    }

    public static IHealthBarPresenter GetHealthBarPresenter(IHealthBarView view)
    {
        return new HealthBarPresenter(
            view,
            ServiceLocator.EventAggregator
            );
    }

}
