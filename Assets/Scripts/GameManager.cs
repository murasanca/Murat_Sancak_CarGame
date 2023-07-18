using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// GameManager class that manages the game state and logic.
public class GameManager:MonoBehaviour
{
    // Rotation angle of buttons.
    [Header("Rotation Angle of Buttons"), SerializeField]
    private float angles = 22.5f;

    // UI buttons.
    [Header("Buttons"), SerializeField]
    private GameObject playButton;
    [SerializeField]
    private GameObject
        turnLeftButton,
        turnRightButton;

    // Cars in starting positions.
    [Space(8)]
    [Header("Cars in Starting Positions"), SerializeField]
    private GameObject[] cars = new GameObject[8];

    // Finish lines for each car.
    [Header("Finish Lines"), SerializeField]
    private GameObject[] finishes = new GameObject[8];

    // Index of the current car being controlled by the player.
    private int currentCarIndex = 0;

    // Struct to store transform data (position and rotation).
    private struct TransformData
    {
        public Quaternion rotation;
        public Vector3 position;
    }

    // List of transform data for each car to record their movements.
    private IList<TransformData>[] transformRecordList;


    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        // Initialize the transformRecordList with a list for each car.
        transformRecordList=new List<TransformData>[cars.Length];
        for(int i = 0;i<transformRecordList.Length;++i)
            transformRecordList[i]=new List<TransformData>();
    }

    // Start is called before the first frame update.
    private void Start()
    {
        // Set activeSelf of all cars to false except the first car.
        for(int i = 1;i<cars.Length;++i)
            cars[i].SetActive(false);

        // Set activeSelf of all finishes to false except the first finish.
        for(int i = 1;i<finishes.Length;++i)
            finishes[i].SetActive(false);

        Restart();
    }


    /// <summary>
    /// Turn Left Button: Rotates the current car to the left by angles degrees.
    /// </summary>
    public void Left()
    {
        if(currentCarIndex<cars.Length)
            cars[currentCarIndex].transform.Rotate(angles*Vector3.forward);
    }

    /// <summary>
    /// Turn Right Button: Rotates the current car to the right by angles degrees.
    /// </summary>
    public void Right()
    {
        if(currentCarIndex<cars.Length)
            cars[currentCarIndex].transform.Rotate(angles*Vector3.back);
    }

    /// <summary>
    /// Play Button: Starts recording the movements of the current car and replays the movements of previous cars.
    /// </summary>
    public void Play()
    {
        Time.timeScale=1f;

        ReplayPreviousCars();
        StartCoroutine(RecordCurrentCar());
    }


    /// <summary>
    /// Next: Moves on to the next car or loads the next level if all cars have been completed.
    /// </summary>
    public void Next()
    {
        if(++currentCarIndex<cars.Length)
        {
            Restart();

            cars[currentCarIndex].SetActive(true);
            finishes[currentCarIndex].SetActive(true);
        }
        else
        {
            StopAllCoroutines();

            int nextBuildIndex = 1+SceneManager.GetActiveScene().buildIndex;
            if(nextBuildIndex!=SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(nextBuildIndex);
            else
                Debug.LogWarning("End of the Game");
        }
    }

    /// <summary>
    /// Restart: Restarts the current level, resetting all cars and clearing recorded movements for the current car.
    /// </summary>
    public void Restart()
    {
        Time.timeScale=0f;

        StopAllCoroutines();
        transformRecordList[currentCarIndex].Clear();

        for(int i = 0;i<currentCarIndex;++i)
            cars[i].GetComponent<Car>().ResetTransform();

        playButton.SetActive(true);

        turnLeftButton.SetActive(false);
        turnRightButton.SetActive(false);
    }


    // Coroutine to record the movements of the current car every fixed update.
    private IEnumerator RecordCurrentCar()
    {
        while(true)
        {
            transformRecordList[currentCarIndex].Add
            (
                new TransformData
                {
                    position=cars[currentCarIndex].transform.position,
                    rotation=cars[currentCarIndex].transform.rotation
                }
            );
            yield return new WaitForFixedUpdate();
        }
    }

    // Replays the movements of previous cars by setting their position and rotation every fixed update according to their recorded movements.
    private void ReplayPreviousCars()
    {
        for(int i = 0;i<currentCarIndex;++i)
            StartCoroutine(ReplayCar(i));
    }

    // Coroutine to replay a specific car's movements every fixed update according to its recorded movements.
    private IEnumerator ReplayCar(int carIndex)
    {
        foreach(TransformData transformData in transformRecordList[carIndex])
        {
            cars[carIndex].transform.SetPositionAndRotation(transformData.position,transformData.rotation);
            yield return new WaitForFixedUpdate();
        }
    }
}
