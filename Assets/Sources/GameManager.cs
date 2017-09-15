using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private GameParameters parameters;
    public GameParameters Parameters
    {
        get
        {
            return parameters;
        }
    }

    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    private Dictionary<int, int> gameScore = new Dictionary<int, int>();

    [SerializeField]
    private Transform[] startingPositions;
    private Transform[] currentGameStartingPositions;
    public Transform[] CurrentGameStartingPositions
    {
        get
        {
            return currentGameStartingPositions;
        }
    }

    [SerializeField]
    private Transform neutralItemPosition;
    public Transform NeutralItemPosition
    {
        get
        {
            return neutralItemPosition;
        }
    }

    [SerializeField]
    private RoundManager roundManager;
    public RoundManager RoundManager
    {
        get
        {
            return roundManager;
        }
    }

    [SerializeField]
    private UIManager uiManager;
    public UIManager UIManager
    {
        get
        {
            return uiManager;
        }
    }

    private EGameMode gameMode;
    public EGameMode GameMode
    {
        get
        {
            return gameMode;
        }
    }

    private ECharacterControlType[] gameSetup;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void LaunchNewGame(ECharacterControlType[] gameSetup, EGameMode gameMode)
    {
        this.gameSetup = gameSetup;
        this.gameMode = gameMode;

        // Rerolls the starting positions order
        RerollStartingPositions();

        // Resets the scoreboard
        gameScore.Clear();
        for(int i = 0; i < 4; i++)
        {
            gameScore.Add(i, 0);
        }

        int[] scores = new int[4];
        scores[0] = gameScore[0];
        scores[1] = gameScore[1];
        scores[2] = gameScore[2];
        scores[3] = gameScore[3];

        uiManager.SetPlayerRoundsWon(scores);

        // Reinitialize the RoundManager
        roundManager.InitializeNewRound(gameSetup);

        uiManager.ResetTimer();
        uiManager.RegisterOnTimerEnd(OnTimerEnd);
        uiManager.DisplayGameUI(false, gameSetup);
        uiManager.PlayRoundIntro(() => { OnRoundIntroPlayed(); });
        //OnRoundIntroPlayed();
    }

    void OnRoundIntroPlayed()
    {
        roundManager.StartRound();
        uiManager.PlayTimer(true);
        uiManager.DisplayGameUI(true, gameSetup);
    }

    void OnTimerEnd()
    {
        var winner = roundManager.GetWinnerIndex();
        gameScore[winner]++;
        roundManager.EndRound(winner);

        bool gameWinner = false;
        foreach(var score in gameScore)
        {
            if (score.Value > 1)
            {
                EndGame(score.Key);
                gameWinner = true;
            }
        }

        if (!gameWinner)
        {
            int[] rounds = new int[4];
            for (int i = 0; i < rounds.Length; i++)
            {
                rounds[i] = gameScore[i];
            }

            uiManager.SetPlayerRoundsWon(rounds);

            // Reinitialize the RoundManager
            roundManager.InitializeNewRound(gameSetup);

            uiManager.ResetTimer();
            uiManager.DisplayGameUI(false, gameSetup);
            uiManager.PlayRoundIntro(OnRoundIntroPlayed);
        }

    }

    void EndGame(int winnerIndex)
    {
        uiManager.PlayWinnerAnnoucement(winnerIndex, () => SceneManager.LoadScene(0));
    }

    void RerollStartingPositions()
    {
        List<Transform> newPositions = new List<Transform>();

        List<Transform> oldPositions = new List<Transform>();
        oldPositions.AddRange(startingPositions);

        while(oldPositions.Count > 0)
        {
            var index = Random.Range(0, oldPositions.Count);
            newPositions.Add(oldPositions[index]);
            oldPositions.RemoveAt(index);
        }

        currentGameStartingPositions = newPositions.ToArray();
    }
}
