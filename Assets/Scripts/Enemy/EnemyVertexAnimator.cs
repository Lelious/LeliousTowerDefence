using UnityEngine;

public class EnemyVertexAnimator : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    
    private MaterialPropertyBlock _block;
    private float _animationSpeed = 1f;
    private float _time;

    private void Awake()
    {
        _block = new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(_block);
    }

    private void Update()
    {
        _time += Time.deltaTime * _animationSpeed;
        _block.SetFloat("_AnimationTime", _time);
        _renderer.SetPropertyBlock(_block);
    }

    public void SetAnimationSpeed(float speed) => _animationSpeed = speed;
    public void SetColor(Color color)
    {
        _block.SetColor("_AdittionalColor", color);
        _renderer.SetPropertyBlock(_block);
    }
}
