using UnityEngine;
using UnityEngine.SceneManagement;

public class PondController : MonoBehaviour
{
    private Vector2 defaultGravity = new Vector2(0f, -9.8f);

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Physics2D.gravity = defaultGravity;

            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}