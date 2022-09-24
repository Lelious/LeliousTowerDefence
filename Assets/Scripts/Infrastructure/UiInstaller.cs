using Infrastructure.Services.Input;
using Zenject;

public class UiInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IInputService>().FromComponentInNewPrefabResource(AssetPath.InputService).AsSingle().NonLazy();
        Container.Bind<BottomMenuInformator>().FromComponentInNewPrefabResource(AssetPath.BottomMenuInformator).AsSingle().NonLazy();
        Container.Bind<TopMenuInformator>().FromComponentInNewPrefabResource(AssetPath.TopMenuInformator).AsSingle().NonLazy();
        Container.Bind<BottomBuildingMenu>().FromComponentInNewPrefabResource(AssetPath.BottomBuildingMenu).AsSingle().NonLazy();
        Container.Bind<SelectedFrame>().FromComponentInNewPrefabResource(AssetPath.SelectedFrame).AsSingle().NonLazy();     
    }
}
