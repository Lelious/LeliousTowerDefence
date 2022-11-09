using Zenject;

public class UiInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SelectedFrame>().FromComponentInNewPrefabResource(AssetPath.SelectedFrame).AsSingle().NonLazy();
        Container.Bind<InputService>().FromComponentInNewPrefabResource(AssetPath.InputService).AsSingle().NonLazy();
        Container.Bind<GameInformationMenu>().FromComponentInNewPrefabResource(AssetPath.GameInformationMenu).AsSingle().NonLazy();
        Container.Bind<TopMenuInformator>().FromComponentInNewPrefabResource(AssetPath.TopMenuInformator).AsSingle().NonLazy();
    }
}
