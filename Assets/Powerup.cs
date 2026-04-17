using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupType type;
    public float duration = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        switch (type)
        {
            case PowerupType.SlowTime:
                SlowTimeEffect slow = other.GetComponent<SlowTimeEffect>();
                if (slow != null)
                {
                    slow.Activate(duration);
                }
                break;
        }

        Destroy(gameObject);
    }
}

public enum PowerupType
{
    SlowTime
}