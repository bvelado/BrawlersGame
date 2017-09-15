using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    [SerializeField]
    private Text[] characterControlTexts;

    [SerializeField]
    private Text gameModeText;

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

            this.gameSetup.SetCharacterControlTypes(0, ECharacterControlType.Player);
            this.gameSetup.SetCharacterControlTypes(1, ECharacterControlType.Bot);
            this.gameSetup.SetCharacterControlTypes(2, ECharacterControlType.Empty);
            this.gameSetup.SetCharacterControlTypes(3, ECharacterControlType.Empty);
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
            newCharacterControl = ECharacterControlType.Count - 1;
        else
            newCharacterControl = gameSetup.Setup[playerIndex] - 1;

        gameSetup.SetCharacterControlTypes(playerIndex, newCharacterControl);
        characterControlTexts[playerIndex].text = newCharacterControl.ToString();
    }

    public void ScrollCharacterControlRight(int playerIndex)
    {
        ECharacterControlType newCharacterControl;
        if (gameSetup.Setup[playerIndex] + 1 == ECharacterControlType.Count)
            newCharacterControl = ECharacterControlType.Player;
        else
            newCharacterControl = gameSetup.Setup[playerIndex] + 1;

        gameSetup.SetCharacterControlTypes(playerIndex, newCharacterControl);
        characterControlTexts[playerIndex].text = newCharacterControl.ToString();
    }

    public void ScrollGameModeRight()
    {
        EGameMode newGameMode;
        if (gameSetup.GameMode + 1 == EGameMode.Count)
            newGameMode = EGameMode.Score;
        else
            newGameMode = gameSetup.GameMode + 1;

        gameSetup.SetGameMode(newGameMode);
        switch (newGameMode)
        {
            case EGameMode.Score:
                gameModeText.text = "Score";
                break;
            case EGameMode.LastOneStanding:
                gameModeText.text = "Last crown owner";
                break;
            default:
                gameModeText.text = "Score";
                break;
        }
    }

    public void ScrollGameModeLeft()
    {
        EGameMode newGameMode;
        if (gameSetup.GameMode - 1 < 0)
            newGameMode = EGameMode.Count - 1;
        else
            newGameMode = gameSetup.GameMode - 1;

        gameSetup.SetGameMode(newGameMode);
        switch (newGameMode)
        {
            case EGameMode.Score:
                gameModeText.text = "Score";
                break;
            case EGameMode.LastOneStanding:
                gameModeText.text = "Last crown owner";
                break;
            default:
                gameModeText.text = "Score";
                break;
        }
    }

}
