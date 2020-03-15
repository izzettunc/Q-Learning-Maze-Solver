using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UI_Handler : MonoBehaviour
{

    public void valueTextUpdater()
    {
        string name = this.name.Split('_')[1];
        GameObject valueText = GameObject.Find("Value_" + name);
        float val = this.GetComponent<Slider>().value;
        if (Mathf.Ceil(val) > val)
            valueText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:0.00}", this.GetComponent<Slider>().value);
        else
            valueText.GetComponent<TextMeshProUGUI>().text = this.GetComponent<Slider>().value + "";
    }

    public void startSimulation()
    {
        GameObject.Find("Label_Error").GetComponent<TextMeshProUGUI>().text = "";
        try
        {
            parameters.mazeLength = (int)GameObject.Find("Slider_MazeLength").GetComponent<Slider>().value;
            parameters.selectRandomStartAndEnd = GameObject.Find("Toggle_SelectRSE").GetComponent<Toggle>().isOn;
            parameters.stepPerSecond = GameObject.Find("Slider_StepPerSecond").GetComponent<Slider>().value;
            parameters.waitingTimeAtTheEnd = GameObject.Find("Slider_WTATE").GetComponent<Slider>().value;
            parameters.reward = int.Parse(GameObject.Find("Input_Reward").GetComponent<TMP_InputField>().text);
            parameters.learningRate = GameObject.Find("Slider_LearningRate").GetComponent<Slider>().value;
            parameters.discountFactor = GameObject.Find("Slider_DiscountFactor").GetComponent<Slider>().value;
            parameters.epoch = int.Parse(GameObject.Find("Input_Epoch").GetComponent<TMP_InputField>().text);
            parameters.stopLearningAt = int.Parse(GameObject.Find("Input_StopLearningAt").GetComponent<TMP_InputField>().text);
            parameters.smartChoosing = GameObject.Find("Toggle_Smart").GetComponent<Toggle>().isOn;
            SceneManager.LoadScene("Simulation");
        }
        catch
        {
            GameObject.Find("Label_Error").GetComponent<TextMeshProUGUI>().text = "Error : Please check the inputs";
        }

    }

    public void assignPreviousValues(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Settings")
        {
            GameObject.Find("Slider_MazeLength").GetComponent<Slider>().value = parameters.mazeLength;
            GameObject.Find("Toggle_SelectRSE").GetComponent<Toggle>().isOn = parameters.selectRandomStartAndEnd;
            GameObject.Find("Slider_StepPerSecond").GetComponent<Slider>().value = parameters.stepPerSecond;
            GameObject.Find("Slider_WTATE").GetComponent<Slider>().value = parameters.waitingTimeAtTheEnd;
            GameObject.Find("Input_Reward").GetComponent<TMP_InputField>().text = parameters.reward + "";
            GameObject.Find("Slider_LearningRate").GetComponent<Slider>().value = parameters.learningRate;
            GameObject.Find("Slider_DiscountFactor").GetComponent<Slider>().value = parameters.discountFactor;
            GameObject.Find("Input_Epoch").GetComponent<TMP_InputField>().text = parameters.epoch + "";
            GameObject.Find("Input_StopLearningAt").GetComponent<TMP_InputField>().text = parameters.stopLearningAt + "";
            GameObject.Find("Toggle_Smart").GetComponent<Toggle>().isOn = parameters.smartChoosing;
        }
    }


    public void stopSimulation()
    {
        SceneManager.LoadScene("Settings");
    }

    // Use this for initialization
    void Start()
    {
        SceneManager.sceneLoaded += assignPreviousValues;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
