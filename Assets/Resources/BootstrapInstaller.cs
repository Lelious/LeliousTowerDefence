using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private GameBottomPanel _bottomPanel;
    [SerializeField] private BuildCellInitializer _cellInitializer;
    [SerializeField] private MenuUpdater _menuUpdater;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TowerBuilder _towerBuilder;

    public override void InstallBindings()
    {
        BindEnemyFactory();
        BindBottomGamePanel();
        BindCellChanger();
        BindMenuUpdater();
        BindGameManager();
        BindTowerBuilder();
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

    private void BindMenuUpdater()
    {
        Container.
           Bind<MenuUpdater>().
           FromInstance(_menuUpdater).
           AsSingle();
    }

    private void BindCellChanger()
    {
        Container.
            Bind<BuildCellInitializer>().
            FromInstance(_cellInitializer).
            AsSingle();
    }

    private void BindBottomGamePanel()
    {
        Container.
            Bind<GameBottomPanel>().
            FromInstance(_bottomPanel).
            AsSingle();
    }

    private void BindEnemyFactory()
    {
        Container
            .BindInterfacesAndSelfTo<EnemyFactory>()
            .AsSingle();
    }
}