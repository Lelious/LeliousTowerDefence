using Infrastructure.StateMachine;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

public class GameBuildingState : State
{
	private TopMenuInformator _topMenuInformator;

	public GameBuildingState(GameLoopStateMachine gameLoopStateMachine) : base(gameLoopStateMachine) { }

	[Inject]
	private void Construct(TopMenuInformator topMenuInformator)
	{
		_topMenuInformator = topMenuInformator;
	}

	public override void Enter()
	{

	}

	public override void Exit()
	{

	}
}
