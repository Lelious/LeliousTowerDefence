using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _flyingSpeed = 1f;

    private Transform _enemyPos;
    private int _damage;
    private bool _onFlying;
    private float _flyingProgress;
    private bool _isTakeDamage;

    public void FlyToTarget(Transform enemyPosition, int damage)
    {
        if (!_onFlying)
        {         
            _enemyPos = enemyPosition;
            _damage = damage;
            _onFlying = !_onFlying;
        }
    }

    private protected void FixedUpdate()
    {
        if (_onFlying)
        {
            if (_enemyPos != null)
            {
                transform.LookAt(_enemyPos);

                if (Vector3.Distance(transform.position, _enemyPos.position) > 0.2f)
                {
                    transform.position = Vector3.Lerp(transform.position, _enemyPos.position, _flyingProgress);
                    _flyingProgress += _flyingSpeed;
                }
                else
                {
                    if (!_isTakeDamage)
                    {
                        _isTakeDamage = !_isTakeDamage;
                        gameObject.transform.SetParent(_enemyPos);
                        _enemyPos.GetComponent<Enemy>().RecieveDamage(_damage);
                        Destroy(gameObject, 1f);
                    }
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
