using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{
    //[SerializeField] private PlayerSettingsUI _settingsUI;
    private int spawnedPlayers = 0;

    public bool IsPaused { get; private set; }

    [SerializeField] private GameObject pauseScreen;

    private void Start()
    {
        ChangePauseStatus();
    }
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.actions.FindAction("Pause").started += ChangePauseStatus;
        spawnedPlayers++;
        if (spawnedPlayers == 2)
        {
            ChangePauseStatus();
        }
    }
    [ContextMenu("Change Pause Status")]
    public void ChangePauseStatus()
    {
        IsPaused = !IsPaused;
        pauseScreen.SetActive(IsPaused);
        Time.timeScale = IsPaused ? 0f : 1f;
        //_settingsUI?.ShowUI(IsPaused);
    }
    public void ChangePauseStatus(InputAction.CallbackContext context) => ChangePauseStatus();
}
