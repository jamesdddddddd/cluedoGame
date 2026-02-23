using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text diceOneText, diceTwoText;
    public TextMeshProUGUI doorNotif;

    private void OnEnable()
    {
        Dice.OndiceResult += SetText;
    }

    private void OnDisable()
    {
        Dice.OndiceResult -= SetText;
    }

    public void SetText(int diceIndex, int diceResult)
    {
        if (diceIndex != 0)
        {
            diceTwoText.SetText($"Dice two rolled a {diceResult}");
        }
        else
        {
            diceOneText.SetText($"Dice one rolled a {diceResult}");
        }
    }

}