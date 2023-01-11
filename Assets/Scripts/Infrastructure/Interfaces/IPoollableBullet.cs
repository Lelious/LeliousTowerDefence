using UnityEngine;

public interface IPoollableBullet
{
    public Bullet GetBulletType();
    public void ReturnToPool();
    public void SetInnactive();
    public void SetActive();
}
