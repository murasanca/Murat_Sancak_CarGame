using UnityEngine;

public class GameManager:MonoBehaviour
{
    [Header("Cars in Starting Positions and Rotations"),SerializeField]
    private GameObject[]cars;

    private void Awake()
    {
        // Set activeSelf of all cars to false except the first car.
        for(int i=1;i<cars.Length;++i)
            cars[i].SetActive(false);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}