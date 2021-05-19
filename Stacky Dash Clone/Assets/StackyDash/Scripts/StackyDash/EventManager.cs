using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{

    public static EventManager Instance;

    public static UnityAction OnGameStart;
    public static UnityAction OnGameFinish;
    public static UnityAction OnGameClear;
    public static UnityAction OnLevelStart;
    public static UnityAction OnCollectStack;
    

    

    public static UnityAction<GameStates> OnGameStateChange;
    private GameStates currentState = GameStates.GameStart; 

    public enum GameStates
    {
        GameStart,
        LoadingLevel,
        GameWin,
        NextLevel,
    }


    public GameStates CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
            OnGameStateChange?.Invoke(value);
        }
    }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        OnGameStart?.Invoke();
    }


}
