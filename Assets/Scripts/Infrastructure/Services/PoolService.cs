using System;
using System.Collections.Generic;
using Zenject;

public class PoolService : IInitializable
{
    private Dictionary<Type, Queue<IPoollableBullet>> _pool;

    public void Initialize()
    {
        _pool = new Dictionary<Type, Queue<IPoollableBullet>>();
    }

    public void AddBulletToPool(Type type, IPoollableBullet bullet)
    {
        bullet.SetInnactive();

        if (Validate(type))      
            _pool[type].Enqueue(bullet);       
        else       
            InitializeNewKeyValuePair(type, bullet);      
    }

    public void RemoveBulletsFromPool(Type type, int count)
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

    public Bullet GetBulletFromPool(Type type)
    {
        if (Validate(type))
        {
            var queue = _pool[type];

            if (queue.Count > 0)
            {
                var bullet = queue.Dequeue();
                bullet.SetActive();
                return bullet.GetBulletType();
            }
            else           
                return null;            
        }
        else      
            return null;       
    }

    private void InitializeNewKeyValuePair(Type type, IPoollableBullet bullet)
    {
        var queue = new Queue<IPoollableBullet>();
        queue.Enqueue(bullet);
        _pool.Add(type, queue);
    }

    private bool Validate(Type type)
    {
        return _pool.ContainsKey(type);
    }
}
