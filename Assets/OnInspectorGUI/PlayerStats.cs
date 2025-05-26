// This is a basic MonoBehaviour
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // These fields will be serialized automatically
    public int health = 100;
    public float speed = 5f;

    // Private fields won't be serialized by default
    private int experience;

    // Unless we use the [SerializeField] attribute!
    [SerializeField]
    private int level = 1;

    // This won't be serialized because it has the NonSerialized attribute
    [System.NonSerialized]
    public bool isJumping;

    // Properties aren't serialized by default
    public int Experience
    {
        get { return experience; }
        set { experience = value; }
    }
}