public interface IPoollableObject
{
    public void ReturnToPool();
    public void SetInnactive();
    public void SetActive();
    public void Destroy();
    public void SetBulletPool(PoolService pool, bool addToPool = true);
    public virtual PoollableType GetPoolableType() { return PoollableType.Common;}
}
