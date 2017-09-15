using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {

    private List<GameObject> characters = new List<GameObject>();
    private GameObject neutralItem;
    private Dictionary<int, int> scoresByCharacterIndex = new Dictionary<int, int>();
    private Dictionary<int, int> roundsByCharacterIndex = new Dictionary<int, int>();
        
	public void InitializeNewRound(ECharacterControlType[] gameSetup)
    {
        // Resets the characters list
        characters.Clear();
        scoresByCharacterIndex.Clear();
        var startingPositions = GameManager.Instance.CurrentGameStartingPositions;

        SpawNeutralItem(GameManager.Instance.NeutralItemPosition.position);

        for(int i = 0; i < gameSetup.Length; i++)
        {
            if(gameSetup[i] != ECharacterControlType.Empty)
            {
                var character = SpawnCharacter(startingPositions[i].position, i, (gameSetup[i] == ECharacterControlType.Player) ? false : true);
                scoresByCharacterIndex.Add(i, 0);
                if (!roundsByCharacterIndex.ContainsKey(i))
                    roundsByCharacterIndex.Add(i, 0);
            }
        }
    }

    GameObject SpawnCharacter(Vector3 position, int playerIndex, bool isBot)
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

        return character;
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

    public void EndRound(int winnerIndex)
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

        roundsByCharacterIndex[winnerIndex]++;
    }

    public int GetWinnerIndex()
    {
        // Last character carrier is winner

        if(GameManager.Instance.GameMode == EGameMode.LastOneStanding)
        {
            foreach (var character in characters)
            {
                if (character.GetComponent<CharacterItemCarrier>().IsCarrying)
                {
                    return character.GetComponent<CharacterData>().PlayerIndex;
                }
            }
        } else if(GameManager.Instance.GameMode == EGameMode.Score)
        {
            // Best score character

            int highestScoreCharacterIndex = 0;
            int highestScore = 0;
            foreach (var kvp in scoresByCharacterIndex)
            {
                if (kvp.Value > highestScore)
                {
                    highestScoreCharacterIndex = kvp.Key;
                    highestScore = kvp.Value;
                }

            }
            return highestScoreCharacterIndex;
        }

        return 0;
    }

    public void UpdateScore(int playerIndex, int scoreModifier)
    {
        if (scoresByCharacterIndex.ContainsKey(playerIndex))
        {
            scoresByCharacterIndex[playerIndex] += scoreModifier;
            GameManager.Instance.UIManager.UpdatePlayerScore(playerIndex, roundsByCharacterIndex[playerIndex], scoresByCharacterIndex[playerIndex]);
        }
    }
}
