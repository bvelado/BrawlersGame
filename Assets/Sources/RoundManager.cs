using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {

    private List<GameObject> characters = new List<GameObject>();
    private GameObject neutralItem;
        
	public void InitializeNewRound(ECharacterControlType[] gameSetup)
    {
        // Resets the characters list
        characters.Clear();
        var startingPositions = GameManager.Instance.CurrentGameStartingPositions;

        SpawNeutralItem(GameManager.Instance.NeutralItemPosition.position);

        for(int i = 0; i < gameSetup.Length; i++)
        {
            if(gameSetup[i] != ECharacterControlType.Empty)
                SpawnCharacter(startingPositions[i].position, i, (gameSetup[i] == ECharacterControlType.Player) ? false : true);
        }
    }

    void SpawnCharacter(Vector3 position, int playerIndex, bool isBot)
    {
        GameObject character;
        // If it is a bot
        if(isBot)
        {
            character = Instantiate(GameManager.Instance.Parameters.CHARACTER_BOT_PREFAB, position, Quaternion.identity, null);
        } else
        {
            character = Instantiate(GameManager.Instance.Parameters.CHARACTER_PLAYER_PREFAB, position, Quaternion.identity, null);
        }

        characters.Add(character);

        character.GetComponent<CharacterData>().Initialize(playerIndex);

        character.GetComponent<CharacterData>().IsInterrupted = true;
        if(isBot)
            character.GetComponent<BotInputs>().IsInterrupted = true;
        else
            character.GetComponent<CharacterInputs>().IsInterrupted = true;
        character.GetComponent<CharacterPhysics>().IsInterrupted = true;
    }

    void SpawNeutralItem(Vector3 position)
    {
        neutralItem = Instantiate(GameManager.Instance.Parameters.NEUTRAL_ITEM_PREFAB, position, Quaternion.identity, null);
    }

    public void StartRound()
    {
        foreach(var character in characters)
        {
            character.GetComponent<CharacterData>().IsInterrupted = false;
            if(character.GetComponent<CharacterInputs>() != null)
                character.GetComponent<CharacterInputs>().IsInterrupted = false;
            else
                character.GetComponent<BotInputs>().IsInterrupted = false;
            character.GetComponent<CharacterPhysics>().IsInterrupted = false;
        }
    }

    public void EndRound()
    {
        var tmpChars = characters.ToArray();

        characters.Clear();

        foreach(var tmpChar in tmpChars)
        {
            Destroy(tmpChar);
        }

        if (neutralItem != null)
            Destroy(neutralItem);
        neutralItem = null;
    }

    public int GetWinnerIndex()
    {
        foreach(var character in characters)
        {
            if (character.GetComponent<CharacterItemCarrier>().IsCarrying)
            {
                return character.GetComponent<CharacterData>().PlayerIndex;
            }
        }

        return 0;
    }
}
