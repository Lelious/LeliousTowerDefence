using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShurikenBullet : Bullet
{
    [SerializeField] private List<IDamagable> _hittedTargets = new List<IDamagable>();

    private float _currentDamage;
    private int _currentTarget; 

    public override void ApplySpecialEffects()
    {
        throw new System.NotImplementedException();
    }

    public override void BulletAchieveTarget()
    {
        ApplyDamage((int)_currentDamage);

        if (!_hittedTargets.Contains(_damagable))
        {
            _hittedTargets.Add(_damagable);
        }

        if (_currentTarget < _ricoshetteCount)
        {
            _damagable = FindNextTarget();

            if (_damagable != null)
            {
                SetTarget(_damagable);

                _currentTarget++;
                _currentDamage *= 0.7f;
                Mathf.Clamp(_currentDamage, 1f, Mathf.Infinity);

                ResetPath(transform.position);
            }
            else
            {
                _currentTarget = _ricoshetteCount;                

                ReturnToPool();
            }
        }
        else
        {
            ReturnToPool();
        }
    }

    public override void BulletReadyToFly()
    {
        _currentDamage = CalculateDamage();
        _currentTarget = 0;
    }

    private IDamagable FindNextTarget()
    {
        var availableTargets = _enemyPool.GetEnemiesFromDistance(transform, 2f, _damagable);
        List<IDamagable> correctTargets = new List<IDamagable>();

        if (availableTargets.Count == 0)
        {
            return null;
        }

        else
        {
            foreach (var target in availableTargets)
            {
                if (!_hittedTargets.Contains(target))
                {
                    correctTargets.Add(target);
                }
            }

            if (correctTargets.Count > 0)
            {
                correctTargets = correctTargets.OrderBy((x) => (x.GetOrigin().position - transform.position).sqrMagnitude).ToList();
                return correctTargets[0];
            }

            else
            {
                return availableTargets.FirstOrDefault();
            }
        }
    }
}
