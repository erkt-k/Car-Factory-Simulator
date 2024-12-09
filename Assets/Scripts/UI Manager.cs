using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI coinText;
    [SerializeField] TMPro.TextMeshProUGUI diamondsText;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private MoneyManager moneyManager;
    
    private int repairmentLevelNumber = 1;
    [SerializeField] TMPro.TextMeshProUGUI repairmentLevelText;
    List<GameObject> repairmentCoinButtons;
    List<GameObject> repairmentDiamondButtons;

    private int washingLevelNumber = 1;
    [SerializeField] TMPro.TextMeshProUGUI washingLevelText;
    List<GameObject> washingCoinButtons;
    List<GameObject> washingDiamondButtons;

    [SerializeField] private GameObject sellingWindow;
    [SerializeField] private GameObject sellingButton;

    [SerializeField] private GameObject auctionButton1;
    [SerializeField] private GameObject auctionButton2;
    [SerializeField] private GameObject auctionButton3;
    private void Start()
    {
        coinText.text = FormatMoney(moneyManager.CoinAmount);
        diamondsText.text = FormatMoney(moneyManager.DiamondAmount);
        repairmentLevelText.text = "Level 1";
        washingLevelText.text = "Level 1";
        repairmentCoinButtons = new List<GameObject>();
        repairmentDiamondButtons = new List<GameObject>();
        washingCoinButtons = new List<GameObject>();
        washingDiamondButtons = new List<GameObject>();


        GameObject[] tempButtons = GameObject.FindGameObjectsWithTag("RepairmentCoinButton");
        foreach (GameObject repairmentButton in tempButtons)
        {
            repairmentCoinButtons.Add(repairmentButton);
            TMPro.TextMeshProUGUI textComponent = repairmentButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = FormatMoney(moneyManager.RepairmentCoinCost);
            }
        }

        tempButtons = GameObject.FindGameObjectsWithTag("RepairmentDiamondButton");
        foreach (GameObject repairmentButton in tempButtons)
        {
            repairmentDiamondButtons.Add(repairmentButton);
            TMPro.TextMeshProUGUI textComponent = repairmentButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = FormatMoney(moneyManager.RepairmentDiamondCost);
            }
        }

        tempButtons = GameObject.FindGameObjectsWithTag("WashingCoinButton");
        foreach (GameObject washingButton in tempButtons)
        {
            washingCoinButtons.Add(washingButton);
            TMPro.TextMeshProUGUI textComponent = washingButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = FormatMoney(moneyManager.WashingCoinUpgradeCost);
            }
        }

        tempButtons = GameObject.FindGameObjectsWithTag("WashingDiamondButton");
        foreach (GameObject washingButton in tempButtons)
        {
            washingDiamondButtons.Add(washingButton);
            TMPro.TextMeshProUGUI textComponent = washingButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = FormatMoney(moneyManager.WashingDiamondUpgradeCost);
            }
        }

        TMPro.TextMeshProUGUI auctionText1 = auctionButton1.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (auctionText1 != null)
        {
            auctionText1.text = FormatMoney((int)(moneyManager.Gainings * 0.8));
        }

        TMPro.TextMeshProUGUI auctionText2 = auctionButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (auctionText2 != null)
        {
            auctionText2.text = FormatMoney(moneyManager.Gainings);
        }

        TMPro.TextMeshProUGUI auctionText3 = auctionButton3.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (auctionText3 != null)
        {
            auctionText3.text = FormatMoney((int) (moneyManager.Gainings * 0.6));
        }

        sellingWindow.SetActive(false);
    }

    public void UpdateMoney(double amount)
    {
        coinText.text = FormatMoney(moneyManager.CoinAmount);
    }

    public void UpdateDiamonds(double amount) 
    {
        diamondsText.text = moneyManager.DiamondAmount.ToString();
    }

    private static string FormatMoney(double num)
    {
        if (num >= 1e12)
        {
            return (num / 1e12).ToString("0.#") + " T"; //Trillion
        } else if (num >= 1e12)
        {
            return (num / 1e9).ToString("0.#") + " B"; //Billion
        } else if (num >= 1e6)
        {
            return (num / 1e6).ToString("0.#") + " M"; //Million
        } else if (num >= 1e3)
        {
            return (num / 1e3).ToString("0.#") + " K"; //Thousand
        } else
        {
            return num.ToString("0.#"); //Less Than a Thousand
        }
    }

    public void IncreaseRepairmentLevel() {
        repairmentLevelNumber++;
        repairmentLevelText.text = "Level " + repairmentLevelNumber.ToString();
        foreach (GameObject repairmentButton in repairmentCoinButtons) {
            TMPro.TextMeshProUGUI textComponent = repairmentButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = FormatMoney(moneyManager.RepairmentCoinCost);
            }
        }

        foreach (GameObject repairmentButton in repairmentDiamondButtons) {
            TMPro.TextMeshProUGUI textComponent = repairmentButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = FormatMoney(moneyManager.RepairmentDiamondCost);
            }
        }
    }

    public void IncreaseWashingLevel() {
        washingLevelNumber++;
        washingLevelText.text = "Level " + washingLevelNumber.ToString();
        foreach (GameObject washingButton in washingCoinButtons) {
            TMPro.TextMeshProUGUI textComponent = washingButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = FormatMoney(moneyManager.WashingCoinUpgradeCost);
            }
        }

        foreach (GameObject washingButton in washingDiamondButtons){
            TMPro.TextMeshProUGUI textComponent = washingButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = FormatMoney(moneyManager.WashingDiamondUpgradeCost);
            }
        }
    }

    public void OpenSellingWindow() {
        sellingWindow.SetActive(true);
        TMPro.TextMeshProUGUI auctionText1 = auctionButton1.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (auctionText1 != null)
        {
            auctionText1.text = FormatMoney((int)(moneyManager.Gainings * 0.8));
        }

        TMPro.TextMeshProUGUI auctionText2 = auctionButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (auctionText2 != null)
        {
            auctionText2.text = FormatMoney(moneyManager.Gainings);
        }

        TMPro.TextMeshProUGUI auctionText3 = auctionButton3.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (auctionText3 != null)
        {
            auctionText3.text = FormatMoney((int)(moneyManager.Gainings * 0.6));
        }

        sellingButton.SetActive(false);
    }

    public void CloseSellingWindow() {
        sellingWindow.SetActive(false);
        sellingButton.SetActive(true);
    }  

    public void UpfateAuctionButtons() {
        TMPro.TextMeshProUGUI auctionText1 = auctionButton1.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (auctionText1 != null)
        {
            auctionText1.text = FormatMoney((int) (moneyManager.Gainings * 0.8));
        }

        TMPro.TextMeshProUGUI auctionText2 = auctionButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (auctionText2 != null)
        {
            auctionText2.text = FormatMoney((int) (moneyManager.Gainings));
        }

        TMPro.TextMeshProUGUI auctionText3 = auctionButton3.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (auctionText3 != null)
        {
            auctionText3.text = FormatMoney((int) (moneyManager.Gainings * 0.6));
        }
    }   
}
