using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private double coin = 100000;   
    private double diamonds = 15000; 
    private double repairmentCoinUpgradeCost = 5000;
    private double repairmentDiamondUpgradeCost = 50;
    private double repairmentUnlockCost = 10000;

    private double washingCoinUpgradeCost = 7000;
    private double washingDiamondUpgradeCost = 70;
    private double washingUnlockCost = 15000;

    private double gainings = 1000;


    public bool CheckEnoughCoin(double amountToCheck) {
        return amountToCheck <=  coin;
    } 

    public bool CheckEnoughDiamond(double amountToCheck) {
        return amountToCheck <= diamonds;
    }

    public void ChangeCoin(double amountToChange) {
        coin += amountToChange;
    }

    public void ChangeDiamond(double amountToChange) {
        diamonds += amountToChange;
    }
    
    public double CoinAmount {
        get { return coin; }
    }

    public double DiamondAmount {
        get { return diamonds;}
    }

    public double RepairmentCoinCost {
        get { return repairmentCoinUpgradeCost;}
    }

    public double RepairmentDiamondCost {
        get { return repairmentDiamondUpgradeCost;}
    }

    public double WashingCoinUpgradeCost {
        get { return washingCoinUpgradeCost;}
    }

    public double WashingDiamondUpgradeCost {
        get { return washingDiamondUpgradeCost;}
    }

    public double Gainings {
        get { return gainings;}
    }

    public void IncreaseGainings() {
        gainings *= 1.48;
        gainings = (int)gainings;
    }

    public void IncreaseRepCoinUpgradeCost() {
        repairmentCoinUpgradeCost *= 1.7;
        repairmentCoinUpgradeCost = (int)repairmentCoinUpgradeCost;
    }

    public void IncreaseRepDiamondUpgradeCost() {
        repairmentDiamondUpgradeCost *= 1.7;
        repairmentDiamondUpgradeCost = (int)repairmentDiamondUpgradeCost;
    }

    public void IncreaseWashingCoinUpgradeCost() {
        washingCoinUpgradeCost *= 1.7;
        washingCoinUpgradeCost = (int)washingCoinUpgradeCost;
    }
    public void IncreaseWashingDiamondUpgradeCost() {
        washingDiamondUpgradeCost *= 1.7;
        washingDiamondUpgradeCost = (int)washingDiamondUpgradeCost;
    }
}
