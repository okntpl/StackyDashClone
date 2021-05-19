using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
public class Level : MonoBehaviour
{
    public static Level Instance;
    #region Public
    public int stackCount;
    public LevelMode levelMode;

    public enum LevelMode
    {
        STARTED,
        CLEAR,
        FINISHED,
    }

    #endregion

    #region Private
    [SerializeField] CinemachineVirtualCamera cm1, cm2,cm3;
    [SerializeField] GameObject confecttiEffect;
    [SerializeField] GameObject player;
    #endregion

    private void OnEnable()
    {
            EventManager.OnLevelStart += SetActiveScene;
            EventManager.OnCollectStack += SetScore;
            EventManager.OnGameFinish += SetFinish;
            EventManager.OnGameClear += SetClear;
           
    }


    private void Awake()
    {
        
          Instance = this;
        
     
        if (EventManager.Instance == null)
        {
            Debug.Log("ManagerScene isnt loaded. Trying to load");
            SceneManager.LoadSceneAsync("ManagerScene", LoadSceneMode.Additive).completed += (op) =>
            {
                EventManager.OnGameStart?.Invoke();
            };
        }

    }
    private void OnDisable()
    {
        EventManager.OnLevelStart -= SetActiveScene;
        EventManager.OnCollectStack -= SetScore;
        EventManager.OnGameFinish -= SetFinish;
        EventManager.OnGameClear -= SetClear;
    }

    public Scene scene
    {
        get
        {
            if (this == null || gameObject == null)
            {
                return SceneManager.GetActiveScene();
            }

            return gameObject.scene;
        }
    }



    void SetActiveScene()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene.name));
    }

    void SetScore()
    {
       
        stackCount++;
        if(stackCount>DataManager.PlayerData.Coin)
        {
            DataManager.PlayerData.Coin = stackCount;
        }
    }

    void SetFinish()
    {


        cm3.enabled = false;
        cm2.enabled = true;

        Instantiate(confecttiEffect, player.transform.position, Quaternion.identity);


    }

    void SetClear()
    {
        cm1.enabled = false;
        cm3.enabled = true;
        Instantiate(confecttiEffect, player.transform.position, Quaternion.identity);
    }


    public void SetLevelMode(LevelMode mode)
    {
        levelMode = mode;
    }

}
