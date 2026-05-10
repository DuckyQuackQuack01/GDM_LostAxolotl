using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PondController : MonoBehaviour
{
    [Header("Splash Effect")]
    public GameObject splashEffect;

    private bool levelComplete = false;

    private Vector2 defaultGravity = new Vector2(0f, -9.8f);

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || levelComplete)
            return;

        levelComplete = true;

        // Spawn splash effect
        if (splashEffect != null)
        {
            GameObject splash = Instantiate(
            splashEffect,
            collision.transform.position,
            Quaternion.identity
        );

            splash.transform.SetParent(collision.transform);
        }

        // Freeze player
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // Play finish sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayFinish();
        }

        // Reset gravity
        Physics2D.gravity = defaultGravity;

        // Load next scene
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(1f);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}