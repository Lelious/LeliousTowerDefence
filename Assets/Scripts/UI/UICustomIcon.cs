using UnityEngine;
using UnityEngine.UI;

public class UICustomIcon : MonoBehaviour, ITouchable
{
    [SerializeField] private Image _image;
    [SerializeField] private Text _name;
    public bool IsTouched()
    {
        throw new System.NotImplementedException();
    }

    public void Touch()
    {
        throw new System.NotImplementedException();
    }

    public void Untouch()
    {
        throw new System.NotImplementedException();
    }
}
