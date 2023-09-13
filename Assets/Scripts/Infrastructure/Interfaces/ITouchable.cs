public interface ITouchable
{
    public bool IsTouched();
    public void Touch();
    public void Untouch();
    public UnityEngine.GameObject gameObject { get; }
}
