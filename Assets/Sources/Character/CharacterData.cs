using UnityEngine;
using UnityEngine.UI;

public class CharacterData : MonoBehaviour {

    [SerializeField]
    private int playerIndex;
    public int PlayerIndex
    {
        get
        {
            return playerIndex;
        }
    }

    [SerializeField]
    private CharacterSelectionCircle characterSelectionCircle;

    public bool IsInterrupted = false;

    public void Initialize(int playerIndex)
    {
        this.playerIndex = playerIndex;
        characterSelectionCircle.SetColor(GameManager.Instance.Parameters.COLORS_PLAYER[playerIndex]);
        InitializeUI();
    }

    public void ResetCooldown()
    {
        
    }

    void InitializeUI()
    {
        
            
    }

    public void StopTimer()
    {
        IsInterrupted = true;
    }
}
