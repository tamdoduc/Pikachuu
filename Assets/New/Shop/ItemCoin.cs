using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemCoin : MonoBehaviour
{
    [SerializeField] private string key;
    [SerializeField] private int coinReceive;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private TextMeshProUGUI txtCoin;
    UnityAction<string> actionOnClick;

    public string Key => key;
    public int CoinReceive => coinReceive;

    public void Init(string txtPrice, UnityAction<string> actionOnClick)
    {
        this.txtPrice.text = txtPrice;
        txtCoin.text = coinReceive.ToString();
        this.actionOnClick = actionOnClick;
    }
    public void OnClickItem()
    {
        Debug.Log($"OnClickItem: name:{gameObject.name} key:{key}");
        actionOnClick?.Invoke(key);
    }
    public void OnSuccess()
    {
        Debug.Log($"OnSuccess: name:{gameObject.name} key:{key}");
        var coin =  PlayerPrefs.GetInt(PlayerPrefsManager.Coin, 0);
        coin += coinReceive;
        PlayerPrefs.SetInt(PlayerPrefsManager.Coin, coin);
      //  GameplayUI.Instance.UpdateCoin();
    }
}
