using UnityEngine;

namespace Elemental
{
    public abstract class ElementalHitModifier : ScriptableObject
    {
        public abstract void ModifyHit(ref WeaponHit hitData, ElementalTarget target);
    }
}


