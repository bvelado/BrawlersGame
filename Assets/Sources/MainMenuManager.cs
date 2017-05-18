using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    [SerializeField]
    private Text[] characterControlTexts;

    private GameSetup gameSetup;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        var gameSetup = FindObjectOfType<GameSetup>();

        // If there is already a GameSetup instanciated
        if (gameSetup != null)
        {
            this.gameSetup = gameSetup;

            for(int i = 0;i < characterControlTexts.Length; i++)
            {
                characterControlTexts[i].text = gameSetup.Setup[i].ToString();
            }
        }
        else
        {
            this.gameSetup = new GameObject("GameSetup").AddComponent<GameSetup>();

            this.gameSetup.SetGameSetup(0, ECharacterControlType.Player);
            this.gameSetup.SetGameSetup(1, ECharacterControlType.Bot);
            this.gameSetup.SetGameSetup(2, ECharacterControlType.Empty);
            this.gameSetup.SetGameSetup(3, ECharacterControlType.Empty);
        }
    }

    public void PlayButtonPressed()
    {
        int numberOfPlayersAndBots = 0;
        foreach(var characterControl in gameSetup.Setup)
        {
            if (characterControl != ECharacterControlType.Empty)
                numberOfPlayersAndBots++;
        }

        if(numberOfPlayersAndBots > 1)
            SceneManager.LoadScene(1);
    }

    public void CreditsButtonPressed()
    {

    }

    public void QuitButtonPresed()
    {
        Application.Quit();
    }

    public void ScrollCharacterControlLeft(int playerIndex)
    {
        ECharacterControlType newCharacterControl;
        if (gameSetup.Setup[playerIndex] - 1 < 0)
            newCharacterControl = ECharacterControlType.Empty;
        else
            newCharacterControl = gameSetup.Setup[playerIndex] - 1;

        gameSetup.SetGameSetup(playerIndex, newCharacterControl);
        characterControlTexts[playerIndex].text = newCharacterControl.ToString();
    }

    public void ScrollCharacterControlRight(int playerIndex)
    {
        ECharacterControlType newCharacterControl;
        if (gameSetup.Setup[playerIndex] + 1 == ECharacterControlType.Count)
            newCharacterControl = ECharacterControlType.Player;
        else
            newCharacterControl = gameSetup.Setup[playerIndex] + 1;

        gameSetup.SetGameSetup(playerIndex, newCharacterControl);
        characterControlTexts[playerIndex].text = newCharacterControl.ToString();
    }

}
