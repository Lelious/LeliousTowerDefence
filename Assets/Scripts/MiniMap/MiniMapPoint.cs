using UnityEngine;

public class MiniMapPoint : MonoBehaviour, ITouchable
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private PointDescriptionData _data;

    private MiniMapPointInfoField _infoField;
    private MaterialPropertyBlock _block;

    private void Start()
    {
        _block = new MaterialPropertyBlock();
        Untouch();
    }

    public void InitializePoint(MiniMapPointInfoField infoField) => _infoField = infoField;
    public bool IsTouched() => false;
    public MeshRenderer GetMeshRenderer() => _renderer;

    public void Touch(Vector3 touchPos)
    {
        _renderer.GetPropertyBlock(_block);
        _block.SetFloat("_IsTouched", 1f);
        _renderer.SetPropertyBlock(_block);
        _infoField.ShowInfoField(_data);
    }

    public void Untouch()
    {
        _renderer.GetPropertyBlock(_block);
        _block.SetFloat("_IsTouched", 0f);
        _renderer.SetPropertyBlock(_block);
        _infoField.HideInfoField();
    }
}
