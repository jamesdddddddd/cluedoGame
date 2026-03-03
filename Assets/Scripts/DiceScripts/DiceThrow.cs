using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiceThrow : MonoBehaviour
{
    public Dice diceToThrow;
    public int amountOfDice = 2;
    public float throwForce = 5f;
    public float rollForce = 10f;

    private List<GameObject> _spawnedObjects = new List<GameObject>();
    private bool logged = false;

    private void Update()
    {
        // do func if press space
        if (Input.GetKeyDown(KeyCode.Space) && !logged) RollDice();
        else if (Input.GetKeyDown(KeyCode.Space) && logged)
            print("You have already rolled the dice.");
    }

    // async waits defoar spawning a functin
    private async void RollDice()
    {
        if(diceToThrow == null) return;
        logged = true;

        foreach (var die in _spawnedObjects)
        {
            Destroy(die);

        }

        for (int i = 0; i < amountOfDice; i++)
        {
            Dice dice = Instantiate(diceToThrow, transform.position, transform.rotation);
            _spawnedObjects.Add(dice.gameObject);
            dice.RollDice(throwForce, rollForce, i);
            await Task.Yield();
        }
    }
}
