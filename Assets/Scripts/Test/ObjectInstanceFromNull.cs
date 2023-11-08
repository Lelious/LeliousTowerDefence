using UnityEngine;
using DG.Tweening;

public class ObjectInstanceFromNull : MonoBehaviour
{
    [SerializeField] private Vector3 _startScale;

    public void EnableEffect()
    {
        transform.DOScale(_startScale, 1f);
    }

    public void DisableEffect()
    {
        transform.transform.DOScale(Vector3.zero, 1f);
    }
}
