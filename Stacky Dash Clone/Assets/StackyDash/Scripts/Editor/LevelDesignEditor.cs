using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class LevelDesignEditor : EditorWindow
{
    public SectionList sectionList;
    private int viewIndex = 1;
    Scene scene;

   


    [MenuItem("Window/Level Design")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(LevelDesignEditor));


       
    }

    private void OnEnable()
    {
         
    }


    private bool CheckOnLevel()
    {
        
        scene = SceneManager.GetActiveScene();
        sectionList = (SectionList)AssetDatabase.LoadAssetAtPath("Assets/StackyDash/Scripts/ScriptableObjects/" + scene.name + ".asset", typeof(SectionList));

        if (sectionList == null)

        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(30);
            GUILayout.Label("Opps.. There is no asset on "+scene.name+" Click New Level Button", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("New Level"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = sectionList;
                CreateSectionList();
            }

            return false;
        }
        else
        {
            return true;
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(SceneManager.GetActiveScene().name + " Design", EditorStyles.boldLabel);
      


        if (CheckOnLevel())
        {
           
           

                if (GUILayout.Button("Show Section List"))
                {
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = AssetDatabase.LoadAssetAtPath($"Assets/StackyDash/Scripts/ScriptableObjects/"
                    + scene.name + ".asset",
                    typeof(SectionList)) as SectionList;
                }





            GUILayout.EndHorizontal();




            if (sectionList.sections.Count > 0)
            {
                GUIStyle tableStyle = new GUIStyle("box");
                tableStyle.padding = new RectOffset(10, 10, 10, 10);
                tableStyle.margin.left = 32;

                GUIStyle enumStyle = new GUIStyle("popup");
                GUIStyle stackStyle1 = new GUIStyle("popup");
                GUIStyle playerStyle = new GUIStyle("popup");
                GUIStyle moveStyle = new GUIStyle("popup");
                GUIStyle holeStyle = new GUIStyle("popup");

                stackStyle1.fontStyle = FontStyle.Bold;
                stackStyle1.normal.textColor = Color.red;

                playerStyle.fontStyle = FontStyle.Bold;
                playerStyle.normal.textColor = Color.cyan;

                moveStyle.fontStyle = FontStyle.Bold;
                moveStyle.normal.textColor = Color.green;

                holeStyle.fontStyle = FontStyle.Bold;
                holeStyle.normal.textColor = Color.yellow;


                GUILayout.BeginHorizontal();

                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Item", viewIndex, GUILayout.ExpandWidth(false)), 1, sectionList.sections.Count);
                EditorGUILayout.LabelField("of   " + sectionList.sections.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));

                GUILayout.EndHorizontal();



                GUILayout.BeginHorizontal();

                sectionList.sections[viewIndex - 1].size = (Vector2Int)EditorGUILayout.Vector2IntField("Grid Size", sectionList.sections[viewIndex - 1].size, GUILayout.ExpandWidth(false));


              


                EditorGUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(tableStyle);
                for (int row = 0; row < sectionList.sections[viewIndex - 1].size.x; row++)
                {

                    GUILayout.BeginVertical();
                    for (int column = 0; column < sectionList.sections[viewIndex - 1].size.y; column++)
                    {
                        EditorGUILayout.BeginHorizontal();

                        if (sectionList.sections[viewIndex - 1].grid[row, column] == Section.Grid.Stack)
                        {
                            sectionList.sections[viewIndex - 1].grid[row, column] =
                               (Section.Grid)EditorGUILayout.EnumPopup(sectionList.sections[viewIndex - 1].grid[row, column], stackStyle1);
                        }

                        else if (sectionList.sections[viewIndex - 1].grid[row, column] == Section.Grid.PlayerPos)
                        {
                            sectionList.sections[viewIndex - 1].grid[row, column] =
                               (Section.Grid)EditorGUILayout.EnumPopup(sectionList.sections[viewIndex - 1].grid[row, column], playerStyle);
                            SpawnManager.SetPlayerPos(row, column, sectionList.sections[viewIndex - 1]);

                        }

                        else if (sectionList.sections[viewIndex - 1].grid[row, column] == Section.Grid.Floor)
                        {
                            sectionList.sections[viewIndex - 1].grid[row, column] =
                               (Section.Grid)EditorGUILayout.EnumPopup(sectionList.sections[viewIndex - 1].grid[row, column], enumStyle);

                        }

                        else if (sectionList.sections[viewIndex - 1].grid[row, column] == Section.Grid.Hole)
                        {
                            sectionList.sections[viewIndex - 1].grid[row, column] =
                               (Section.Grid)EditorGUILayout.EnumPopup(sectionList.sections[viewIndex - 1].grid[row, column], holeStyle);
                          

                        }


                        else
                        {
                            sectionList.sections[viewIndex - 1].grid[row, column] =
                               (Section.Grid)EditorGUILayout.EnumPopup(sectionList.sections[viewIndex - 1].grid[row, column], moveStyle);
                        }

                        EditorGUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("FR,FL,DL,DR Means : AUTO MOVE directions For example: FR = Forward and right");
                EditorGUILayout.EndHorizontal();




                if (GUILayout.Button("Reset Grid"))
                {
                    for (int row = 0; row < sectionList.sections[viewIndex - 1].size.x; row++)
                    {


                        for (int column = 0; column < sectionList.sections[viewIndex - 1].size.y; column++)
                        {

                            sectionList.sections[viewIndex - 1].grid[row, column] = Section.Grid.Floor;

                        }
                    }
                }



               


                if (viewIndex != sectionList.sections.Count)
                {
                   

                    GUILayout.BeginHorizontal();
                    sectionList.sections[viewIndex - 1].bridgeLenght = (int)EditorGUILayout.IntField("Bridge Lenght", sectionList.sections[viewIndex - 1].bridgeLenght, GUILayout.ExpandWidth(false));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    sectionList.sections[viewIndex - 1].bridgeDirection =
                             (Section.BridgeDirection)EditorGUILayout.EnumPopup(sectionList.sections[viewIndex - 1].bridgeDirection, enumStyle);
                    GUILayout.EndHorizontal();

                }





                if (viewIndex == sectionList.sections.Count)
                {
                    GUILayout.Space(30);
                    GUILayout.BeginVertical();

                   

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    sectionList.sections[viewIndex - 1].scoreLenght = (int)EditorGUILayout.IntField("xScore Lenght", sectionList.sections[viewIndex - 1].scoreLenght, GUILayout.ExpandWidth(false));
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();



                    GUILayout.BeginHorizontal();
                    for (int i = 0; i < sectionList.sections[viewIndex - 1].scoreLenght; i++)
                    {

                        GUILayout.Label($"x" + (1f + 0.1f * i));

                    }

                    GUILayout.EndHorizontal();


                }
  

                if (GUILayout.Button($"Build Section " + viewIndex))
                {


                    SpawnManager.Spawn(sectionList.sections[viewIndex - 1], viewIndex, sectionList.sections.Count);
                }

                if (GUILayout.Button($"Destroy Section " + viewIndex))
                {
                    SpawnManager.DestroySection(viewIndex);
                }


                GUILayout.Space(30);

                GUILayout.BeginHorizontal();

                if (viewIndex == sectionList.sections.Count)
                {
                    

                    sectionList.sections[viewIndex - 1].groundColor = (Color)EditorGUILayout.ColorField("Ground Color", sectionList.sections[viewIndex - 1].groundColor, GUILayout.ExpandWidth(false));

                    if (GUILayout.Button($"Colorize Grounds"))
                    {
                        ColorManager.ColorizeGrid(viewIndex, sectionList.sections[viewIndex - 1].groundColor);
                    }

                    GUILayout.EndHorizontal();



                    GUILayout.BeginHorizontal();

                    sectionList.sections[viewIndex - 1].floorColor = (Color)EditorGUILayout.ColorField("Floor Color", sectionList.sections[viewIndex - 1].floorColor, GUILayout.ExpandWidth(false));
                    if (GUILayout.Button($"Colorize Floors"))
                    {
                        ColorManager.ColorizeFloor(viewIndex, sectionList.sections[viewIndex - 1].floorColor);
                    }

                    GUILayout.EndHorizontal();



                    GUILayout.BeginHorizontal();

                    sectionList.sections[viewIndex - 1].stackColor = (Color)EditorGUILayout.ColorField("Stack Color", sectionList.sections[viewIndex - 1].stackColor, GUILayout.ExpandWidth(false));
                    if (GUILayout.Button($"Colorize Stacks"))
                    {
                        ColorManager.ColorizeStack(viewIndex, sectionList.sections[viewIndex - 1].stackColor);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    sectionList.sections[viewIndex - 1].bridgeColor = (Color)EditorGUILayout.ColorField("Bridge Color", sectionList.sections[viewIndex - 1].bridgeColor, GUILayout.ExpandWidth(false));
                    if (GUILayout.Button($"Colorize Bridges"))
                    {
                        ColorManager.ColorizeBridge(viewIndex, sectionList.sections[viewIndex - 1].bridgeColor);
                    }
                }
                GUILayout.EndHorizontal();


                    if (GUILayout.Button("Save & Exit"))
                    {
                        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                         this.Close();
                    }
                    
                }

                GUILayout.EndVertical();
           




        }

           
        
        GUIUtility.ExitGUI();

    }


    void CreateSectionList()
    {
        viewIndex = 1;
       sectionList=CreateSection.Create();
        
        if (sectionList)
        {
            sectionList.sections = new List<Section>();
            string relPath = AssetDatabase.GetAssetPath(sectionList);
            Debug.Log(relPath);
            EditorPrefs.SetString("ObjectPath", relPath);
            
        }

        //Design only Active Scene Level to avoid irregularity
   /*     sectionList = AssetDatabase.LoadAssetAtPath($"Assets/StackyDash/Scripts/ScriptableObjects/"
                + SceneManager.GetActiveScene().name + ".asset",
                typeof(SectionList)) as SectionList;
   */
    }
}
