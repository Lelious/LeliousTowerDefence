using System;
using UnityEngine;

public interface ITouchable
{
    public Vector3 GetPosition();
    public void Touch();
    public void Untouch();
}
