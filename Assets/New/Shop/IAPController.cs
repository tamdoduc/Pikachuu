using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Events;

[System.Serializable]
public class ItemIap
{
    public string key;
    public ProductType productType;
}

public class IAPController : Singleton<IAPController>, IStoreListener
{
    private static IAPController instance;
    private IStoreController storeController;
    private IExtensionProvider storeExtensionProvider;
    [SerializeField] private List<ItemIap> lstKeyCode;

    public static IAPController Instance => instance;

    public UnityAction<string> OnPurchaseSuccess;
    public UnityAction<string, PurchaseFailureReason> OnPurchaseFailedAction;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializePurchasing();
    }

    public void InitializePurchasing()
    {
        if (IsInitialized()) return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        for (int i = 0; i < lstKeyCode.Count; i++)
        {
            builder.AddProduct(lstKeyCode[i].key, lstKeyCode[i].productType);
        }

        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return storeController != null && storeExtensionProvider != null;
    }

    public bool BuyProduct(string productId, out string resultMessage)
    {
        resultMessage = "";
        if (!IsInitialized())
        {
            resultMessage = "IAP not initialized.";
            return false;
        }

        Product product = storeController.products.WithID(productId);
        if (product != null && product.availableToPurchase)
        {
            storeController.InitiatePurchase(product);
            return true;
        }
        else
        {
            resultMessage = "Product not found or not available for purchase.";
            Debug.LogError("BuyProduct FAIL: " + resultMessage);
            return false;
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized()) return;

        var apple = storeExtensionProvider.GetExtension<IAppleExtensions>();
        apple.RestoreTransactions((success) =>
        {
            Debug.Log("RestorePurchases: " + success);
        });
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("Unity IAP Initialization Failed: " + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log("Purchase successful: " + args.purchasedProduct.definition.id);
        OnPurchaseSuccess?.Invoke(args.purchasedProduct.definition.id);
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError("Purchase failed: " + product.definition.id + " Reason: " + failureReason);
        OnPurchaseFailedAction?.Invoke(product.definition.id, failureReason);
    }

    public Product GetProductByKey(string key)
    {
        if (!IsInitialized()) return null;

        return storeController.products.WithID(key);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        IsInitialized();
        throw new NotImplementedException();
    }
}
