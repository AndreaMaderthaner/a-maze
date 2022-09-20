using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int columnLength, rowLength;
    public int blockWidth;
    [Serializable]
    public struct MazeBlock
    {
        public int quantity;
        public GameObject prefab;
    }
    public MazeBlock[] mazeBlocksToGenerate;
    public GameObject[] placementBlocks;
    Transform[,] mazeBlocks;
    public float moveTime=1.0f;
    // Start is called before the first frame update
    void Start()
    {
        mazeBlocks = new Transform[rowLength, columnLength];
        //create array with all the blocks we want to use
        var allMazeBlocks = new GameObject[columnLength * rowLength];
        int idx = 0;
        foreach (MazeBlock mazeBlock in mazeBlocksToGenerate)
        {
            for (int i = 0; i < mazeBlock.quantity; i++)
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
        if (idx < columnLength * rowLength)
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
            GameObject block = Instantiate(allMazeBlocks[i], new Vector3(i / columnLength * blockWidth, 0, i % columnLength * blockWidth), Quaternion.identity);
            mazeBlocks[i / columnLength, i % columnLength] = block.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            moveRow(0, false,0);
        }
    }
    void moveRow(int rowIdx, bool moveLeft, int newBlockId)
    {
        for (int i = rowLength - 1; i >= 0; i--)
        {
            Debug.Log(i);
            var block = mazeBlocks[i, rowIdx];
            var tween = block.DOMove(block.position + new Vector3((moveLeft ? -1:1)*blockWidth, 0, 0), 1);
            if (i == rowLength - 1 && !moveLeft || i == 0 && moveLeft)//delete last block
                tween.OnComplete(() => Destroy(block.gameObject));
            else mazeBlocks[i + 1, rowIdx] = block;
        }
        placeNewBlock(rowIdx, true,  moveLeft, newBlockId);

    }
    void placeNewBlock(int idx, bool inRow, bool moveLeft, int newBlockId)
    {
        var pos = new Vector3(moveLeft ?  rowLength * blockWidth : -blockWidth, 0,idx*blockWidth);
        var newBlock = Instantiate(placementBlocks[newBlockId], pos, Quaternion.identity);
        mazeBlocks[moveLeft?rowLength:0, idx] = newBlock.transform;
        newBlock.transform.DOMove(newBlock.transform.position + new Vector3((moveLeft ? -1 : 1) * blockWidth, 0, 0), 1);
    }
 
}
