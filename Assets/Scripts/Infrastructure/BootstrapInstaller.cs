using Infrastructure.StateMachine;
using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private TowerBuilder _towerBuilder;

    public override void InstallBindings()
    {
        BindEnemyFactory();
        BindGameManager();
        BindTowerBuilder();
        BindStateMachine();
    }

    private void BindStateMachine()
    {
        Container.
            BindInterfacesAndSelfTo<GameLoopStateMachine>().
            AsSingle();
    }

    private void BindTowerBuilder()
    {
        Container.
           Bind<TowerBuilder>().
           FromInstance(_towerBuilder).
           AsSingle();
    }

    private void BindGameManager()
    {
        Container.Bind<GameManager>()
            .FromComponentInNewPrefabResource(AssetPath.GameManager)
            .AsSingle()
            .NonLazy();
    }

    private void BindEnemyFactory()
    {
        Container
            .BindInterfacesAndSelfTo<EnemyFactory>()
            .AsSingle();
    }
}