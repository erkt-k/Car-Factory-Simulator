using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    /*
    Keep buttons inside,
    Handle every onClick action
    Buttons:
        Repairment {
            Upgrade with Coins
                -> Increase Level of Repairment Unit
                -> Increase the cost of ugrade in coin and diamonds
                -> 
            Upgrade with Diamonds
        }

        Washing {
            Upgrade with Coins
            Upgrade with Diamonds
        }

        Unlock new Repairment Unit {
            Close Unlock Buttons
        }

        Sell Window {
            Open Sell Car Window
        }

        Sell Car Button {
            Sell car to earn coins
        }

        Watch Ads Button {
            Watch Ad for Coin
            Watch Ads for Diamond
        }
    */

    [SerializeField] private GameManager gameManager;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private UIManager uiManager;  

    [SerializeField] private GameObject repairmentCoinUpgrade;
    [SerializeField] private GameObject repairmentDiamondUpgrade;

    [SerializeField] private GameObject washingCoinUpgrade;
    [SerializeField] private GameObject washingDiamondUpgrade;
    [SerializeField] private GameObject exclamationMark;


    public void RepairmentCoinUpgrade() {
        if (moneyManager.CheckEnoughCoin(moneyManager.RepairmentCoinCost)) {
            moneyManager.ChangeCoin(-1 * (moneyManager.RepairmentCoinCost));
            uiManager.UpdateMoney(moneyManager.CoinAmount);
            gameManager.DecreaseRepairmentTimer();
            moneyManager.IncreaseRepCoinUpgradeCost();
            moneyManager.IncreaseRepDiamondUpgradeCost();
            Debug.Log(moneyManager.CoinAmount.ToString() + " Coin");
            uiManager.IncreaseRepairmentLevel();
        }
    }

    public void RepairmentDiamondUpgrade() {
        if (moneyManager.CheckEnoughDiamond(moneyManager.RepairmentDiamondCost)) {
            moneyManager.ChangeDiamond(-1 * (moneyManager.RepairmentDiamondCost));
            uiManager.UpdateDiamonds(moneyManager.DiamondAmount);
            gameManager.DecreaseRepairmentTimer();
            moneyManager.IncreaseRepCoinUpgradeCost();
            moneyManager.IncreaseRepDiamondUpgradeCost();
            Debug.Log(moneyManager.DiamondAmount.ToString() + " Diamond"); 
            uiManager.IncreaseRepairmentLevel();
        }
    }

    public void UnlockNewRepairmentUnit() {
    }

    public void WashingCoinUpgrade() {
        if (moneyManager.CheckEnoughCoin(moneyManager.WashingCoinUpgradeCost)) {
            moneyManager.ChangeCoin(-1 * (moneyManager.WashingCoinUpgradeCost));
            uiManager.UpdateMoney(moneyManager.CoinAmount);
            gameManager.DecreaseWashingTimer();
            moneyManager.IncreaseWashingCoinUpgradeCost();
            moneyManager.IncreaseWashingDiamondUpgradeCost();
            Debug.Log(moneyManager.CoinAmount.ToString() + " Coin");
            uiManager.IncreaseWashingLevel();
        }
    }

    public void WashingDiamondUpgrade() {
        if (moneyManager.CheckEnoughDiamond(moneyManager.WashingDiamondUpgradeCost)) {
            moneyManager.ChangeDiamond(-1 * (moneyManager.WashingDiamondUpgradeCost));
            uiManager.UpdateDiamonds(moneyManager.DiamondAmount);
            gameManager.DecreaseWashingTimer();
            moneyManager.IncreaseWashingCoinUpgradeCost();
            moneyManager.IncreaseWashingDiamondUpgradeCost();
            Debug.Log(moneyManager.DiamondAmount.ToString() + " Diamond");
            uiManager.IncreaseWashingLevel();
        }
    }

    public void UnlockNewWashingUnit() {

    }

    public void OpenSellingWindow() {
        uiManager.OpenSellingWindow();
    }

    public void CloseSellingWindow() {
        uiManager.CloseSellingWindow();
    }
    
    public void SellCar() {
        if (gameManager.SellCar()) {
            moneyManager.ChangeCoin(moneyManager.Gainings);
            moneyManager.IncreaseGainings();
            uiManager.UpdateMoney(moneyManager.CoinAmount);
            uiManager.UpfateAuctionButtons();
            uiManager.CloseSellingWindow();
        }
    }

}
