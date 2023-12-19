using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Player List", menuName = "Player List")]
public class PlayerList : ScriptableObject
{
    private List<PlayerInput> _players;
    public void ResetList() => _players = new List<PlayerInput>();
    public void AddPlayer(PlayerInput player) => _players.Add(player);
    public List<PlayerInput> GetPlayers() => _players;

    private void OnDisable() => ResetList();
}
