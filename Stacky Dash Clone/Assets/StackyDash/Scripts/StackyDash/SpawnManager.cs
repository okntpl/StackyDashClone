using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.SceneManagement;

public class SpawnManager : SuperClass
{
     static GameObject grids;
     static GameObject bridges;
     static GameObject floors;
     static GameObject stacks;
     static GameObject bonuses;
     static GameObject finish;
     static GameObject podium;
     static GameObject holes;

    static Vector3 lastStackPos;
    static int firstStackPos;
    

    static Vector3 lastBridgePos=Vector3.zero;
 
  
  

    public static void Spawn(Section section, int index, int sectionSize)
    {

       GameObject obj2 = Resources.Load("Ground",typeof(GameObject))as GameObject;
        Debug.Log(obj2.name);
      

          lastStackPos = Vector3.zero;
          grids = new GameObject($"Grids" + index);
          bridges = new GameObject($"Bridge" + index);
          floors = new GameObject($"Floor" + index);
          stacks = new GameObject($"Stack" + index);
          holes = new GameObject($"Holes" + index);

       if (index != 1)
        {
            lastBridgePos = FindLastBridge(index);
            firstStackPos = FindFirstStack(section);
            lastBridgePos.y = 0;
        }
     
        else
        {
            lastBridgePos = Vector3.zero;
            firstStackPos = 0;
        }

        float posX = (float)(section.size.x / 2f);
        Debug.Log(posX +" "+firstStackPos);
        float posY = (section.size.y);

        for (int i = 0; i < section.size.x; i++)
        {
            for (int j = 0; j < section.size.y; j++)
            {
              
                var obj = MonoBehaviour.Instantiate(Resources.Load("Ground", typeof(GameObject)) as GameObject);
                obj.transform.position = new Vector3((i-firstStackPos+lastBridgePos.x), 0, (posY - j)+lastBridgePos.z);
            

                obj.transform.SetParent(grids.transform);
            }
        }

    
     
       for (int i = 0; i < section.size.x; i++)
        {
            for (int j = 0; j < section.size.y; j++)
            {
                if (section.grid[i, j] == Section.Grid.Floor)
                {
                    var obj = MonoBehaviour.Instantiate(Resources.Load("Floor", typeof(GameObject)) as GameObject);
                    obj.transform.position = new Vector3(i-firstStackPos+lastBridgePos.x, DEFAULT_FLOOR_Y, (posY-j)+lastBridgePos.z);
                
                    obj.transform.SetParent(floors.transform);
                 
                }
             else   if (section.grid[i, j] == Section.Grid.Stack)
                {
                    var obj = MonoBehaviour.Instantiate(Resources.Load("Stack", typeof(GameObject)) as GameObject);
                    obj.transform.position = new Vector3(i - firstStackPos+lastBridgePos.x, DEFAULT_STACK_Y, (posY - j)+lastBridgePos.z);
             

                    obj.transform.SetParent(stacks.transform);

                   
                    if(j==0)
                    {
                        lastStackPos = obj.transform.position;
                    }
                }

              else  if (section.grid[i, j] == Section.Grid.FR)
                {
                    var obj = MonoBehaviour.Instantiate(Resources.Load("ForwardRight", typeof(GameObject)) as GameObject);
                    obj.transform.position = new Vector3(i - firstStackPos + lastBridgePos.x, DEFAULT_FLOOR_Y, (posY - j) + lastBridgePos.z);

                    obj.transform.SetParent(stacks.transform);

                }

              else  if (section.grid[i, j] == Section.Grid.FL)
                {
                    var obj = MonoBehaviour.Instantiate(Resources.Load("ForwardLeft", typeof(GameObject)) as GameObject);
                    obj.transform.position = new Vector3(i - firstStackPos + lastBridgePos.x, DEFAULT_FLOOR_Y, (posY - j) + lastBridgePos.z);

                    obj.transform.SetParent(stacks.transform);

                }
             else   if (section.grid[i, j] == Section.Grid.DR)
                {
                    var obj = MonoBehaviour.Instantiate(Resources.Load("DownRight", typeof(GameObject)) as GameObject);
                    obj.transform.position = new Vector3(i - firstStackPos + lastBridgePos.x, DEFAULT_FLOOR_Y, (posY - j) + lastBridgePos.z);

                    obj.transform.SetParent(stacks.transform);

                }
             else   if (section.grid[i, j] == Section.Grid.DL)
                {
                    var obj = MonoBehaviour.Instantiate(Resources.Load("DownLeft", typeof(GameObject)) as GameObject);
                    obj.transform.position = new Vector3(i - firstStackPos + lastBridgePos.x, DEFAULT_FLOOR_Y, (posY - j) + lastBridgePos.z);

                    obj.transform.SetParent(stacks.transform);

                }

                else if (section.grid[i, j] == Section.Grid.Hole)
                {
                    var obj = MonoBehaviour.Instantiate(Resources.Load("Hole", typeof(GameObject)) as GameObject);
                    obj.transform.position = new Vector3(i - firstStackPos + lastBridgePos.x, DEFAULT_FLOOR_Y, (posY - j) + lastBridgePos.z);
                    obj.transform.SetParent(holes.transform);
                  
                }

               


            }

        }

      
     

        if(index==sectionSize)
        {
            //Spawn Bonus Zone
            SpawnFinish(section);
            SpawnBonus(section,section.scoreLenght);
            SpawnPodium(section.scoreLenght);
        }
        else
        {
            //Spawn Bridge
            Vector3 movePos;
            float randx = 0;


            Vector3 startPos = lastStackPos;
           
            for (int i = 0; i < section.bridgeLenght; i++)
            {

                movePos.x = randx;
                var obj = MonoBehaviour.Instantiate(Resources.Load("Bridge", typeof(GameObject)) as GameObject);
                obj.transform.position = new Vector3(startPos.x + movePos.x, DEFAULT_BRIDGE_Y, (startPos.z) + i + 1);
                obj.transform.SetParent(bridges.transform);

                section.startPos = obj.transform.position;
                randx = Random.Range(0f, 0.4f);
            }
        }

        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

    }

 

    static int FindFirstStack(Section section)
    {
        for (int i = 0; i < section.size.x; i++)
        {
            if (section.grid[i, section.size.y-1] == Section.Grid.Stack)
                return i;
        }
        return -1;
       
    }

    static Vector3 FindLastBridge(int index)
    {
        var lastBridge = GameObject.Find($"Bridge" + (index - 1));
        return lastBridge.transform.GetChild(lastBridge.transform.childCount-1).transform.localPosition;
        
    }


    static void SpawnFinish(Section section)
    {
        finish = new GameObject("Finish");
        float posX = (section.size.x / 2);
        float posY = (section.size.y);
        int colorindex=0;


        Vector3 startPos = lastStackPos;



        for (int i = 0; i < section.size.x; i++)
        {
            for (int j = 0; j < section.size.y; j++)
            {
                var obj = MonoBehaviour.Instantiate(Resources.Load("Finish", typeof(GameObject)) as GameObject);
                obj.transform.position = new Vector3((i - posX+lastStackPos.x), DEFAULT_FINISH_Y, (posY - j) + startPos.z);
                obj.transform.SetParent(finish.transform);
                if (obj.transform.position.x == lastStackPos.x)
                {
                    var bMaterial = new Material(obj.GetComponent<Renderer>().sharedMaterial);
                    if (colorindex % 2 == 0)
                    {
                        bMaterial.color = Color.black;
                    }
                    else
                    {
                        bMaterial.color = Color.white;
                    }
                    obj.GetComponent<Renderer>().sharedMaterial = bMaterial;
                    colorindex++;
                }
            }
        }
    }


    public static void SpawnBonus(Section section,int lenght)
    {
        float offsetZ=1;
     
        bonuses = new GameObject($"BonusArea");
        Vector3 startPos = GameObject.Find("Finish").transform.GetChild(0).transform.position;
            startPos.x = lastStackPos.x;
        Debug.Log(startPos.x);
        for (int i = 0; i < lenght; i++)
        {

            var obj = MonoBehaviour.Instantiate(Resources.Load("xBonus", typeof(GameObject)) as GameObject);
            obj.transform.position = new Vector3(startPos.x-1, DEFAULT_BONUS_Y, startPos.z + offsetZ);
            obj.transform.SetParent(bonuses.transform);
            offsetZ += 7F;



            for (int j = 0; j < obj.transform.childCount-1; j++)
            {
                var bMaterial = new Material(obj.transform.GetChild(j).GetComponent<Renderer>().sharedMaterial);

                bMaterial.color = SetRainbowColor(i,lenght);
                obj.transform.GetChild(j).GetComponent<Renderer>().sharedMaterial = bMaterial;
            }

            obj.transform.GetChild(obj.transform.childCount - 1).GetChild(0).GetComponent<TextMeshProUGUI>().text = (1f + (0.1f * i)).ToString(".0")+"x";
        }

        
       
    }

    static Color SetRainbowColor(int index,int lenght)
    {
        if (index == 0)
        {
            return Color.white;
        }
        else
        {
            float hVal, sVal, bVal;
            hVal = (float)index / lenght;
            sVal = 0.8f;
            bVal = 0.8f;
            return (Color.HSVToRGB(hVal, sVal, bVal));
        }
    }

    static void SpawnPodium(int scoreLenght)
    {
        podium = new GameObject("Podium");
        var obj = MonoBehaviour.Instantiate(Resources.Load("Podium", typeof(GameObject)) as GameObject);

        Vector3 startPos = GameObject.Find("BonusArea").transform.GetChild(scoreLenght-1).transform.position;

        obj.transform.position = new Vector3(startPos.x, 0.5f, startPos.z +DEFAULT_PODIUM_Z);
        obj.transform.parent = podium.transform;
    }

  
   
    public static void DestroySection(int index)
    {
       

        DestroyImmediate(GameObject.Find($"Grids" + index));
        DestroyImmediate(GameObject.Find($"Bridge" + index));
        DestroyImmediate(GameObject.Find($"Floor" + index));
        DestroyImmediate(GameObject.Find($"Stack" + index));
        DestroyImmediate(GameObject.Find("Finish"));
        DestroyImmediate(GameObject.Find("BonusArea"));
        DestroyImmediate(GameObject.Find("Podium"));
        DestroyImmediate(GameObject.Find($"Holes" + index));

        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

    }

    public static void SetPlayerPos(int row, int column, Section section)
    {
       
        float posX = (section.size.x)/row;
        float posY = (section.size.y);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.transform.position = new Vector3(row, DEFAULT_PLAYER_Y, posY - column);


    }


}
