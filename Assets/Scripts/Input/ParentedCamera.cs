using UnityEngine;

public class ParentedCamera : MonoBehaviour
{
	[SerializeField]
	private float _leftBoarderLimitMin,
				  _leftBoarderLimitMax,
				  _rightBoarderLimitMin,
				  _rightBoarderLimitMax,
				  _upperBoarderLimitMin,
				  _upperBoarderLimitMax,
				  _downBoarderLimitMin,
				  _downBoarderLimitMax;

	private CameraLimiter _cameraLimiter;
	private Vector4 _limitsVector;

	private void Awake()
	{
		_cameraLimiter = FindObjectOfType<CameraLimiter>();
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

	public void ZoomCamera(Vector3 zoomVector)
	{
		if (zoomVector.y > 0 && transform.position.y < 30f)
		{
			transform.position += zoomVector;
		}
		else if (zoomVector.y < 0 && transform.position.y > 5f)
		{
			transform.position += zoomVector;
		}

		CheckForClampedValue(5, 30, transform.position.y);
	}

	private void CheckForClampedValue(float min, float max, float current)
	{
		float minBoarder = 0f;
		float maxBoarder = 1f;
		float real = minBoarder + (current - min) * ((maxBoarder - minBoarder) / (max - min));

		float leftLimit = Mathf.Lerp(_leftBoarderLimitMax, _leftBoarderLimitMin, real);
		float rightLimit = Mathf.Lerp(_rightBoarderLimitMin, _rightBoarderLimitMax, real);
		float upLimit = Mathf.Lerp(_upperBoarderLimitMin, _upperBoarderLimitMax, real);
		float downLimit = Mathf.Lerp(_downBoarderLimitMin, _downBoarderLimitMax, real);
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
