using UnityEngine;
using TMPro;

public class DoorUI : Unity.Netcode.NetworkBehaviour
{
    public TextMeshProUGUI doorNotif;
    public GameObject enterButton;
    public GameObject exitButton;

    public override void OnNetworkSpawn()
    {
        doorNotif.gameObject.SetActive(false);
        enterButton.SetActive(false);
    }


    public void notifDoor(string popentry)
    {
        doorNotif.text = popentry;
        doorNotif.gameObject.SetActive(true);
        enterButton.SetActive(true);

    }

    public void noMoDoor()
    {
        doorNotif.gameObject.SetActive(false);
        enterButton.SetActive(false);
    }

    public void roomExit()
    {
        exitButton.SetActive(true);
    }
}
