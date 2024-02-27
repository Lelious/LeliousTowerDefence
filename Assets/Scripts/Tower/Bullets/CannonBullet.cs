using UnityEngine;

public class CannonBullet : Bullet
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private LayerMask _mask;

    private Collider[] _hits = new Collider[10];

    public override void BulletReadyToFly() => _trail.SetActive(true);
    public override void BulletAchieveTarget() => Explode();
    public override void ApplySpecialEffects()
    {
        throw new System.NotImplementedException();
    }

    private void Explode()
    {
        ClearColliders();

        _impactOnHit.transform.position = transform.position;

        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, _hits, _mask);

        for (int i = 0; i < numColliders; i++)
        {
            _damagable = _hits[i].GetComponent<IDamagable>();

            ApplyDamage();
        }
        
        _impactOnHit.SetActive(true);

        ReturnToPool();
    }

    private void ClearColliders()
    {
        for (int i = 0; i < _hits.Length - 1; i++)
            _hits[i] = null;
    }
}

