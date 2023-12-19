using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public int playerID = -1;

    [SerializeField] private GameObject[] _playerPrefabs = new GameObject[2];
    [SerializeField] private PlayerInputManager _manager;

    [SerializeField] private Transform playerSpawnPoint;

    [SerializeField] private PlayerList _playerList;
   


    private GameObject[] _spawnedPlayers = new GameObject[2];

    
    private void Awake()
    {
        _manager.playerPrefab = _playerPrefabs[0];
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        GameObject player = playerInput.gameObject;
        player.GetComponent<CharacterController>().enabled = false;

        player.transform.position = playerSpawnPoint != null ? playerSpawnPoint.position : transform.position;
        
        player.GetComponent<CharacterController>().enabled = true;
        

        playerID++;
        _spawnedPlayers[playerID] = playerInput.gameObject;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) 
        {
            enemy.GetComponent<SpiderAIPatrol>().SetPlayerRefs(player);
        }

        if (playerID == 0)
        {
            _manager.playerPrefab = _playerPrefabs[1];
            //_spawnedPlayers[0].GetComponentInChildren<Camera>().rect = new Rect(0, 0f, 1, 1f); Entire screen
            _spawnedPlayers[0].GetComponentInChildren<Camera>().rect = new Rect(0, 0.5f, 1, 0.5f);

            _playerList.ResetList();
            _playerList.AddPlayer(playerInput);
        }

        if(playerID == 1)
        {
            
            _spawnedPlayers[0].GetComponentInChildren<Camera>().rect = new Rect(0, 0.5f, 1, 0.5f);
            _spawnedPlayers[1].GetComponentInChildren<Camera>().rect = new Rect(0, 0, 1, 0.5f);

            _playerList.AddPlayer(playerInput);
        }
    }


}
