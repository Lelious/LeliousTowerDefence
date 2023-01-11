using Infrastructure.StateMachine;
using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private StartPoint _startPoint;
    [SerializeField] private EndPoint _endPoint;

    public override void InstallBindings()
    {
        BindEnemyFactory();
        BindGameManager();
        BindStateMachine();
        BindTowerFactory();
        BindStartPoint();
        BindEndPoint();
        BindEnemyPool();
        BindTapRegistrator();
        BindSelectedFrame();
        BindPool();
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

    private void BindStartPoint()
    {
        Container.
            Bind<StartPoint>().
            FromInstance(_startPoint).
            AsSingle();
    }
    private void BindEndPoint()
    {
        Container.
            Bind<EndPoint>().
            FromInstance(_endPoint).
            AsSingle();
    }

    private void BindTowerFactory()
    {
        Container.
            BindInterfacesAndSelfTo<TowerFactory>().
            AsSingle();
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
}