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

public class UiWinLose : MonoBehaviour
{
    [Header("Panel : ")] public GameObject winPanel, losePanel;
    [Header("Elements : ")] public Image loseTitle, winTitle, starBg;

    public TextMeshProUGUI coinText;
    public Image leftGreyStar, rightGreyStar, middleGreyStar;
    public Image leftYellowStar, rightYellowStar, middleYellowStar;
    [FormerlySerializedAs("ReplayButton")] public Button replayButton;
    public GameObject shadowPanel;
    public CanvasGroup coinBar;

    public TextMeshProUGUI nextOrRePlay;

    private void Awake()
    {
        replayButton.onClick.AddListener(OnClickReplayButton);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowWinPanel3Star();
        }
    }

    private void OnClickReplayButton()
    {
        Scene scene = SceneManager.GetActiveScene();
        ManagerScene.ins.LoadScene(scene.name);
      //  SceneManager.LoadSceneAsync(scene.name);
    }

    public void ShowWinPanel1Star()
    {
        nextOrRePlay.text = "Next";
        coinText.text = GameManager.instance.coin.ToString();
        shadowPanel.gameObject.SetActive(true);
        winTitle.gameObject.SetActive(true);
        rightYellowStar.gameObject.SetActive(false);
        middleYellowStar.gameObject.SetActive(false);
        List<GameObject> objects = new List<GameObject>();
        objects.Add(winTitle.gameObject);
        objects.Add(starBg.gameObject);
        objects.Add(leftGreyStar.gameObject);
        objects.Add(middleGreyStar.gameObject);
        objects.Add(rightGreyStar.gameObject);
        objects.Add(coinBar.gameObject);
        objects.Add(replayButton.gameObject);
        for (int i = 0; i < objects.Count; i++)
        {
            var a = i;
            objects[a].transform.DOScale(1, 0.25f).SetEase(Ease.OutBack).SetDelay(a * 0.1f).From(0);
        }
    }

    public void ShowWinPanel2Star()
    {
        nextOrRePlay.text = "Next";
        coinText.text = GameManager.instance.coin.ToString();
        shadowPanel.gameObject.SetActive(true);
        winTitle.gameObject.SetActive(true);
        // middleYellowStar.gameObject.SetActive(true);
        rightYellowStar.gameObject.SetActive(false);
        List<GameObject> objects = new List<GameObject>();
        objects.Add(winTitle.gameObject);
        objects.Add(starBg.gameObject);
        objects.Add(leftGreyStar.gameObject);
        objects.Add(middleGreyStar.gameObject);
        objects.Add(rightGreyStar.gameObject);
        objects.Add(coinBar.gameObject);
        objects.Add(replayButton.gameObject);
        for (int i = 0; i < objects.Count; i++)
        {
            var a = i;
            objects[a].transform.DOScale(1, 0.25f).SetEase(Ease.OutBack).SetDelay(a * 0.1f).From(0);
        }
    }

    public void ShowWinPanel3Star()
    {
        nextOrRePlay.text = "Next";
        coinText.text = GameManager.instance.coin.ToString();
        shadowPanel.gameObject.SetActive(true);
        winTitle.gameObject.SetActive(true);
        //  middleYellowStar.gameObject.SetActive(true);
        //  rightYellowStar.gameObject.SetActive(true);
        List<GameObject> objects = new List<GameObject>();
        objects.Add(winTitle.gameObject);
        objects.Add(starBg.gameObject);
        objects.Add(leftGreyStar.gameObject);
        objects.Add(middleGreyStar.gameObject);
        objects.Add(rightGreyStar.gameObject);
        objects.Add(coinBar.gameObject);
        objects.Add(replayButton.gameObject);
        for (int i = 0; i < objects.Count; i++)
        {
            var a = i;
            objects[a].transform.DOScale(1, 0.25f).SetEase(Ease.OutBack).SetDelay(a * 0.1f).From(0);
        }
    }

    public void ShowLosePanel()
    {
        nextOrRePlay.text = "Replay";
        coinText.text = GameManager.instance.coin.ToString();
        shadowPanel.gameObject.SetActive(true);
        loseTitle.gameObject.SetActive(true);
        leftYellowStar.gameObject.SetActive(false);
        middleYellowStar.gameObject.SetActive(false);
        rightYellowStar.gameObject.SetActive(false);
        List<GameObject> objects = new List<GameObject>();
        objects.Add(winTitle.gameObject);
        objects.Add(starBg.gameObject);
        objects.Add(leftGreyStar.gameObject);
        objects.Add(middleGreyStar.gameObject);
        objects.Add(rightGreyStar.gameObject);
        objects.Add(coinBar.gameObject);
        objects.Add(replayButton.gameObject);
        for (int i = 0; i < objects.Count; i++)
        {
            var a = i;
            objects[a].transform.DOScale(1, 0.25f).SetEase(Ease.OutBack).SetDelay(a * 0.1f).From(0);
        }
    }
}