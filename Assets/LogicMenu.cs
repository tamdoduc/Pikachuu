using System;
using System.Collections;
using System.Collections.Generic;
using DevDuck;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Color = System.Drawing.Color;

public class LogicMenu : MonoBehaviour
{ 
    public Button playButton;
    public Button quitButton;

    [Header("Top UI")] public Button shopButton;
    public Button SoundButton,musicButton;
    public Button inAppButton, coinsButton;
    public GameObject shopPanel, tabInAppPanel, tabCoinPanel;
    public Button closeShopButton;
    
    public TextMeshProUGUI coinText,shuffleText,hintText;
    private void Awake()
    {
        playButton.onClick.AddListener(OnClickEasyButton);
        quitButton.onClick.AddListener(OnClickQuitButton);
        shopButton.onClick.AddListener(OnClickShopButton);
        inAppButton.onClick.AddListener(OnClickInAppButton);
        coinsButton.onClick.AddListener(OnClickCoinsButton);
        closeShopButton.onClick.AddListener(OnClickCloseShopButton);
    }

    private void Start()
    {
        GetData();
    }

    public void GetData()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsManager.isGetDefaultItems) == 0)
        {
            PlayerPrefs.SetInt(PlayerPrefsManager.Hint, 5);
            PlayerPrefs.SetInt(PlayerPrefsManager.Shuffle, 5);
            PlayerPrefs.SetInt(PlayerPrefsManager.isGetDefaultItems,1);
        }
       
        coinText.text = PlayerPrefs.GetInt(PlayerPrefsManager.Coin).ToString();
        hintText.text = PlayerPrefs.GetInt(PlayerPrefsManager.Hint).ToString();
        shuffleText.text = PlayerPrefs.GetInt(PlayerPrefsManager.Shuffle).ToString();
    }
    private void OnClickCloseShopButton()
    {
        shopPanel.gameObject.SetActive(false);
    }

    private void OnClickCoinsButton()
    {
        tabInAppPanel.SetActive(false);
        tabCoinPanel.SetActive(true);
        inAppButton.GetComponent<Image>().color = new Color32(183,183,183,255);
        coinsButton.GetComponent<Image>().color =new Color32(255,255,255,255);
    }

    private void OnClickInAppButton()
    {
        tabInAppPanel.SetActive(true);
        tabCoinPanel.SetActive(false);
        coinsButton.GetComponent<Image>().color = new Color32(183,183,183,255);
        inAppButton.GetComponent<Image>().color =new Color32(255,255,255,255);
    }

    private void OnClickShopButton()
    {
        shopPanel.gameObject.SetActive(true);
    }

    private void OnClickQuitButton()
    {
        Application.Quit();
    }

    private void OnClickHardButton()
    {
        throw new NotImplementedException();
    }

    private void OnClickNormalButton()
    {
        throw new NotImplementedException();
    }

    private void OnClickEasyButton()
    {
        ManagerScene.ins.LoadScene("GamePlay");
    }
}
