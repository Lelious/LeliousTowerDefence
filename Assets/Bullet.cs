using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _flyingSpeed = 1f;
    [SerializeField, Range(0, 5)] private float _timeBeforeReturning;
    private Shooter _shooter;
    private IDamagable _damagable;
    private Transform _enemyPos;
    private int _damage;
    private bool _onFlying;
    private float _flyingProgress;
    private bool _isTakeDamage;

    public void FlyToTarget(Transform enemyPosition, int damage, IDamagable damagable, Shooter shooter)
    {
        _shooter = shooter;

        if (!_onFlying)
        {
            _flyingProgress = 0f;
            _enemyPos = enemyPosition;
            _damage = damage;
            _onFlying = true;
            _damagable = damagable;
            _isTakeDamage = false;
        }
    }

    private protected void FixedUpdate()
    {
        if (_onFlying)
        {
            if (_enemyPos != null)
            {
                transform.LookAt(_enemyPos);

                if (Vector3.Distance(transform.position, _enemyPos.position) > 0.5f)
                {
                    transform.position = Vector3.Lerp(transform.position, _enemyPos.position, _flyingProgress);
                    _flyingProgress += _flyingSpeed;
                }
                else
                {
                    if (!_isTakeDamage)
                    {
                        _isTakeDamage = !_isTakeDamage;
                        _damagable.TakeDamage(_damage);
                        _damagable.ApplyBullet(this);
                        transform.SetParent(_enemyPos);
                        _onFlying = false;
                        StartCoroutine(ReturnToPoolRoutine());
                    }
                }
            }
        }
    }

    public void ReturnBullet()
    {
        _shooter.ReturnToPool(this);        
    }

    private IEnumerator ReturnToPoolRoutine()
    {
        yield return new WaitForSeconds(_timeBeforeReturning);

        if (_damagable != null)
        {
            _damagable.RemoveBullet(this);
            _damagable = null;
        }
        _shooter.ReturnToPool(this);
    }
}
