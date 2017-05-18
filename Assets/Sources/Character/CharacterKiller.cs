using UnityEngine;
using UnityEngine.Events;

public enum DeathType
{
    Fall,
    NoClaim
}

public class CharacterKiller : MonoBehaviour {

    public UnityEvent OnKill;

	public void Kill(DeathType type)
    {
        OnKill.Invoke();

        Destroy(gameObject);
    }

}
