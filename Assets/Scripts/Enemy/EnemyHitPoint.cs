using System.Collections.Generic;
using UnityEngine;

public class EnemyHitPoint : MonoBehaviour
{
    private List<IPoollableBullet> _attachedBullets = new List<IPoollableBullet>();

    public void AttachBulletToHitPoint(IPoollableBullet bullet)
    {
        bullet.Transform().SetParent(transform);
        _attachedBullets.Add(bullet);
    }

    public void RemoveAttachedBulletFromHitPoint(IPoollableBullet bullet)
    {
        bullet.Transform().SetParent(null);
        bullet.ReturnToPool();

        _attachedBullets.Remove(bullet);
    }

    public void ReturnAttachedBulletsToPool()
    {
        foreach (var bullet in _attachedBullets)
        {
            bullet.Transform().SetParent(null);
            bullet.ReturnToPool();
        }

        _attachedBullets.Clear();
    }
}
