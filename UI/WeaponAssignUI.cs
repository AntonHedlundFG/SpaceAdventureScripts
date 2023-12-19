using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponAssignUI : MonoBehaviour
{
    [SerializeField] private WeaponCooldownManager cooldownManager;
    public int id = 0;

    private void OnPlayerJoined(PlayerInput playerInput)
    {

        cooldownManager = GameObject.Find("CooldownManager").GetComponent<WeaponCooldownManager>();
        id++;
        cooldownManager?.OnJoinedPlayer(playerInput.transform.GetComponentInChildren<SimpleElementalGun>(), id); // NUllcheck
    }
}
