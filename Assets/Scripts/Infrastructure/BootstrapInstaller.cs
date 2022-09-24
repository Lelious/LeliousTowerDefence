using Infrastructure.StateMachine;
using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private BuildCellInitializer _cellInitializer;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TowerBuilder _towerBuilder;

    public override void InstallBindings()
    {
        BindEnemyFactory();
        BindCellChanger();
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
        Container.
           Bind<GameManager>().
           FromInstance(_gameManager).
           AsSingle();
    }

    private void BindCellChanger()
    {
        Container.
            Bind<BuildCellInitializer>().
            FromInstance(_cellInitializer).
            AsSingle();
    }

    private void BindEnemyFactory()
    {
        Container
            .BindInterfacesAndSelfTo<EnemyFactory>()
            .AsSingle();
    }
}