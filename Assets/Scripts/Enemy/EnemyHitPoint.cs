using System.Collections.Generic;
using UnityEngine;

public class EnemyHitPoint : MonoBehaviour
{
    private List<Bullet> _attachedBullets = new List<Bullet>();
    private bool _isActivePoint = true;
    public bool GetActiveStatus() => _isActivePoint;
    public void SetInnactive() => _isActivePoint = false;
    public void AttachBulletToHitPoint(Bullet bullet)
    {
        _attachedBullets.Add(bullet);
    }

    public void RemoveAttachedBulletFromHitPoint(Bullet bullet)
    {
        _attachedBullets.Remove(bullet);
    }

    public void ReturnAttachedBulletsToPool()
    {
        if (_attachedBullets.Count < 1)     
            return;
               
        _attachedBullets.Clear();
    }    
}
