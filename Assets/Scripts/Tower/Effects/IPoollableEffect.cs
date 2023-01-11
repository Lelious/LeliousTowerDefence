public interface IPoollableEffect
{
    public void SetPool(EffectsPool pool);
    public void ReturnToPool();
    public void SetInnactive();
    public void SetActive();
    public bool IsFree();
}
