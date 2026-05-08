using System.Runtime.CompilerServices;
using UnityEngine;

public class TumbleweedSpawner : MonoBehaviour
{
    [SerializeField] private GameObject TumbleweedTemplate; // create tumbleweed with new config and set this to be
    [SerializeField] private Transform Sandstorm; // set to sandstorm in scene
    [SerializeField] private int TumbleweedTotal = 4; // amount of tumbleweed to spawn

    private Transform templateTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        templateTransform = TumbleweedTemplate.transform;

        for (int i = 1; i < TumbleweedTotal; i++)
        {
            // creating new tumbleweed and adding it to this object
            var newTumbleweed = Instantiate(TumbleweedTemplate);
            newTumbleweed.transform.parent = gameObject.transform;

            UpdateTumbleweedPosition(newTumbleweed, i);

            // setting tumbleweed to still spawn where original starts.
            newTumbleweed.GetComponent<FallingPlatform>().setNewStartPosition(templateTransform.position);
        }

    }

    //Spreads the tumbleweeds between the startpos and sandstorm.
    private void UpdateTumbleweedPosition(GameObject tumbleweedTransform, int distanceNumerator)
    {
        float distanceFraction = (float)distanceNumerator / (TumbleweedTotal);
        Vector3 newPosition = Vector3.Lerp(templateTransform.position, Sandstorm.position, distanceFraction);
        tumbleweedTransform.transform.position = newPosition;
    }

}
