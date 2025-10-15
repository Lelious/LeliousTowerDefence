using BezierSolution;
using Cysharp.Threading.Tasks;
using Infrastructure.StateMachine;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameForestMap : GameMap
{
    [SerializeField] private Material _gameMaterial;
    [SerializeField] private Material _fogMaterial;
    [SerializeField] private Material _treeMaterial;
    [SerializeField] private List<MeshRenderer> _treesList;
    [SerializeField] private BezierSpline _path;
    [SerializeField] private BuildingGround _buildGround;

    public override void ConstructGameMap(GameLoopStateMachine stateMachine, EnemyFactory enemyFactory, TopMenuInformator topInformator, PointDescriptionData pointDescriptionData, SelectedFrame selectedFrame, GameUIService gameUIService, TowerFactory towerFactory, PoolService poolService)
    {
        _topInformator = topInformator;
        _gameLoopStateMachine = stateMachine;
        _enemyFactory = enemyFactory;
        _pointDescriptionData = pointDescriptionData;
        _colorScheme = _pointDescriptionData.ColorScheme;

        _gameMaterial.SetTexture("_Mask", _colorScheme.MaskTexture);
        _gameMaterial.SetTexture("_Tex1", _colorScheme.BuildPlaneTexture);
        _gameMaterial.SetTexture("_Tex2", _colorScheme.CliffTexture);
        _gameMaterial.SetTexture("_Tex3", _colorScheme.RoadTexture);
        _gameMaterial.SetTexture("_TexEmission", _colorScheme.IsGlowingFloor ? _colorScheme.FloorEmissionTexture : null);
        _gameMaterial.SetColor("_EmissionColorH", _colorScheme.InnerFloorGlow);
        _gameMaterial.SetColor("_EmissionColorL", _colorScheme.OuterFloorGlow);

        _fogMaterial.SetColor("_BaseColor", _colorScheme.FogColor);

        var block = new MaterialPropertyBlock();

        foreach (var item in _treesList)
        {
            item.GetPropertyBlock(block);
            block.SetColor("_Color", GetRandomColor(_colorScheme.TreeColor1, _colorScheme.TreeColor2));
            item.SetPropertyBlock(block);
        }

        _treeMaterial.SetInt("_RenderLeaf", _colorScheme.TreeLeaf == true ? 1 : 0);
        _buildGround.Construct(gameUIService, selectedFrame, towerFactory, poolService);
    }

    public override void StartSpawnPattern()
    {
        EnemySpawn(_closeSpawnToken).Forget();
    }

    public override void ForceSpawn()
    {
        _isForceSpawn = true;
    }

    protected override async UniTask EnemySpawn(CancellationTokenSource token)
    {
        var firstDelay = _pointDescriptionData.SpawnScheme.SpawnPattern[0].Delay;

        _enemyFactory.CreateEnemy(_pointDescriptionData.SpawnScheme).Forget();
        _topInformator.EnableCounter();

        while (firstDelay > 0 && _isForceSpawn == false)
        {
            _topInformator.SetSpawnTime((int)firstDelay);
            await UniTask.WaitForSeconds(1f).SuppressCancellationThrow();
            firstDelay -= 1f;
        }

        _topInformator.EnterSpawnState();
        _topInformator.DisableCounter();

        for (int i = 1; i < _pointDescriptionData.SpawnScheme.SpawnPattern.Count; i++)
        {
            switch (_pointDescriptionData.SpawnScheme.SpawnPattern[i].SpawnPhase)
            {
                case SpawnPattern.PeriodicSpawn:
                    for (int j = 0; j < _pointDescriptionData.SpawnScheme.SpawnPattern[i].SpawnCount; j++)
                    {
                        if (!token.IsCancellationRequested)
                        {
                            var enemy = _enemyFactory.GetEnemy();
                            enemy.SetMovePath(_path);
                            enemy.gameObject.SetActive(true);
                            await UniTask.WaitForSeconds(_pointDescriptionData.SpawnScheme.SpawnPattern[i].Delay);
                        }
                    }
                    break;
                case SpawnPattern.Spawn:
                    if (!token.IsCancellationRequested)
                    {
                        var enemy2 = _enemyFactory.GetEnemy();
                        enemy2.SetMovePath(_path);
                        enemy2.gameObject.SetActive(true);
                    }
                    break;
                case SpawnPattern.Wait:
                    await UniTask.WaitForSeconds(_pointDescriptionData.SpawnScheme.SpawnPattern[i].Delay);
                    break;
            }            
        }
    }
}