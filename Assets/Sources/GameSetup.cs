using UnityEngine;
using UnityEngine.SceneManagement;

public enum ECharacterControlType
{
    Player,
    Bot,
    Empty,
    Count
}

public class GameSetup : MonoBehaviour {

    private static GameSetup instance;
    public static GameSetup Instance
    {
        get { return instance; }
    }

    private ECharacterControlType[] gameSetup;
    public ECharacterControlType[] Setup
    {
        get
        {
            return gameSetup;
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

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);

        gameSetup = new ECharacterControlType[4];
        gameMode = EGameMode.Score;
    }

    void Start () {

        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

	}

    /// <summary>
    /// Tries to initialize GameManager with game setup
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void SceneManager_activeSceneChanged(Scene previousScene, Scene newScene)
    {
        if (newScene.buildIndex == 1 && GameManager.Instance != null)
            GameManager.Instance.LaunchNewGame(gameSetup, gameMode);
    }

    public void SetCharacterControlTypes(int index, ECharacterControlType newSetup)
    {
        if(index < 4 && index >= 0)
            gameSetup[index] = newSetup;
    }

    public void SetGameMode(EGameMode newGameMode)
    {
        gameMode = newGameMode;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
    }
}
