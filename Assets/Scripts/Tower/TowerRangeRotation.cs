using UnityEngine;
using DG.Tweening;

public class TowerRangeRotation : MonoBehaviour
{
    private protected void Awake()
    {
        transform.DORotateQuaternion(Quaternion.Euler(0, 90f, 0), 5f).SetEase(Ease.Linear).SetLoops(-1);
    }
}
