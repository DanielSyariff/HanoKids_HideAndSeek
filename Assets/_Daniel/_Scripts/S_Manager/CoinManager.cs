using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }
    public CurrencyData coinValue;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddCoins(int amount)
    {
        coinValue.coinAmount += amount;
        Debug.Log("Coins Added. New Coin Value: " + coinValue.coinAmount);
    }
    public string GetCoin()
    {
        return coinValue.coinAmount.ToString();
    }
}
