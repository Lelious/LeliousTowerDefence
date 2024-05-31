using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PoolService : IInitializable
{
    private Dictionary<BulletType, Queue<IPoollableBullet>> _bulletPool;
    private Dictionary<PoollableType, Queue<IPoollableObject>> _commonPool;
    public void Initialize() 
    {
        _bulletPool = new Dictionary<BulletType, Queue<IPoollableBullet>>();
        _commonPool = new Dictionary<PoollableType, Queue<IPoollableObject>>();
    }

    public void AddBulletToPool(BulletType type, IPoollableBullet bullet)
    {
        bullet.SetInnactive();

        if (Validate(type))      
            _bulletPool[type].Enqueue(bullet);       
        else       
            InitializeNewKeyValuePair(type, bullet);      
    }

    public void AddPoollable(PoollableType type, IPoollableObject poollable)
    {
        poollable.SetInnactive();

        if (Validate(type))
            _commonPool[type].Enqueue(poollable);
        else
            InitializeNewKeyValuePair(type, poollable);
    }

    public void RemoveBulletsFromPool(BulletType type, int count)
    {
        if (Validate(type))
        {
            var queue = _bulletPool[type];

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

    public IPoollableObject GetObjectFromPool(PoollableType type)
    {
        if (Validate(type))
        {
            var queue = _commonPool[type];

            if (queue.Count > 0)
            {
                var poolObj = queue.Dequeue();
                poolObj.SetActive();
                return poolObj;
            }
            else          
                return null;          
        }
        else        
            return null;
        
    }

    public IPoollableBullet GetBulletFromPool(BulletType type)
    {
        if (Validate(type))
        {
            var queue = _bulletPool[type];

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
        _bulletPool.Add(type, queue);
    }

    private void InitializeNewKeyValuePair(PoollableType type, IPoollableObject obj)
    {
        var queue = new Queue<IPoollableObject>();
        queue.Enqueue(obj);
        _commonPool.Add(type, queue);
    }

    private bool Validate(BulletType type) => _bulletPool.ContainsKey(type);
    private bool Validate(PoollableType type) => _commonPool.ContainsKey(type);
}
