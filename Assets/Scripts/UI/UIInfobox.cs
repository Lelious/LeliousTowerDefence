using TMPro;
using UnityEngine;

public class UIInfobox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name, _description;

    public void SetDescription(string name, string description)
    {
        _name.text = name;
        _description.text = description;
    }
}
