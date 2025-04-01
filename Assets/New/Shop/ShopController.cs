using System.Collections.Generic;
using UnityEngine;

public class ShopController : Singleton<ShopController>
{
    [SerializeField] private GameObject shopUI;
    [SerializeField] private List<ItemCoin> lstItemCoin;

    private void Start()
    {
        InitializeIAP();
        InitializeItemCoins();
    }
    public void Show()
    {
        shopUI.gameObject.SetActive(true);
    }

    public void Hide()
    {
        shopUI.gameObject.SetActive(false);

    }

    public void InitializeIAP()
    {
        IAPController.Instance.OnPurchaseSuccess += (key) =>
        {
            Debug.Log($"Purchased item with key: {key}");
            for (int i = 0; i < lstItemCoin.Count; i++)
            {
                if (lstItemCoin[i].Key == key)
                    lstItemCoin[i].OnSuccess();
            }
        };
        Debug.Log("IAP initialized");

    }

    public void InitializeItemCoins()
    {
        for (int i = 0; i < lstItemCoin.Count; i++)
        {
            var product = IAPController.Instance.GetProductByKey(lstItemCoin[i].Key);
            var price = product != null ? product.metadata.localizedPriceString : "N/A";
            lstItemCoin[i].Init(price, OnClickItem);
        }

        Debug.Log("Item coins initialized");
    }
    public void OnClickItem(string key)
    {
        var product = IAPController.Instance.GetProductByKey(key);
        if (product != null)
        {
            // Code to handle the purchase using the product information
            Debug.Log($"Purchased item with key: {key}, price: {product.metadata.localizedPriceString}");
        }
        else
        {
            Debug.LogWarning($"Product with key: {key} not found");
        }
    }
}
