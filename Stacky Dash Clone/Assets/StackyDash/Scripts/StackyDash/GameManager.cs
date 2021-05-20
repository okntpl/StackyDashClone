using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoSingleton<GameManager>
{
   
    private void OnEnable()
    {
        EventManager.OnGameStateChange += ManageLevel;
    }

    private void OnDisable()
    {
        EventManager.OnGameStateChange -= ManageLevel;
    }


    private void ManageLevel(EventManager.GameStates gameStates)
    {


        if (gameStates == EventManager.GameStates.LoadingLevel)
        {

            if (Level.Instance == null)
            {
                try
                {
                    StartCoroutine(LoadAsynchronously(DataManager.GameData.Level_id));
                }
                catch(Exception e)
                {
                    Debug.Log(e);

                    int rand_level = UnityEngine.Random.Range(1, DataManager.GameData.Total_Level_Count);
                    StartCoroutine(LoadAsynchronously(rand_level));
                }

            }

            EventManager.OnLevelStart?.Invoke();
        }

        if(gameStates == EventManager.GameStates.Restart)
        {
            SceneManager.UnloadSceneAsync(Level.Instance.scene).completed += (op) =>
            {
                SceneManager.LoadSceneAsync("Level " + DataManager.PlayerData.Level, LoadSceneMode.Additive).completed += (op2) =>
                {

                    EventManager.OnGameStart?.Invoke();
                };

            };
        }

          

        

        if(gameStates == EventManager.GameStates.NextLevel)
        {
            DataManager.GameData.Level_id++;

            if (DataManager.PlayerData.Level < DataManager.GameData.Total_Level_Count)
            {
                DataManager.PlayerData.Level++;
            }
            else
            {
                int level_count = UnityEngine.Random.Range(1, DataManager.GameData.Total_Level_Count);
                DataManager.PlayerData.Level = level_count;
            }

            SceneManager.UnloadSceneAsync(Level.Instance.scene).completed += (op) =>
            {
                SceneManager.LoadSceneAsync("Level " + DataManager.PlayerData.Level, LoadSceneMode.Additive).completed += (op2) =>
                {

                    EventManager.OnGameStart?.Invoke();
                };

            };
        }
       
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Level " + sceneIndex, LoadSceneMode.Additive);



        while (!operation.isDone)
        {

            yield return null;
        }

        EventManager.OnLevelStart?.Invoke();
    }



}
