using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int score = 0;

    public static void AddPoint()
    {
        score++;
    }

    public static void ResetScore()
    {
        score = 0;
    }
}


