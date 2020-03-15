using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject cameraObject;
    public GameObject character;
    // Use this for initialization
    void Start()
    {
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
        Instantiate(cameraObject, new Vector3(0, 0, -10), Quaternion.identity);
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
