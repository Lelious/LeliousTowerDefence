using System.Collections.Generic;
using Zenject;

public class PoolService : IInitializable
{
    private Dictionary<BulletType, Queue<IPoollableBullet>> _pool;

    public void Initialize() => _pool = new Dictionary<BulletType, Queue<IPoollableBullet>>();

    public void AddBulletToPool(BulletType type, IPoollableBullet bullet)
    {
        bullet.SetInnactive();

        if (Validate(type))      
            _pool[type].Enqueue(bullet);       
        else       
            InitializeNewKeyValuePair(type, bullet);      
    }

    public void RemoveBulletsFromPool(BulletType type, int count)
    {
        if (Validate(type))
        {
            var queue = _pool[type];

            for (int i = 0; i < count; i++)
            {
                if (queue.Count > 0)
                {
                    var bullet = queue.Dequeue();
                    bullet.DestroyBullet();
                }
            }            
        }
    }

    public IPoollableBullet GetBulletFromPool(BulletType type)
    {
        if (Validate(type))
        {
            var queue = _pool[type];

            if (queue.Count > 0)
            {
                var bullet = queue.Dequeue();
                bullet.SetActive();
                return bullet;
            }
            else           
                return null;            
        }
        else      
            return null;       
    }

    private void InitializeNewKeyValuePair(BulletType type, IPoollableBullet bullet)
    {
        var queue = new Queue<IPoollableBullet>();
        queue.Enqueue(bullet);
        _pool.Add(type, queue);
    }

    private bool Validate(BulletType type) => _pool.ContainsKey(type);
}
