using UnityEngine;
using Zenject;

public class ParentedCamera : MonoInstaller
{
	private MapCameraLimits _limits;
	private Vector4 _limitsVector;

	public void SetNewLimits(MapCameraLimits limits)
    {
		_limits = limits;
		transform.position = _limits.InitialCameraPos;
	}

	public void MoveCamera(Vector3 direction)
	{
		//bool moveX = _cameraLimiter.GetAvalabilityToMoveX(direction);
		//bool moveZ = _cameraLimiter.GetAvalabilityToMoveZ(direction);

		//if (moveX && moveZ)	
		//	transform.position += direction;

		//else if (moveX && !moveZ)		
		//	transform.position += new Vector3(direction.x, 0f, direction.z);

		//else if (!moveX && moveZ)
		//	transform.position += new Vector3(0, direction.y, direction.z);
		transform.position += direction;
		CheckForClampedValue(5, 30, transform.position.y);
	}

	public void ZoomCamera(float value)
	{
		Vector3 dir = Camera.main.transform.forward;
		transform.position += dir * value;
		transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _limits.HeightMin, _limits.HeightMax), transform.position.z);
	}

	private void CheckForClampedValue(float min, float max, float current)
	{
		float minBoarder = 0f;
		float maxBoarder = 1f;
		float real = minBoarder + (current - min) * ((maxBoarder - minBoarder) / (max - min));

		float leftLimit = Mathf.Lerp(_limits.LeftBoarderMax, _limits.LeftBoarderMin, real);
		float rightLimit = Mathf.Lerp(_limits.RightBoarderMin, _limits.RightBoarderMax, real);
		float upLimit = Mathf.Lerp(_limits.UpLimitMin, _limits.UpLimitMax, real);
		float downLimit = Mathf.Lerp(_limits.DownLimitMin, _limits.DownLimitMax, real);
		_limitsVector = new Vector4(leftLimit, rightLimit, upLimit, downLimit);

		ReturnCameraToLimits();
	}

	private void ReturnCameraToLimits()
	{
		var clampedX = Mathf.Clamp(transform.position.x, _limitsVector.y, _limitsVector.x);
		var clampedZ = Mathf.Clamp(transform.position.z, _limitsVector.z, _limitsVector.w);		
		transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
	}
}
