using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public static class Consts
{
    public const int defaultSize = 20;
}

[System.Serializable]
public class Section
{
    public enum Grid
    {
        Floor,
        Stack,
        PlayerPos,
        FL,
        FR,
        DL,
        DR,
        Hole,
      
    }

    public enum BridgeDirection
    {
        Forward,
       
    }




    #region Public

    public Vector2Int size;

    [HideInInspector]
    public Vector3 startPos;

    public Grid[,] grid = new Grid[Consts.defaultSize, Consts.defaultSize];
    public BridgeDirection bridgeDirection;

    public Color groundColor;
    public Color floorColor;
    public Color stackColor;
    public Color bridgeColor;

    public int bridgeLenght;
    public int scoreLenght;
    #endregion

       
}


