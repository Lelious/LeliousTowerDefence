using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private GameObject _quad;

    private MaterialPropertyBlock _matBlock;
    private float _maxHealth = 1f;
    private float _currentHealth = 1f;
    private bool _canBeAttacked = true;

    private void Awake()
    {       
        UpdateShaderParams();
    }

    public void SetMaxHealth(float health)
    {
        _maxHealth = health;

        UpdateShaderParams();
    }

    public void SetHealth(float health)
    {
        if (health > _maxHealth)
            _currentHealth = _maxHealth;
        else
            _currentHealth = health;

        UpdateShaderParams();
    }

    public bool CanBeAttacked() => _canBeAttacked;
    public float GetHealth() => _currentHealth;
    public void Hide() => _quad.SetActive(false);
    public void Show() => _quad?.SetActive(true);

    private void UpdateShaderParams()
    {
        if (_matBlock == null)
        {
            _matBlock = new MaterialPropertyBlock();
        }

        _meshRenderer.GetPropertyBlock(_matBlock);
        _matBlock.SetFloat("_Fill", _currentHealth / _maxHealth);
        _meshRenderer.SetPropertyBlock(_matBlock);
    }
}
