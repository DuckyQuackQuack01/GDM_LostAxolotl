using UnityEngine;

public class TumbleweedSpawner : MonoBehaviour
{
    [SerializeField] private GameObject TumbleweedTemplate; // create tumbleweed with new config and set this to be
    [SerializeField] private Transform Sandstorm; // set to sandstorm in scene
    [SerializeField] private int TumbleweedTotal = 4; // amount of tumbleweed to spawn


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 1; i < TumbleweedTotal; i++)
        {
            var newTumbleweed = Instantiate(TumbleweedTemplate);
            newTumbleweed.transform.parent = gameObject.transform;
            UpdateTumbleweedPosition(newTumbleweed, i);
        }

    }

    //Spreads the tumbleweeds between the startpos and sandstorm.
    private void UpdateTumbleweedPosition(GameObject tumbleweedTransform, int distanceNumerator)
    {
        float distanceFraction = (float)distanceNumerator / (TumbleweedTotal+1);
        Debug.Log("moving tumbleweed to " + distanceFraction);
        Vector3 newPosition = Vector3.Lerp(TumbleweedTemplate.transform.position, Sandstorm.transform.position, distanceFraction);
        Debug.Log("moving tumblweed " + distanceNumerator + " to " + newPosition);
        tumbleweedTransform.transform.position = newPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
