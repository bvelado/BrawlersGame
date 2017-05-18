using UnityEngine;

public class BotInputs : MonoBehaviour {

    private string playerPrefix = null;
    private string _PlayerPrefix
    {
        get
        {
            return playerPrefix;
        }
    }

    private CharacterPhysics characterPhysics;
    private CharacterPhysics _CharacterPhysics
    {
        get
        {
            return characterPhysics;
        }
    }

    [SerializeField]
    private float dashChance = 0.06f;

    public bool IsInterrupted = false;

    [SerializeField]
    private float botDecisionCooldown = 0.3f;
    private float botDecisionTimer = 0f;
    private Vector2 botDirection = Vector2.zero;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (GetComponent<CharacterData>() != null)
            playerPrefix = "P" + (GetComponent<CharacterData>().PlayerIndex + 1).ToString();

        if (GetComponent<CharacterPhysics>() != null)
            characterPhysics = GetComponent<CharacterPhysics>();
    }

    private void Update()
    {
        if (!IsInterrupted)
        {
            if (_PlayerPrefix != null)
            {
                if (_CharacterPhysics != null)
                {
                    if(botDecisionTimer > botDecisionCooldown)
                    {
                        botDirection.x = Random.Range(-1f, 1f);
                        botDirection.y =  Random.Range(-1f, 1f);

                        if (Random.Range(0f, 1f) < dashChance)
                            _CharacterPhysics.Dash();

                        botDecisionTimer = 0f;
                    } else
                    {
                        botDecisionTimer += Time.deltaTime;
                    }

                    var direction = new Vector3(botDirection.x, 0f, botDirection.y);

                    _CharacterPhysics.Move(direction);
                }
            }
        }
    }
}
