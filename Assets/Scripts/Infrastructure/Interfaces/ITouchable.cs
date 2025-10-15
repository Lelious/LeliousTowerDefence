using UnityEngine;

public interface ITouchable
{
    public bool IsTouched();
    public void Touch(Vector3 touchPos);
    public void Untouch();
}
