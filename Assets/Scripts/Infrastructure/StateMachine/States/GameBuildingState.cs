using Infrastructure.StateMachine;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

public class GameBuildingState : State
{
	private readonly CompositeDisposable _disposables = new CompositeDisposable();	
	private TopMenuInformator _topMenuInformator;
	private System.IDisposable _timerRoutine;
	private const int _timerTime = 30;
	private int _timerValue;

	[Inject]
	private void Construct(TopMenuInformator topMenuInformator)
	{
		_topMenuInformator = topMenuInformator;
	}

	public GameBuildingState(GameLoopStateMachine gameLoopStateMachine) : base(gameLoopStateMachine) { }

	public override void Enter()
	{
		_topMenuInformator.EnableDisableCounter();
		_timerRoutine = Observable
							.FromCoroutine(TimerBeforeSpawnRoutine)
							.Subscribe()
							.AddTo(_disposables);
	}

	public override void Exit()
	{
		_timerRoutine.Dispose();
		_topMenuInformator.EnableDisableCounter();
	}

	private IEnumerator TimerBeforeSpawnRoutine()
	{
		_timerValue = _timerTime;


		while (_timerValue >= 0)
		{
			_topMenuInformator.SetSpawnTime(_timerValue);
			_timerValue--;

			yield return new WaitForSeconds(1f);
		}
		_topMenuInformator.EnterSpawnState();
    }
}
