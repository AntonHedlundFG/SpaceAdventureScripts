using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Elemental
{
    [CreateAssetMenu(fileName = "New Elemental Effect List", menuName = "Elemental/Effect List")]
    public class HitModifierList : ScriptableObject
    {
        [SerializeField] private ElementalHitModifier[] _modifiers = new ElementalHitModifier[0];

        public ElementalHitModifier[] GetMods() => _modifiers;
    }
}