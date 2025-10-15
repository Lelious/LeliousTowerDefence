using Infrastructure.StateMachine;
using System;
using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private ParentedCamera _camera;

    public override void InstallBindings()
    {
        BindEnemyPrefabLoader();
        BindEnemyFactory();
        BindGameManager();
        BindStateMachine();
        BindTowerFactory();
        BindEnemyPool();
        BindTapRegistrator();
        BindSelectedFrame();
        BindPool();
        BindBuffService();
        BindGameBootstraper();
        BindCamera();
    }

    private void BindCamera()
    {
        Container.
            Bind<ParentedCamera>().
            FromInstance(_camera).
            AsSingle().
            NonLazy();
    }

    private void BindGameBootstraper()
    {
        Container.
            BindInterfacesAndSelfTo<GameBootstrapper>().
            AsSingle();
    }

    private void BindTapRegistrator()
    {
        Container.
            BindInterfacesAndSelfTo<TapRegisterService>().
            AsSingle();
    }

    private void BindEnemyPool()
    {
        Container.
            BindInterfacesAndSelfTo<EnemyPool>().
            AsSingle();
    }

    private void BindTowerFactory()
    {
        Container.
            BindInterfacesAndSelfTo<TowerFactory>().
            AsSingle().NonLazy();
    }

    private void BindStateMachine()
    {
        Container.
            BindInterfacesAndSelfTo<GameLoopStateMachine>().
            AsSingle();
    }

    private void BindGameManager()
    {
        Container.
            Bind<GameManager>().
            FromComponentInNewPrefabResource(AssetPath.GameManager).
            AsSingle().
            NonLazy();
    }

    private void BindEnemyFactory()
    {
        Container.
            BindInterfacesAndSelfTo<EnemyFactory>().
            AsSingle();
    }

    private void BindSelectedFrame()
    {
        Container.
            Bind<SelectedFrame>().
            FromComponentInNewPrefabResource(AssetPath.SelectedFrame).
            AsSingle().
            NonLazy();
    }

    private void BindPool()
    {
        Container.
             BindInterfacesAndSelfTo<PoolService>().
             AsSingle().
             NonLazy();
    }

    private void BindBuffService()
    {
        Container.
            BindInterfacesAndSelfTo<BuffService>().
            AsSingle();
    }
    private void BindEnemyPrefabLoader()
    {
        Container.
            Bind<EnemyLoadService>().
            AsSingle();
    }
}