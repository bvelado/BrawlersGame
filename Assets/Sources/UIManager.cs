using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private CanvasGroup gameIntroPanel;

    [SerializeField]
    private CanvasGroup roundIntroPanel;

    [SerializeField]
    private CanvasGroup winnerAnnoucementPanel;

    [SerializeField]
    private Text winnerText;

    [SerializeField]
    private CanvasGroup timerPanel;

    [SerializeField]
    private Text[] playerScores;

    [SerializeField]
    private Text timerText;

    private float timer;
    private bool playTimer = false;

    public Action OnTimerEnd;

    private Sequence currentSequence;

	public void PlayGameIntro(Action callbackOnEnd)
    {
        Sequence gameIntroSequence = DOTween.Sequence();

        gameIntroSequence.Append(gameIntroPanel.DOFade(1f, GameManager.Instance.Parameters.INTRO_ANIMATION_DURATION));
        gameIntroSequence.Append(gameIntroPanel.DOFade(0f, GameManager.Instance.Parameters.INTRO_ANIMATION_DURATION / 2f));
        
        gameIntroSequence.OnKill(() => callbackOnEnd.Invoke());

        currentSequence = gameIntroSequence;

        gameIntroSequence.Play();
    }

    public void PlayRoundIntro(Action callbackOnEnd)
    {
        Sequence roundIntroSequence = DOTween.Sequence();

        roundIntroSequence.InsertCallback(0f, () => roundIntroPanel.alpha = 1f);

        foreach(Transform child in roundIntroPanel.transform)
        {
            roundIntroSequence.InsertCallback(0f, () => child.GetComponent<CanvasGroup>().alpha = 0f);

            roundIntroSequence.Append(child.GetComponent<CanvasGroup>().DOFade(1f, GameManager.Instance.Parameters.INTRO_ANIMATION_DURATION));
            roundIntroSequence.Append(child.GetComponent<CanvasGroup>().DOFade(0f, GameManager.Instance.Parameters.INTRO_ANIMATION_DURATION / 2f));
        }

        roundIntroSequence.AppendCallback(() => roundIntroPanel.alpha = 0f);

        roundIntroSequence.OnKill(() => callbackOnEnd());

        currentSequence = roundIntroSequence;

        roundIntroSequence.Play();
    }

    public void DisplayGameUI(bool display, ECharacterControlType[] gameSetup)
    {
        foreach(var playerScore in playerScores)
        {
            playerScore.color = (display ? Color.white : Color.clear);
        }
        timerText.color = (display ? Color.white : Color.clear);

        for(int i = 0; i < 4; i++)
        {
            if (gameSetup[i] == ECharacterControlType.Empty)
                playerScores[i].color = Color.clear;
        }
    }

    public void PlayWinnerAnnoucement(int playerIndex, Action callbackOnEnd)
    {
        winnerText.text = "Player " + (playerIndex + 1).ToString() + " won !";

        Sequence playWinnerAnnoucementSequence = DOTween.Sequence();

        playWinnerAnnoucementSequence.InsertCallback(0f, () => winnerAnnoucementPanel.alpha = 1f);

        playWinnerAnnoucementSequence.Append(winnerAnnoucementPanel.DOFade(1f, GameManager.Instance.Parameters.INTRO_ANIMATION_DURATION));
        playWinnerAnnoucementSequence.Append(winnerAnnoucementPanel.DOFade(0f, GameManager.Instance.Parameters.INTRO_ANIMATION_DURATION / 2f));

        playWinnerAnnoucementSequence.AppendCallback(() => winnerAnnoucementPanel.alpha = 0f);
        playWinnerAnnoucementSequence.AppendInterval(2f);

        playWinnerAnnoucementSequence.OnKill(() => callbackOnEnd());

        currentSequence = playWinnerAnnoucementSequence;

        playWinnerAnnoucementSequence.Play();
    }

    void ClearPanels()
    {
        roundIntroPanel.alpha = 0f;
        gameIntroPanel.alpha = 0f;
    }

    private void Update()
    {
        if (playTimer)
        {
            timer -= Time.deltaTime;
            timerText.text = string.Format("{0}:{1:00}", (int)timer / 60, (int)timer % 60);

            if (timer < 0f)
            {
                playTimer = false;
                OnTimerEnd.Invoke();
            }
        }
    }

    public void PlayTimer(bool playTimer) { this.playTimer = playTimer; }

    public void RegisterOnTimerEnd(Action callback)
    {
        OnTimerEnd += callback;
    }

    public void ResetTimer()
    {
        timer = GameManager.Instance.Parameters.ROUND_DURATION;
    }

    public void SetPlayerRoundsWon(int[] rounds)
    {
        for(int i = 0; i < 4; i++)
        {
            playerScores[i].text = "Player " + (i + 1) + "\r\nRounds won : " + rounds[i] + "\r\nScore : 0";
        }
    }

    public void UpdatePlayerScore(int playerIndex, int rounds, int score)
    {
        playerScores[playerIndex].text = "Player " + (playerIndex + 1) + "\r\nRounds won : " + rounds + "\r\nScore : " + score;
    }
}
