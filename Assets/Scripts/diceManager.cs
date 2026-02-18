using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class diceManager : MonoBehaviour
{
    public int diceSum = 0;
    private int value1 = 0;
    private int value2 = 0;

    private void OnEnable()
    {
        Dice.OndiceResult += result;
    }

    private void OnDisable()
    {
        Dice.OndiceResult -= result;
    }

    private void result(int diceIndex, int diceResult)
    {
        if (diceIndex == 0)
        {
            value1 = diceResult;
        }
        else
        {
            value2 = diceResult;
        }
    }

    public void Calc()
    {
        diceSum = value1 + value2;
    }
}
