using Zenject;

public class UiInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindInputService();
        BindGameUIService();
        BindTopMenuInformator();
    }

    private void BindInputService()
    {
        Container.
            Bind<InputService>().
            FromComponentInNewPrefabResource(AssetPath.InputService).
            AsSingle().
            NonLazy();
    }

    private void BindGameUIService()
    {
        Container.
            Bind<GameUIService>().
            FromComponentInNewPrefabResource(AssetPath.GameUIService).
            AsSingle().
            NonLazy();
    }

    private void BindTopMenuInformator()
    {
        Container.
            Bind<TopMenuInformator>().
            FromComponentInNewPrefabResource(AssetPath.TopMenuInformator).
            AsSingle().
            NonLazy();
    }
}
