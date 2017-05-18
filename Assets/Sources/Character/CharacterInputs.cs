using UnityEngine;

public class CharacterInputs : MonoBehaviour {

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

    public bool IsInterrupted = false;

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
                    var direction = new Vector3();
                    direction.x = Input.GetAxis(_PlayerPrefix + "_Horizontal");
                    direction.y = 0f;
                    direction.z = Input.GetAxis(_PlayerPrefix + "_Vertical");

                    _CharacterPhysics.Move(direction);

                    if (Input.GetButton(_PlayerPrefix + "_Dash"))
                        _CharacterPhysics.Dash();
                }
            }
        }
    }
}
