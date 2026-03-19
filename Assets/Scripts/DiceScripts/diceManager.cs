using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class diceManager : MonoBehaviour
{
    public GameObject turn;

    public int diceSum = 0;
    public int value1 = 0;
    public int value2 = 0;

    private void OnEnable()
    {
        Dice.OndiceResult += result;
    }

    private void OnDisable()
    {
        Dice.OndiceResult -= result;
    }

    public void result(int diceIndex, int diceResult)
    {
        if (diceIndex == 0)
        {
            value1 = diceResult;
        }
        else  
            value2 = diceResult;

        // Check if both dice have values
        if (value1 > 0 && value2 > 0)
            {
                Calc();
                // Push the game to the MOVING phase
                TurnManager tm = turn.GetComponent<TurnManager>();
                if (tm.phase == turnyWurny.TurnStage.ROLLING)
                {
                    tm.switchPhase();
                }
        
            // Reset values for the next turn's roll
            value1 = 0;
            value2 = 0;
            }
    }

    public void Calc()
    {
            diceSum = value1 + value2;
    }

}
