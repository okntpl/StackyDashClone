using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager :MonoBehaviour
{
    [Header("GAME UI")]
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Image loadingBar;
    [SerializeField] GameObject IndexLevel;
    [SerializeField] GameObject gameViewPanel;
    [SerializeField] TextMeshProUGUI stageClearText;
    [SerializeField] GameObject stageFinishedPanel;
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI levelCompletedText;


    private void OnEnable()
    {
        EventManager.OnGameClear += StageClear;
        EventManager.OnGameFinish += StageFinished;
        EventManager.OnGameStart += GameStart;
        EventManager.OnLevelStart += LevelStart;
        EventManager.OnCollectStack += UpdateScore;
       

        InputManager.Swipe += SetTutorial;
    }

    private void OnDisable()
    {
        EventManager.OnGameClear -= StageClear;
        EventManager.OnGameFinish -= StageFinished;
        EventManager.OnGameStart -= GameStart;
        EventManager.OnLevelStart -= LevelStart;
        EventManager.OnCollectStack -= UpdateScore;
    }

    private void GameStart()
    {
        stageFinishedPanel.SetActive(false);
        loadingPanel.SetActive(true);
        loadingBar.DOFillAmount(1f, 1.5f).OnComplete(() =>
        EventManager.Instance.CurrentState = EventManager.GameStates.LoadingLevel);
    }

    private void LevelStart()
    {

        loadingPanel.SetActive(false);
        gameViewPanel.SetActive(true);


        int sceneIndex = DataManager.GameData.Level_id;
        

        Debug.Log(sceneIndex);
        for (int i = 0;i<5;i++)
        {
            if (sceneIndex - 5 < 0)
            {
                IndexLevel.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"" + (i + 1) % 11;
                if (sceneIndex > i)
                {
                    IndexLevel.transform.GetChild(i).GetComponent<Image>().DOFade(255f, -1);
                }
            }
            else
            {
                IndexLevel.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"" + (i + 6) % 11;
                if (sceneIndex > i + 5)
                {
                    IndexLevel.transform.GetChild(i).GetComponent<Image>().DOFade(255f, -1);
                }
            }
        }

    }

   

    private void StageClear()
    {
        stageClearText.transform.DOScale(Vector3.one * 1.5f, 0.5f).SetEase(Ease.InOutBounce).OnComplete(() =>
           stageClearText.transform.DOScale(Vector3.zero, 0.3f)
        );
    }

    private void StageFinished()
    {
        gameViewPanel.SetActive(false);
        stageFinishedPanel.SetActive(true);
        levelCompletedText.text = Level.Instance.scene.name + $" COMPLETED";
    }

    private void UpdateScore()
    {
        scoreText.text = Level.Instance.stackCount.ToString();
    }

    private void SetTutorial(Vector3 direction)
    {
        if(direction != Vector3.zero)
        {
            infoText.gameObject.SetActive(false);
        }
    }

    public void ResartGame()
    {

        loadingPanel.SetActive(true);
        loadingBar.DOFillAmount(1f, 1.5f).OnComplete(() =>
        EventManager.Instance.CurrentState = EventManager.GameStates.LoadingLevel);
    }

    public void NextLevel()
    {
        Debug.Log("nextlevel");
        EventManager.Instance.CurrentState = EventManager.GameStates.NextLevel;
    }

   


}

