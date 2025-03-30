using System;
using System.Collections;
using System.Collections.Generic;
using DevDuck;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameUiManager : MonoBehaviour
{

    public float currentTime = 0;
    public float maxTime = 180;
    
    
    public Image timerBar,coinIcon;
    public TextMeshProUGUI shuffleAmountText, hintAmountText, coinAmountText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI startText;

    [Header( "Buttons")]
    public Button pauseButton;
    public Button shuffleButton;
    public Button hintButton;
    public Button closePausePanelButton;
    [Header("Panels")]
    public GameObject pausePanel;

    public Button resumeButton,rePlayButton,ExitButton;
    public List<GameObject> elementsInPausePanel = new List<GameObject>();
    

    private void Awake()
    {
        pauseButton.onClick.AddListener(OnClickPauseButton);
        shuffleButton.onClick.AddListener(OnClickShuffleButton);
        hintButton.onClick.AddListener(OnClickHintButton);
        closePausePanelButton.onClick.AddListener(OnClickClosePausePanelButton);

        resumeButton.onClick.AddListener(OnClickCResumeButton);
        rePlayButton.onClick.AddListener(OnClickRePlayButton);
        ExitButton.onClick.AddListener(OnClickExitButton);
    }

    private void OnClickExitButton()
    {
        ManagerScene.ins.LoadScene("Menu");
    }

    private void OnClickRePlayButton()
    {
        Scene scene =  SceneManager.GetActiveScene();
        Debug.Log("Replay ??");
        ManagerScene.ins.LoadScene(scene.name);
    }

    private void OnClickCResumeButton()
    {
        ManagerGame.TIME_SCALE = 1;
        pausePanel.gameObject.SetActive(false);
        GameManager.instance.isCanSelect = false;
    }

    private void OnClickClosePausePanelButton()
    {
        ManagerGame.TIME_SCALE = 1;
        pausePanel.gameObject.SetActive(false);
        GameManager.instance.isCanSelect = false;
    }
    private void OnClickHintButton()
    {
        GameManager.instance.Hint();
    }
    private void OnClickShuffleButton()
    {
        GameManager.instance.ShuffleBoard();
    }
    private void OnClickPauseButton()
    {
        ManagerGame.TIME_SCALE = 0;
        pausePanel.gameObject.SetActive(true);
        GameManager.instance.isCanSelect = true;

        for (int i = 0; i < elementsInPausePanel.Count; i++)
        {
            var a = i;
            elementsInPausePanel[a].transform.DOScale(1,0.3f).SetEase(Ease.OutBack).SetDelay(a*0.15f).From(0);
        }
    }

    public void ReduceTimer(float value)
    {
        timerBar.fillAmount =  value;
    }

    public void UpdaeCoinText(int coin)
    {
        coinIcon.transform.DOKill();
        coinIcon.transform.DOScale(1.1f, 0.1f).OnComplete((() =>
        {
            coinIcon.transform.DOScale(1, 0.1f);
        }));
        coinAmountText.text = coin.ToString();
    }
    
    public void UpdaeScoreText(int score)
    {
        scoreText.transform.DOKill();
        scoreText.transform.DOScale(1.1f, 0.1f).OnComplete((() =>
        {
            scoreText.transform.DOScale(1, 0.1f);
        }));
        scoreText.text = score.ToString();
    }
    

    public void AnimStartGame()
    {
        Sequence mySequence = DOTween.Sequence();
        startText.gameObject.SetActive(true);
        startText.text = "3";
        mySequence.Append(startText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).From(0));
        mySequence.Append(startText.transform.DOScale(0f, 0.2f).SetDelay(0.5f).SetEase(Ease.InBack).OnComplete(()=>startText.text = "2"));
        mySequence.Append(startText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack));
        mySequence.Append(startText.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack).SetDelay(0.5f).OnComplete(()=>
        {
            startText.text = "1";
        }));
        mySequence.Append(startText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack));
        mySequence.Append(startText.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack).SetDelay(0.5f).OnComplete(()=>startText.text = "Start"));
        mySequence.Append(startText.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack).SetDelay(0.5f).OnComplete(()=>
        {
            startText.DOFade(0, 0.2f).SetDelay(0.5f).OnComplete(() => GameManager.instance.isCanPlay = true);
        }));

    }

    public void SetHintText(int hintAmount)
    {
        hintAmountText.text = hintAmount.ToString();
    }
    public void SetShuffleText(int shuffleAmount)
    {
        shuffleAmountText.text = shuffleAmount.ToString();
    }
}
