using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class q_learner : MonoBehaviour
{
    //settings
    public bool smart;
    public int reward;
    public int maxEpoch;
    public int stopLearningAt;
    public float waitingTimeAtTheEnd;
    public GameObject character;
    public float learning_rate;
    public float discount_factor;
    public float step_per_second;
    //counters/markers/iterators
    float timer;
    int epochCounter;
    int curVertice;
    float target_x, target_y;
    bool flag = false;
    bool lilFlag = false;
    bool lastFlag = true;
    int[,] usedTable;
    //datas
    int[,] rewardTable;
    float[,] qTable;
    
    float biggestQVal=float.MinValue;
    int tableLength;
    maze dungeon;
    List<int> indices = new List<int>();
    List<float> qValues = new List<float>();
    
    // Use this for initialization
    void Start()
    {
        timer = 1 / step_per_second;

        maze_generator mg = this.GetComponentInParent<maze_generator>();
        dungeon = mg.Get_Dungeon;

        int mazeLength = dungeon.getMaze_length();
        tableLength = mazeLength * mazeLength;

        rewardTable = new int[tableLength, tableLength];
        qTable = new float[tableLength, tableLength];
        usedTable = new int[tableLength, tableLength];
        //creates reward table
        for (int i = 0; i < tableLength; i++)
        {
            for (int j = 0; j < tableLength; j++)
            {
                //first put every state to state as -1 so it is not reachable
                rewardTable[i, j] = -1;
                int curVerIndex_j = i % mazeLength;
                int curVerIndex_i = (i - curVerIndex_j) / mazeLength;
                int targetVerIndex_j = j % mazeLength;
                int targetVerIndex_i = (j - targetVerIndex_j) / mazeLength;
                //foreach state to state
                cell current = dungeon.getCells()[curVerIndex_i][curVerIndex_j];
                cell target = dungeon.getCells()[targetVerIndex_i][targetVerIndex_j];
                //look for current states neighbours
                foreach (cell neighbour in current.getNeighbours())
                {
                    //if its reachable
                    if (neighbour == target)
                    {
                        //then mark it as 0 so it is reachable
                        rewardTable[i, j] = 0;
                        //if it is the end point then mark it with reward
                        if (dungeon.end_i == targetVerIndex_i && dungeon.end_j == targetVerIndex_j)
                        {
                            rewardTable[i, j] = reward;
                        }
                        break;
                    }
                }
            }
        }

        character = Instantiate(character, new Vector3(dungeon.start_x, dungeon.start_y, 0), Quaternion.identity);
        curVertice = dungeon.start_i * dungeon.getMaze_length() + dungeon.start_j;
    }
    
    //Assigns target
    void go(int to)
    {
        //converts between markers , indexes and coordinates
        //finds index of marker
        curVertice = to;
        int target_j = to % dungeon.getMaze_length();
        int target_i = (to - target_j) / dungeon.getMaze_length();
        target_x = 0;
        target_y = 0;
        //find coordinates of indices
        if (dungeon.getMaze_length() % 2 == 0)
        {
            target_x -= ((dungeon.getMaze_length() / 2) - 0.5f)*16;
            target_y += ((dungeon.getMaze_length() / 2) - 0.5f)*16;
        }
        else
        {
            target_x -= Mathf.Floor(dungeon.getMaze_length() / 2)*16;
            target_y += Mathf.Floor(dungeon.getMaze_length() / 2)*16;
        }
        target_x += target_j*16;
        target_y -= target_i*16;
        //assign target
        if (target_i == dungeon.end_i && target_j == dungeon.end_j)
        {
            flag = true;
            epochCounter++;
        }
    }

    Color colorFunction(int usedTime)
    {
        return new Color(usedTime*1.0f/dungeon.getMaze_length(), 0f, 1f/usedTime, usedTime * 2.0f / (dungeon.getMaze_length()/2));
    }
    //Update visual represantation of Q Table in scene
    void updateVisualQTable(int cur,int to)
    {
        if(usedTable[cur, to]<dungeon.getMaze_length())
            usedTable[cur, to]++;
        int mazeLength = dungeon.getMaze_length();
        maze_generator mg = this.gameObject.GetComponent<maze_generator>();
        GameObject[][] dungeonObjects = mg.Get_DungeonObjects;
        int cur_j = cur % mazeLength;
        int cur_i = (cur - cur_j) / mazeLength;
        if (cur-mazeLength==to) //north
        {
            Transform cell = dungeonObjects[cur_i][cur_j].transform;
            for (int i=0;i< cell.childCount;i++)
            {
                if(cell.GetChild(i).tag=="NM")
                {
                    cell.GetChild(i).GetComponent<SpriteRenderer>().color = colorFunction(usedTable[cur, to]);
                    break;
                }
            }
        }
        else if(cur +1 == to) //east
        {
            Transform cell = dungeonObjects[cur_i][cur_j].transform;
            for (int i = 0; i < cell.childCount; i++)
            {
                if (cell.GetChild(i).tag == "EM")
                {
                    cell.GetChild(i).GetComponent<SpriteRenderer>().color = colorFunction(usedTable[cur, to]);
                    break;
                }
            }
        }
        else if (cur + mazeLength == to) //south
        {
            Transform cell = dungeonObjects[cur_i][cur_j].transform;
            for (int i = 0; i < cell.childCount; i++)
            {
                if (cell.GetChild(i).tag == "SM")
                {
                    cell.GetChild(i).GetComponent<SpriteRenderer>().color = colorFunction(usedTable[cur, to]);
                    break;
                }
            }
        }
        else if (cur - 1 == to) //west
        {
            Transform cell = dungeonObjects[cur_i][cur_j].transform;
            for (int i = 0; i < cell.childCount; i++)
            {
                if (cell.GetChild(i).tag == "WM")
                {
                    cell.GetChild(i).GetComponent<SpriteRenderer>().color = colorFunction(usedTable[cur, to]); ;
                    break;
                }
            }
        }
    }

    //Q learning algorithm
    void learn()
    {
        float maxNextVal = float.MinValue;
        float tempVal;
        if (smart)//finds max next prospective Q value and selects it
        {
            for (int i = 0; i < tableLength; i++)
            {
                if (rewardTable[curVertice, i] != -1)
                {
                    maxNextVal = int.MinValue;
                    for (int j = 0; j < tableLength; j++)
                    {
                        if (rewardTable[i, j] != -1 && qTable[i, j] > maxNextVal)
                        {
                            maxNextVal = qTable[i, j];
                        }
                    }
                    tempVal = qTable[curVertice, i] + learning_rate * (rewardTable[curVertice, i] + discount_factor * maxNextVal - qTable[curVertice, i]);
                    qValues.Add(tempVal);
                    indices.Add(i);
                }
            }
        }
        else//finds max q value and selects it
        {
            for (int i = 0; i < tableLength; i++)
                if (rewardTable[curVertice, i] != -1)
                {
                    qValues.Add(qTable[curVertice, i]);
                    indices.Add(i);
                }
        }
        float temp = 0;
        //check for if its all the same
        for (int i = 0; i < qValues.Count; i++)
        {
            temp += qValues[i];
        }
        int maxIndex = -1;
        float maxVal = float.MinValue;
        //if so then selects random
        if (temp == qValues[0] * qValues.Count)
        {
            maxIndex = indices[Random.Range(0, qValues.Count)];
            maxVal = qValues[0];
        }
        else
        {
            //else selects the max
            for (int i = 0; i < qValues.Count; i++)
            {
                if (qValues[i] > maxVal)
                {
                    maxVal = qValues[i];
                    maxIndex = indices[i];
                }
            }
        }
        qValues.Clear();
        indices.Clear();
        if (smart)//assign qTable value to already computed value
            qTable[curVertice, maxIndex] = maxVal;
        else
        {
            //compute the qTable value then assign it
            for (int j = 0; j < tableLength; j++)
            {
                if (rewardTable[maxIndex, j] != -1 && qTable[maxIndex, j] > maxNextVal)
                {
                    maxNextVal = qTable[maxIndex, j];
                }
            }
            qTable[curVertice, maxIndex] = qTable[curVertice, maxIndex] + learning_rate * (rewardTable[curVertice, maxIndex] + discount_factor * maxNextVal - qTable[curVertice, maxIndex]);
        }
        if (qTable[curVertice, maxIndex] > biggestQVal)
            biggestQVal = qTable[curVertice, maxIndex];
        if(qTable[curVertice, maxIndex]>0)
            updateVisualQTable(curVertice, maxIndex);
        go(maxIndex);


    }
   
    //Selects where to go with the logic of q learning but without learning process
    void walk()
    {

        //Selection algorithm of the q learning
        float maxVal = float.MinValue;
        int maxIndex = -1;
        //looks for all reachable states
        for (int i = 0; i < tableLength; i++)
            if (rewardTable[curVertice, i] != -1)
            {
                qValues.Add(qTable[curVertice, i]);
                indices.Add(i);
            }
        float temp = 0;
        //controll for if all of them are the same
        for (int i = 0; i < qValues.Count; i++)
        {
            temp += qValues[i];
        }
        maxIndex = -1;
        maxVal = float.MinValue;
        //if so select random
        if (temp == qValues[0] * qValues.Count)
        {
            maxIndex = indices[Random.Range(0, qValues.Count)];
            maxVal = qValues[0];
        }
        else
        {
            //else select the biggest one
            for (int i = 0; i < qValues.Count; i++)
            {
                if (qValues[i] > maxVal)
                {
                    maxVal = qValues[i];
                    maxIndex = indices[i];
                }
            }
        }
        qValues.Clear();
        indices.Clear();

        go(maxIndex);
    }
    
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (maxEpoch == -1 || epochCounter < maxEpoch)
        {
            if (timer <= 0 && !flag)
            {
                if (stopLearningAt == -1 || epochCounter < stopLearningAt)
                    learn();
                else
                    walk();
                lilFlag = true;
                timer = 1 / step_per_second;
                if (flag)//if character reaches the end
                {
                    //wait for a bit
                    timer = waitingTimeAtTheEnd;
                }
            }
            if (lilFlag)//if our character decided to move then move it on screen
            {
                float distCovered = Time.deltaTime * step_per_second;
                float fractionOfJourney = distCovered / Vector3.Distance(character.transform.position, new Vector3(target_x, target_y, 0))*16;
                character.transform.position = Vector3.Lerp(character.transform.position, new Vector3(target_x, target_y, 0), fractionOfJourney);
            }
            if (timer <= 0 && flag)//when the character reaches end
            {
                //send it to start
                character.transform.position = new Vector3(dungeon.start_x, dungeon.start_y, 0);
                //change the marker
                curVertice = dungeon.start_i * dungeon.getMaze_length() + dungeon.start_j;
                //set the scene again
                flag = false;
                timer = 1 / step_per_second;
                lilFlag = false;
            }
        }
        else if (lastFlag)//if programme reaches the limit of execution(epoch)
        {
            //Then print out the q table
            FileStream fs = new FileStream("qTable.txt", FileMode.OpenOrCreate,FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            string st = "";
            for (int i = 0; i < tableLength; i++)
            {
                st = "";
                for (int j = 0; j < tableLength; j++)
                {
                    st += qTable[i, j] + " ";
                }
                Debug.Log(st);
                sw.WriteLine(st);
            }
            sw.Close();
            lastFlag = false;
        }
    }
}
