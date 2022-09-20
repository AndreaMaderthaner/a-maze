using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int columnLength, rowLength;
    public GameObject prefab;
    public int blockWidth;
    [Serializable]
    public struct MazeBlock
    {
        public int quantity;
        public GameObject prefab;
    }
    public MazeBlock[] mazeBlocks;
    // Start is called before the first frame update
    void Start()
    {
        //create array with all the blocks we want to use
        var allMazeBlocks = new GameObject[columnLength * rowLength];
        int idx = 0;
        foreach (MazeBlock mazeBlock in mazeBlocks)
        {
            for(int i =0; i<mazeBlock.quantity; i++)
            {
                if (idx < columnLength * rowLength)
                {
                    allMazeBlocks[idx] = mazeBlock.prefab;
                    idx++;
                }
                else
                    Debug.LogWarning("number of maze blocks is bigger than cell amount, please fix!");
            }
        }
        if(idx< columnLength * rowLength)
            Debug.LogWarning("number of maze blocks is smaller than cell amount, please fix!");

        //randomize array
        for (int i = 0; i < allMazeBlocks.Length - 1; i++)
        {
            int rnd = UnityEngine.Random.Range(i, allMazeBlocks.Length);
           var temp = allMazeBlocks[rnd];
            allMazeBlocks[rnd] = allMazeBlocks[i];
            allMazeBlocks[i] = temp;
        }

        //place blocks on grid
        for (int i = 0; i < columnLength * rowLength; i++)
        {
            Instantiate(allMazeBlocks[i], new Vector3(i % columnLength * blockWidth, i / columnLength * blockWidth, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
