using Elemental;
using UnityEngine;
using UnityEngine.Events;

public class ElementalTargetObject : ElementalTarget
{
    public UnityEvent<ElementalType> OnHitByCorrectElement;
    [SerializeField] private ElementalType _type;
    
    [SerializeField][Range(0f, 2f)] private float _cooldown = 0.75f;
    private float _lastInvokeTime;
    public override void ApplyHit(WeaponHit hitData)
    {
        if (enabled && hitData.HitType == _type && Time.time >= _lastInvokeTime + _cooldown)
        {
            OnHitByCorrectElement.Invoke(_type);
            _lastInvokeTime = Time.time;
        }
    }

    private void OnEnable() => _lastInvokeTime = Time.time;
}
