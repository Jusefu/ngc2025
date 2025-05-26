using UnityEngine;

public class PlayerExample : MonoBehaviour, IGameEventListener
{
    [SerializeField] private GameEvent playerDiedEvent;
    [SerializeField] private GameEvent damageEvent;

    int currentHealth = 1;

    private void OnEnable()
    {
        damageEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        damageEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        currentHealth--;
        if(currentHealth <= 0)
        {
            playerDiedEvent.Raise();
        }
    }

}
