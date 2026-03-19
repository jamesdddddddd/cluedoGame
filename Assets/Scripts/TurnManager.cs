using UnityEngine;
using TMPro;
using turnyWurny;

public class TurnManager : MonoBehaviour
{

    public TextMeshProUGUI status;
    public TextMeshProUGUI activePlayer;


    public TurnStage phase = TurnStage.ROLLING;
    public PlayerTurn who = PlayerTurn.Player1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        status.text = "Rolling!";
        activePlayer.text = "Player1";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PlayerTurn whosUp()
    {
        return who;

    }

    public TurnStage whatStage()
    {
        return phase;
    }


    public void switchPhase()
    {
        if (phase == TurnStage.ROLLING)
        {
            phase = TurnStage.MOVING;
            status.text = "Moving!";
        }
    

        else if (phase == TurnStage.MOVING)
        {
            phase = TurnStage.SUGGESTING;
            status.text = "Suggesting!";
        }
        else
        {
            phase = TurnStage.ROLLING;
            status.text = "Next Player Rolling!";
            nextTurn();
        }
        

    }

    public void nextTurn()
    {  
        int playerNo = System.Enum.GetValues(typeof(PlayerTurn)).Length;
        int nextPlayer = ((int)who + 1) % playerNo;
        who = (PlayerTurn)nextPlayer;
        activePlayer.text = who.ToString();
    }
}