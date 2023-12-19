using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elemental;

public class EnemyStatusEffectUI : MonoBehaviour
{
    [SerializeField] private ElementalTarget elementalTarget;

    [SerializeField] private GameObject wetIndicator;
    [SerializeField] private GameObject oilyIndicator;
    [SerializeField] private GameObject burningIndicator;
    [SerializeField] private GameObject stunnedIndicator;

    
    void Update()
    {
        wetIndicator.SetActive(elementalTarget.AffectedBy(ElementalType.Water));
        oilyIndicator.SetActive(elementalTarget.AffectedBy(ElementalType.Earth));
        burningIndicator.SetActive(elementalTarget.AffectedBy(ElementalType.Fire));
        stunnedIndicator.SetActive(elementalTarget.AffectedBy(ElementalType.Air));
    }
}
