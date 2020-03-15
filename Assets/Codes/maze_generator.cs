using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class directions
{
    public static short north = 0, east = 1, south = 2, west = 3;
}
public class cell
{
    private bool[] doors;
    private cell[] neighbours;

    public bool[] getDoors()
    {
        return doors;
    }

    public cell[] getNeighbours()
    {
        return neighbours;
    }

    public int getC_x()
    {
        return c_x;
    }

    public int getC_y()
    {
        return c_y;
    }

    private int c_x;
    private int c_y;

    public bool visited;

    public cell(int x, int y)
    {
        visited = false;
        c_x = x;
        c_y = y;
        doors = new bool[4];
        for (int i = 0; i < 4; i++)
            doors[i] = false;
        neighbours = new cell[4];
    }

    public void set_neighbours(maze dungeon)
    {
        if (c_x != 0)
            neighbours[directions.north] = dungeon.getCells()[c_x - 1][c_y];
        if (c_y != dungeon.getMaze_length() - 1)
            neighbours[directions.east] = dungeon.getCells()[c_x][c_y + 1];
        if (c_x != dungeon.getMaze_length() - 1)
            neighbours[directions.south] = dungeon.getCells()[c_x + 1][c_y];
        if (c_y != 0)
            neighbours[directions.west] = dungeon.getCells()[c_x][c_y - 1];
    }

    public void fix_neighbours()
    {
        if (!doors[directions.north])
            neighbours[directions.north] = null;
        if (!doors[directions.east])
            neighbours[directions.east] = null;
        if (!doors[directions.south])
            neighbours[directions.south] = null;
        if (!doors[directions.west])
            neighbours[directions.west] = null;
    }

    public void openDoor(int direction)
    {
        this.doors[direction] = true;
    }
}
public class maze
{
    private cell[][] cells;
    private int maze_length;
    public int start_i;
    public int start_j;
    public int end_i;
    public int end_j;
    public float start_x;
    public float start_y;

    public int getMaze_length()
    {
        return maze_length;
    }

    public cell[][] getCells()
    {
        return cells;
    }

    public maze(int mazeLength)
    {
        maze_length = mazeLength;
        cells = new cell[mazeLength][];
        for (int i = 0; i < mazeLength; i++)
        {
            cells[i] = new cell[mazeLength];
            for (int j = 0; j < mazeLength; j++)
            {
                cells[i][j] = new cell(i, j);
            }
        }
        for (int i = 0; i < mazeLength; i++)
        {
            for (int j = 0; j < mazeLength; j++)
            {
                cells[i][j].set_neighbours(this);
            }
        }
    }

    public void createPassage(int from_x, int from_y, int to_x, int to_y)
    {
        if (from_x == to_x && from_y < to_y)
        {
            cells[from_x][from_y].openDoor(directions.east);
            cells[to_x][to_y].openDoor(directions.west);
        }
        else if (from_x == to_x && from_y > to_y)
        {
            cells[from_x][from_y].openDoor(directions.west);
            cells[to_x][to_y].openDoor(directions.east);
        }
        else if (from_x < to_x && from_y == to_y)
        {
            cells[from_x][from_y].openDoor(directions.south);
            cells[to_x][to_y].openDoor(directions.north);
        }
        else if (from_x > to_x && from_y == to_y)
        {
            cells[from_x][from_y].openDoor(directions.north);
            cells[to_x][to_y].openDoor(directions.south);
        }
    }
}
public class maze_generator : MonoBehaviour
{
    //settings
    public GameObject[] roomArray;
    public GameObject[] roomArray_end;
    public GameObject[] roomArray_start;
    public GameObject[] markers;
    public bool select_random_start_and_end;
    //datas
    public int mazeLength;
    private maze dungeon;
    private GameObject[][] dungeonObjects;

    public GameObject[][] Get_DungeonObjects
    {
        get
        {
            return dungeonObjects;
        }
    }

    public maze Get_Dungeon
    {
        get
        {
            return dungeon;
        }
    }

    //prims maze generation algorithm
    public maze generate()
    {
        maze dungeon = new maze(mazeLength);
        //Prim's maze generator algorithm
        List<cell> toBeVisited = new List<cell>();
        List<short> choices = new List<short>();
        toBeVisited.Add(dungeon.getCells()[0][0]);
        while (toBeVisited.Count != 0)
        {
            //select randomly
            cell selected = toBeVisited[Random.Range(0, toBeVisited.Count)];
            selected.visited = true;
            //find choices
            if (selected.getC_x() != 0 && selected.getNeighbours()[directions.north].visited)
                choices.Add(directions.north);
            if (selected.getC_y() != dungeon.getMaze_length() - 1 && selected.getNeighbours()[directions.east].visited)
                choices.Add(directions.east);
            if (selected.getC_x() != dungeon.getMaze_length() - 1 && selected.getNeighbours()[directions.south].visited)
                choices.Add(directions.south);
            if (selected.getC_y() != 0 && selected.getNeighbours()[directions.west].visited)
                choices.Add(directions.west);
            //choose where selected will be connected
            if (choices.Count > 0)
            {
                cell toWhere = selected.getNeighbours()[choices[Random.Range(0, choices.Count)]];
                dungeon.createPassage(selected.getC_x(), selected.getC_y(), toWhere.getC_x(), toWhere.getC_y());
            }
            //add unvisited nodes
            if (selected.getC_x() != 0 && !selected.getNeighbours()[directions.north].visited)
                toBeVisited.Add(selected.getNeighbours()[directions.north]);
            if (selected.getC_y() != dungeon.getMaze_length() - 1 && !selected.getNeighbours()[directions.east].visited)
                toBeVisited.Add(selected.getNeighbours()[directions.east]);
            if (selected.getC_x() != dungeon.getMaze_length() - 1 && !selected.getNeighbours()[directions.south].visited)
                toBeVisited.Add(selected.getNeighbours()[directions.south]); ;
            if (selected.getC_y() != 0 && !selected.getNeighbours()[directions.west].visited)
                toBeVisited.Add(selected.getNeighbours()[directions.west]);
            choices.Clear();
            toBeVisited.Remove(selected);
        }
        //Remove unreachable neighbours
        for (int i = 0; i < mazeLength; i++)
        {
            for (int j = 0; j < mazeLength; j++)
            {
                dungeon.getCells()[i][j].fix_neighbours();
            }
        }

        return dungeon;
    }

    //instatiation of maze and indices
    public void instantiate_maze(maze dungeon)
    {
        Transform mazeObject = new GameObject("mazeObject").transform;
        GameObject generator = GameObject.FindGameObjectWithTag("Mg");
        Vector3 startPoint = generator.transform.position;
        if (mazeLength % 2 == 0)
        {
            startPoint.x -= ((mazeLength / 2) - 0.5f)*16;
            startPoint.y += ((mazeLength / 2) - 0.5f)*16;
        }
        else
        {
            startPoint.x -= Mathf.Floor(mazeLength / 2)*16;
            startPoint.y += Mathf.Floor(mazeLength / 2)*16;
        }
        int startIndex_x, startIndex_y, endIndex_x, endIndex_y;
        //Select random start and end point or make them fixed to the most left-up and the most right-down
        if (select_random_start_and_end)
        {
            startIndex_x = Random.Range(0, mazeLength);
            startIndex_y = Random.Range(0, mazeLength);
            endIndex_x = Random.Range(0, mazeLength);
            endIndex_y = Random.Range(0, mazeLength);
            while (startIndex_x == endIndex_x && startIndex_y == endIndex_y)
            {
                endIndex_x = Random.Range(0, mazeLength);
                endIndex_y = Random.Range(0, mazeLength);
            }
        }
        else
        {
            startIndex_x = startIndex_y = 0;
            endIndex_x = endIndex_y = mazeLength - 1;
        }
        dungeon.start_i = startIndex_y;
        dungeon.start_j = startIndex_x;
        dungeon.end_i = endIndex_y;
        dungeon.end_j = endIndex_x;
        dungeon.start_x = startPoint.x + dungeon.start_j;
        dungeon.start_y = startPoint.y - dungeon.start_i;
        for (int i = 0; i < mazeLength; i++)
        {
            for (int j = 0; j < mazeLength; j++)
            {
                Vector3 point = startPoint;
                point.x += j * 16;
                point.y -= i * 16;
                cell room = dungeon.getCells()[i][j];
                int index = 0;
                bool[] doors = room.getDoors();
                if (doors[0]) { index += 8; }
                if (doors[1]) { index += 4; }
                if (doors[2]) { index += 2; }
                if (doors[3]) { index += 1; }
                GameObject newRoom;
                if (i == startIndex_y && j == startIndex_x)
                    newRoom = Instantiate(roomArray_start[index], point, Quaternion.identity) as GameObject;
                else if (i == endIndex_y && j == endIndex_x)
                    newRoom = Instantiate(roomArray_end[index], point, Quaternion.identity) as GameObject;
                else
                    newRoom = Instantiate(roomArray[index], point, Quaternion.identity) as GameObject;
                newRoom.transform.SetParent(mazeObject);
                GameObject marker;
                if (doors[0]) { marker = Instantiate(markers[directions.north], newRoom.transform); }
                if (doors[1]) { marker = Instantiate(markers[directions.east], newRoom.transform); }
                if (doors[2]) { marker = Instantiate(markers[directions.south], newRoom.transform); }
                if (doors[3]) { marker = Instantiate(markers[directions.west], newRoom.transform); }
                dungeonObjects[i][j] = newRoom;
            }
        }
    }
    
    // Use this for initialization
    void Start()
    {
        dungeon = generate();
        dungeonObjects = new GameObject[dungeon.getMaze_length()][];
        for (int i=0;i<dungeon.getMaze_length();i++)
        {
            dungeonObjects[i] = new GameObject[dungeon.getMaze_length()];
        }
        instantiate_maze(dungeon);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
