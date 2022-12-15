using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MazeGenerator : MonoBehaviour
{
    private int columnLength, rowLength, blockWidth;
    public GameObject[] mazeBlocksToGenerate; //order needs to match order in VR project!!
    Transform[,] mazeBlocks;
    public GameObject playerBlock, finishBlock, cornerBlock;
    public float moveTime = 0.5f;
    public static MazeGenerator instance { get; private set; }
    public GameObject btnPrefab;
    public GameObject rotateBtn;
    public Transform mazeUI;
    public Transform nextBlock;
    Vector3 nextBlockPos;
    public Button[,] columnBtns, rowBtns;
    int columnIdx, rowIdx;
    public GameObject[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

    }
    public void generateMaze(int columnLength, int rowLength, int blockWidth, int[] blockIdxs, int[] blockRotations)
    {
        this.columnLength = columnLength;
        this.rowLength = rowLength;
        this.blockWidth = blockWidth;
        mazeBlocks = new Transform[rowLength, columnLength];
        mazeBlocks[0, 0] = Instantiate(playerBlock, transform).transform;
        PositionManager.instance.player = mazeBlocks[0, 0].transform.Find("Player");
        mazeBlocks[rowLength - 1, columnLength - 1] = Instantiate(finishBlock, new Vector3((rowLength - 1) * blockWidth, 0, (columnLength - 1) * blockWidth), Quaternion.identity, transform).transform;
        enemies = new GameObject[columnLength * rowLength];
        Debug.Log(blockRotations.Length);
        Debug.Log(blockIdxs.Length);
        //place blocks on grid
        for (int i = 1; i < columnLength * rowLength - 1; i++)
        {
            Vector3 pos = new Vector3(i / columnLength * blockWidth, 0, i % columnLength * blockWidth);
            if (i / columnLength == rowLength - 1 && i % columnLength == 0)
            { //left upper corner block
                Debug.Log("cornerblock");
                Instantiate(cornerBlock, pos, Quaternion.Euler(0, 90, 0), transform);

            }
            else if (i % columnLength == columnLength - 1 && i / columnLength == 0)
            {
                Instantiate(cornerBlock, pos, Quaternion.Euler(0, -90, 0), transform);
            }

            else
            {
                Debug.Log(blockIdxs[i - 1]);
                GameObject block = Instantiate(mazeBlocksToGenerate[blockIdxs[i - 1]], pos, Quaternion.Euler(0, blockRotations[i - 1] * 90, 0), transform);
                mazeBlocks[i / columnLength, i % columnLength] = block.transform;
                if (block.transform.Find("enemy"))
                {
                    enemies[i]=(block.transform.Find("enemy").gameObject);
                }
            }

        }

        //place camera
        UnityEngine.Camera.main.transform.position = new Vector3(rowLength / 2 * blockWidth, UnityEngine.Camera.main.transform.position.y, columnLength / 2 * blockWidth);

        //place movement btns 
        //for rows

        columnBtns = new Button[rowLength, 2];
        rowBtns = new Button[columnLength, 2];

        for (int i = 1; i < columnLength - 1; i++)
        {
            //right
            columnBtns[i, 0] = createBtn(blockWidth, new Vector3(-blockWidth, 0, i * blockWidth), Quaternion.Euler(90, 0, -90), i, true, false).GetComponent<Button>();
            //left
            columnBtns[i, 1] = createBtn(blockWidth, new Vector3(rowLength * blockWidth, 0, i * blockWidth), Quaternion.Euler(90, 0, 90), i, true, true);
        }
        //for columns
        for (int i = 1; i < rowLength - 1; i++)
        {
            //up
            rowBtns[i, 0] = createBtn(blockWidth, new Vector3(i * blockWidth, 0, -blockWidth), Quaternion.Euler(90, 0, 0), i, false, false);
            //down
            rowBtns[i, 1] = createBtn(blockWidth, new Vector3(i * blockWidth, 0, columnLength * blockWidth), Quaternion.Euler(90, 0, 180), i, false, true);
        }

        //place next block
        nextBlockPos = new Vector3((rowLength + 2) * blockWidth, 0, ((columnLength / 2) * blockWidth));
        nextBlock = Instantiate(mazeBlocksToGenerate[0], nextBlockPos, Quaternion.Euler(0, 0, 0), transform).transform;
        //nextBlock.localScale = Vector3.one * blockWidth;

        Instantiate(rotateBtn, new Vector3(nextBlockPos.x + blockWidth, 0, nextBlockPos.z), Quaternion.Euler(90, 0, 0), mazeUI);

        Debug.Log("btns created");
        Inventory.instance.Init();
    }
    Button createBtn(int blockWidth, Vector3 pos, Quaternion rot, int index, bool isRow, bool moveNegDir)
    {
        var btn = Instantiate(btnPrefab, pos, rot, mazeUI);
        btn.transform.localScale = Vector3.one * blockWidth;
        btn.GetComponent<moveBtn>().Init(index, isRow, moveNegDir);
        return btn.GetComponent<Button>();
    }
    public void move(int index, bool isRow, bool moveNegDir)
    {

        if ((nextBlock.position == nextBlockPos))
        {
            //movePlayerWithMaze();
            if (moveNegDir) moveInNegativeDir(isRow, index, 0);
            else moveInPositiveDir(isRow, index, 0);
            Net_MoveMazeMsg msg = new Net_MoveMazeMsg(index, isRow, moveNegDir);
            Debug.Log(index + " " + isRow + moveNegDir);
            BaseClient.instance.SendToServer(msg);
        }
    }
    bool isPlayerInMovement(int index, bool isRow)
    {
        if (isRow) return rowIdx == index;
        else return columnIdx == index;
    }
    void disableBtnsWithPlayer()
    {
        var columnIdxNew = (int)((PositionManager.instance.player.position.x + blockWidth / 2) / blockWidth);
        var rowIdxNew = (int)((PositionManager.instance.player.position.z + blockWidth / 2) / blockWidth);
        if (columnIdxNew == columnIdx && rowIdxNew == rowIdx) return;
        if (columnIdxNew != columnIdx && columnIdx > 0 && columnIdx < columnLength && rowBtns[columnIdx, 0] != null)
            rowBtns[columnIdx, 0].interactable = rowBtns[columnIdx, 1].interactable = true;
        if (rowIdxNew != rowIdx && rowIdx > 0 && rowIdx < rowLength && columnBtns[rowIdx, 0] != null)
            columnBtns[rowIdx, 0].interactable = columnBtns[rowIdx, 1].interactable = true;
        columnIdx = columnIdxNew;
        rowIdx = rowIdxNew;

        if (columnIdx > 0 && columnIdx < columnLength && rowBtns[columnIdx, 0] != null)
            rowBtns[columnIdx, 0].interactable = rowBtns[columnIdx, 1].interactable = false;
        if (rowIdx > 0 && rowIdx < rowLength && columnBtns[rowIdx, 0] != null)
            columnBtns[rowIdx, 0].interactable = columnBtns[rowIdx, 1].interactable = false;

    }
    // Update is called once per frame
    void Update()
    {
        //Disable btns for row/column player is on
        if (PositionManager.instance.player && rowBtns.Length > 0)
            disableBtnsWithPlayer();



        if(Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("restart");
                // destroy maze
                for (var i = 0; i < this.transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
                //destroy maze btns
                for (var i = 0; i < mazeUI.childCount; i++)
                {
                    Destroy(mazeUI.GetChild(i).gameObject);
                }
                BaseClient.instance.SendToServer(new Net_MsgCode(actionTypeCode.RESTART));

                //reset inventory;
                Inventory.instance.Reset();
                WindowUIManager.instance.Reset();

            }
        }
        //if (Input.GetMouseButtonDown(0) && BaseClient.instance.isConnected)
        //{
        //    //only for testing, needs to be improved once we know how we do the interaction (table/tablet..)
        //    Vector3 screenPos = Input.mousePosition;
        //    screenPos.z = UnityEngine.Camera.main.transform.position.y;
        //    Vector3 pos = UnityEngine.Camera.main.ScreenToWorldPoint(screenPos);
        //    var column = (int)((pos.x + blockWidth / 2) / blockWidth);
        //    var row = (int)((pos.z + blockWidth / 2) / blockWidth);
        //    var newBlockIdx = UnityEngine.Random.Range(0, mazeBlocksToGenerate.Length); //should be selected by the user in the future
        //    if (pos.x + blockWidth / 2 < 0 && row < columnLength)//move row right
        //        moveInPositiveDir(true, row, newBlockIdx);
        //    else if ((pos.x + blockWidth / 2) / blockWidth > rowLength && row < columnLength) //move row left
        //        moveInNegativeDir(true, row, newBlockIdx);
        //    else if (pos.z + blockWidth / 2 < 0 && column < rowLength)//move column up
        //        moveInPositiveDir(false, column, newBlockIdx);
        //    else if (((pos.z + blockWidth / 2) / blockWidth > columnLength) && (column < rowLength))    //move column down
        //        moveInNegativeDir(false, column, newBlockIdx);
        //}
    }
    void moveInPositiveDir(bool isRow, int idx, int newBlockId)//right or up
    {
        for (int i = (isRow ? rowLength : columnLength) - 1; i >= 0; i--)
        {
            var block = isRow ? mazeBlocks[i, idx] : mazeBlocks[idx, i];
            var moveVector = isRow ? new Vector3(blockWidth, 0, 0) : new Vector3(0, 0, blockWidth);
            var tween = block.DOMove(block.position + moveVector, moveTime);
            if (isLastBlock(isRow, i, false))//delete last block
                tween.OnComplete(() =>
                {
                    nextBlock = block;
                    nextBlock.position = nextBlockPos;
                });
            else
            {
                if (isRow) mazeBlocks[i + 1, idx] = block;
                else mazeBlocks[idx, i + 1] = block;
            }
        }
        placeNewBlock(idx, isRow, false);

    }
    void moveInNegativeDir(bool isRow, int idx, int newBlockId)//left or down
    {
        for (int i = 0; i < (isRow ? rowLength : columnLength); i++)
        {
            var block = isRow ? mazeBlocks[i, idx] : mazeBlocks[idx, i];
            var moveVector = isRow ? new Vector3(-blockWidth, 0, 0) : new Vector3(0, 0, -blockWidth);
            var tween = block.DOMove(block.position + moveVector, moveTime);
            if (isLastBlock(isRow, i, true))//delete last block
                tween.OnComplete(() =>
                {
                    nextBlock = block;
                    nextBlock.position = nextBlockPos;
                });
            else
            {
                if (isRow) mazeBlocks[i - 1, idx] = block;
                else mazeBlocks[idx, i - 1] = block;
            }
        }
        placeNewBlock(idx, isRow, true);

    }
    bool isLastBlock(bool isRow, int idx, bool moveNegDirt)
    {
        if (isRow) return idx == rowLength - 1 && !moveNegDirt || idx == 0 && moveNegDirt;
        else return idx == columnLength - 1 && !moveNegDirt || idx == 0 && moveNegDirt;
    }

    void placeNewBlock(int idx, bool inRow, bool moveNegDirt)
    {
        Vector3 pos;
        if (inRow) pos = new Vector3(moveNegDirt ? rowLength * blockWidth : -blockWidth, 0, idx * blockWidth);
        else pos = new Vector3(idx * blockWidth, 0, moveNegDirt ? columnLength * blockWidth : -blockWidth);
        //var newBlock = Instantiate(mazeBlocksToGenerate[newBlockId], pos, Quaternion.identity);
        nextBlock.position = pos;
        var moveVector = inRow ? new Vector3((moveNegDirt ? -1 : 1) * blockWidth, 0, 0) : new Vector3(0, 0, (moveNegDirt ? -1 : 1) * blockWidth);
        var tween = nextBlock.transform.DOMove(nextBlock.transform.position + moveVector, moveTime);
        if (inRow) mazeBlocks[moveNegDirt ? rowLength - 1 : 0, idx] = nextBlock.transform;
        else mazeBlocks[idx, moveNegDirt ? columnLength - 1 : 0] = nextBlock.transform;
    }
    void movePlayerWithMaze()
    {
        var column = (int)((PositionManager.instance.player.position.x + blockWidth / 2) / blockWidth);
        var row = (int)((PositionManager.instance.player.position.z + blockWidth / 2) / blockWidth);
        Debug.Log(column + " " + row);
        PositionManager.instance.player.parent = mazeBlocks[column, row];
    }
    public void rotateBlock()
    {
        nextBlock.Rotate(0, 90, 0);
        BaseClient.instance.SendToServer(new Net_MsgCode(actionTypeCode.ROTATE));

    }
    public void killEnemy(int index)
    {
        Debug.Log("kill enemy");
        //var column = (int)((posX + blockWidth / 2) / blockWidth);
        //var row = (int)((posZ + blockWidth / 2) / blockWidth);
        Destroy(enemies[index]);
    }
}
