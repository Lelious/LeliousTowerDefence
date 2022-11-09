using UnityEngine;

public class CameraLimiter : MonoBehaviour
{
	[SerializeField] private CameraFrustrumChecker _leftChecker, 
												   _rightChecker, 
												   _upperChecher, 
												   _downChecker;
	public bool GetAvalabilityToMoveX(Vector3 direction)
	{
		bool canMove = true; 

		if (direction.x < 0)		
			canMove = !_rightChecker.CheckForVisibility();
		
		else if (direction.x > 0)
			canMove = !_leftChecker.CheckForVisibility();
		
		return canMove;
	}

	public bool GetAvalabilityToMoveZ(Vector3 direction)
	{
		bool canMove = true;

		if (direction.z < 0)
			canMove = !_upperChecher.CheckForVisibility();

		else if (direction.z > 0)
			canMove = !_downChecker.CheckForVisibility();

		return canMove;
	}
}
