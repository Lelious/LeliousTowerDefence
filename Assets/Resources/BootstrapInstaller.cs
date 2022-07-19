using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private GameBottomPanel _bottomPanel;
    [SerializeField] private BuildCellChanger _cellChanger;
    [SerializeField] private MenuUpdater _menuUpdater;
    public override void InstallBindings()
    {
        BindEnemyFactory();
        BindBottomGamePanel();
        BindCellChanger();
        BindMenuUpdater();
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
            Bind<BuildCellChanger>().
            FromInstance(_cellChanger).
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