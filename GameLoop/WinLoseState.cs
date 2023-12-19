using UnityEngine;

[CreateAssetMenu(fileName = "WinLoseState", menuName = "WinLoseState")]
public class WinLoseState : ScriptableObject
{
    public bool GameWon = true;
    public bool WingOneAttached = true;
    public bool WingTwoAttached = true;
}
