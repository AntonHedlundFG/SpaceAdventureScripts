using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="NewResource", menuName = "Resources/Resource")]
public class Resource : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon2D;
    [SerializeField] private GameObject _model;
    [SerializeField] private Color _color;
    [SerializeField] private int _gatheredAmount = 0;

    public int GatheredAmount
    {
        get
        {
            return _gatheredAmount;
        }
        set
        {
            _gatheredAmount = value;
            OnAmountChanged.Invoke(_gatheredAmount);
        }
    }

    public UnityEvent<int> OnAmountChanged;

    public string Name => _name;
    public Sprite Icon2D => _icon2D;
    public Color Color => _color;
    public GameObject Model => _model;
}
