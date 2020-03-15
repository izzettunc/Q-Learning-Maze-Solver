using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parameters
{
    public static int mazeLength;
    public static bool selectRandomStartAndEnd;
    public static float stepPerSecond;
    public static float waitingTimeAtTheEnd;
    public static int reward;
    public static float learningRate;
    public static float discountFactor;
    public static int epoch;
    public static int stopLearningAt;
    public static bool smartChoosing;
}
public class handler : MonoBehaviour
{
    //settings
    public int mazeLength;
    public bool selectRandomStartAndEnd;
    public float stepPerSecond;
    public float waitingTimeAtTheEnd;
    public int reward;
    public float learningRate;
    public float discountFactor;
    public int epoch;
    public int stopLearningAt;
    public bool smartChoosing;
    public GameObject mazeGenerator;
    public Camera cameraObject;
    public GameObject character;
    // Use this for initialization
    void Start()
    {
        this.mazeLength = parameters.mazeLength;
        this.selectRandomStartAndEnd = parameters.selectRandomStartAndEnd;
        this.stepPerSecond = parameters.stepPerSecond;
        this.waitingTimeAtTheEnd = parameters.waitingTimeAtTheEnd;
        this.reward = parameters.reward;
        this.learningRate = parameters.learningRate;
        this.discountFactor = parameters.discountFactor;
        this.epoch = parameters.epoch;
        this.stopLearningAt = parameters.stopLearningAt;
        this.smartChoosing = parameters.smartChoosing;
        //set the random seed
        Random.InitState((int)System.DateTime.Now.Ticks);
        this.gameObject.SetActive(false);
        //initializing of the maze
        GameObject generator = Instantiate(mazeGenerator, new Vector3(0, 0, 0), Quaternion.identity);
        maze_generator mgs = generator.GetComponent<maze_generator>();
        //assigning some settings
        mgs.mazeLength = mazeLength;
        mgs.select_random_start_and_end = selectRandomStartAndEnd;
        this.gameObject.SetActive(true);
        //creates maze and stop
        this.gameObject.SetActive(false);
        //initializing of camera
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>().worldCamera = Instantiate(cameraObject, new Vector3(0, 0, -10), Quaternion.identity);
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>().planeDistance = 5;
        //initializing of Q Learner instance
        q_learner ql = generator.AddComponent<q_learner>();
        //assigning of some settings
        ql.character = character;
        ql.reward = reward;
        ql.learning_rate = learningRate;
        ql.discount_factor = discountFactor;
        ql.step_per_second = stepPerSecond;
        ql.smart = smartChoosing;
        ql.maxEpoch = epoch;
        ql.stopLearningAt = stopLearningAt;
        ql.waitingTimeAtTheEnd = waitingTimeAtTheEnd;
        this.gameObject.SetActive(true);
        //and start the programme
    }

    // Update is called once per frame
    void Update()
    {

    }
}
