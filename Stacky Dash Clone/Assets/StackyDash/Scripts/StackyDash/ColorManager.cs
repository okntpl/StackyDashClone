using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class ColorManager : MonoBehaviour
{
    static GameObject grids;
    static GameObject bridges;
    static GameObject floors;
    static GameObject stacks;

    public static void ColorizeGrid(int index,Color gColor)
    {
        for (int j = index; j > 0; j--)
        {

            grids = GameObject.Find($"Grids" + j);



            for (int i = 0; i < grids.transform.childCount; i++)
            {
                var bMaterial = new Material(grids.transform.GetChild(i).GetComponent<Renderer>().sharedMaterial);
                bMaterial.color = gColor;
                grids.transform.GetChild(i).GetComponent<Renderer>().sharedMaterial = bMaterial;
            }
        }

        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
    }

    public static void ColorizeFloor(int index, Color gColor)
    {
        for (int j = index; j > 0; j--)
        {

            floors = GameObject.Find($"Floor" + j);



            for (int i = 0; i < floors.transform.childCount; i++)
            {
                var bMaterial = new Material(floors.transform.GetChild(i).GetComponent<Renderer>().sharedMaterial);
                bMaterial.color = gColor;
                floors.transform.GetChild(i).GetComponent<Renderer>().sharedMaterial = bMaterial;
            }
        }
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
    }

    public static void ColorizeStack(int index, Color gColor)
    {
        for (int j = index; j > 0; j--)
        {
            stacks = GameObject.Find($"Stack" + j);

            for (int i = 0; i < stacks.transform.childCount; i++)
            {
                var bMaterial = new Material(stacks.transform.GetChild(i).GetComponent<Renderer>().sharedMaterial);
                bMaterial.color = gColor;
                stacks.transform.GetChild(i).GetComponent<Renderer>().sharedMaterial = bMaterial;
            }

        }

        var sMaterial = new Material(stacks.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial);
        sMaterial.color = gColor;

        GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetChild(0).GetComponent<Renderer>().sharedMaterial = sMaterial;

       




        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
    }
    public static void ColorizeBridge(int index, Color gColor)
    {
        for (int j = index; j > 0; j--)
        {

            bridges = GameObject.Find($"Bridge" + j);



            for (int i = 0; i < bridges.transform.childCount; i++)
            {
                var bMaterial = new Material(bridges.transform.GetChild(i).GetComponent<Renderer>().sharedMaterial);
                bMaterial.color = gColor;
                bridges.transform.GetChild(i).GetComponent<Renderer>().sharedMaterial = bMaterial;
            }
        }
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
    }



}
