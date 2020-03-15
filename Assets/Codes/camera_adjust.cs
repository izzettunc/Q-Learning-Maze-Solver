using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_adjust : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //automated camera field of view according to maze length
        this.GetComponentInParent<Camera>().orthographicSize = (GameObject.FindGameObjectWithTag("Mg").GetComponent<maze_generator>().mazeLength * 1.0f / 2)+0.5f;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
