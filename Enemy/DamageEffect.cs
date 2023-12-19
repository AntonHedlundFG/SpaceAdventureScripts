using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private Color _dmgColor;
    [SerializeField] float lerpModifier;
    private List<Material> _materials = new List<Material>();
    private List<Color> _colors = new List<Color>();

    private bool _lerpDmg = false;
    private bool _lerpDeath = false;
    private float _dmgTimer;
    private float _deathTimer;


    private void Start()
    {
        for(int i = 0; i < _renderer.materials.Length; i++)
        {
            _materials.Add(_renderer.materials[i]);
            _colors.Add(_materials[i].color);
        }
        
    }
    void Update()
    {
        DamageLerp();
        DeathLerp();
    }

    public void DeathLerp()
    {
        if (!_lerpDeath) return;
        _deathTimer += (lerpModifier * 1f * Time.deltaTime);
        _materials[1].color = Color.Lerp(_materials[1].color, Color.grey, _deathTimer); // tube
        //_materials[3].color = Color.Lerp(_materials[3].color, Color.grey, _deathTimer); // eye //REMOVED TEMPORARILY
        if(_deathTimer >= 1f)
        {
            _lerpDeath = false;
            _deathTimer = 0f;
        }
    }

    public void DamageLerp()
    {
        if (!_lerpDmg) return;
        _dmgTimer += (lerpModifier * Time.deltaTime);
        
        for(int i = 0; i < _materials.Count - 1; i++) // all elements except the eye
        {
            _materials[i].color = Color.Lerp(_materials[i].color, _colors[i], _dmgTimer);
        }
        if(_dmgTimer >= 1f)
        {
            _lerpDmg = false;
            _dmgTimer = 0f;
        }
    }

    public void TakeDamage()
    {
        for(int i = 0; i < _materials.Count - 1; i++) // all elements except the eye
        {
            _materials[i].color = _dmgColor;
        }
        _dmgTimer = 0f;
        _lerpDmg = true;
    }

    public void DeathEffect()
    {
        _lerpDeath = true;
        _deathTimer = 0f;
    }
}
