using Infrastructure.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : State
{
	public GameState(GameLoopStateMachine gameLoopStateMachine) : base(gameLoopStateMachine) { }
	public override void Enter()
	{
		throw new System.NotImplementedException();
	}

	public override void Exit()
	{
		throw new System.NotImplementedException();
	}
}
