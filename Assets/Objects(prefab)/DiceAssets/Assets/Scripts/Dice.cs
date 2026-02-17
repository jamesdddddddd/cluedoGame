using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    // its an array ?? of dice faces i belive 
    public Transform[] diceFaces;
    public Rigidbody rb;

    // It depends on the num of dices u have
    private int _diceIndex = -1;

    private bool _hasStoppedRolling;
    private bool _delayFinished;

    public static UnityAction<int, int> OndiceResult;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_delayFinished) return; 

        if (!_hasStoppedRolling && rb.angularVelocity.sqrMagnitude == 0f)
        {
            _delayFinished = true;
            GetNumberOnTopFace();
        }
    }

    // Makes a manu for displaing the top face
    [ContextMenu(itemName:"Get top face")]

    // getting the topface value
    private int GetNumberOnTopFace()
    {
        if (diceFaces == null) return -1;

        var topFace = 5; // side 6
        var lastYPostion = diceFaces[5].position.y; 

        // determens the new top face
        for (int i = 0; i < diceFaces.Length; i++)
        {
            if (diceFaces[i].position.y > lastYPostion)
            {
                lastYPostion = diceFaces[i].position.y;
                topFace = i;
            }
        }

        // displays the result in the log
        Debug.Log($"Dice result {topFace + 1}");

        OndiceResult?.Invoke(_diceIndex, topFace + 1);
        return topFace + 1;
    }

    public void RollDice(float throwForce, float rollforce, int i)
    {
        _diceIndex = i;
        // 
        var randomVariance = UnityEngine.Random.Range(-1f, 1f);
        // Impulse gives a little bit of a puch before the engen takes over
        rb.AddForce(transform.forward * (throwForce + randomVariance), ForceMode.Impulse);
        
        var randX = UnityEngine.Random.Range(0f, 1f);
        var randY = UnityEngine.Random.Range(0f, 1f);
        var randZ = UnityEngine.Random.Range(0f, 1f);

        rb.AddTorque(new Vector3(randX, randY, randZ) * (rollforce + randomVariance), ForceMode.Impulse);
        DelayResult();
    }

    private async Task DelayResult()
    {
        await Task.Delay(1000);
        _delayFinished = true;
    }
}
