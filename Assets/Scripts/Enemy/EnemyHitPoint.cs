using System.Collections.Generic;
using UnityEngine;

public class EnemyHitPoint : MonoBehaviour
{
    private List<Bullet> _attachedBullets = new List<Bullet>();

    public void AttachBulletToHitPoint(Bullet bullet)
    {
        bullet.transform.SetParent(transform);
        _attachedBullets.Add(bullet);
    }

    public void RemoveAttachedBulletFromHitPoint(Bullet bullet)
    {
        bullet.transform.SetParent(null);
        _attachedBullets.Remove(bullet);
    }

    public void ReturnAttachedBulletsToPool()
    {
        if (_attachedBullets.Count < 1)     
            return;
        
        foreach (var bullet in _attachedBullets)
        {
            bullet.transform.SetParent(null);
            bullet.ReturnBulletToPool();
        }
        _attachedBullets.Clear();
    }    
}
