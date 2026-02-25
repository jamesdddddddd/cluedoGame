using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkManagerUI : MonoBehaviour

{   //not using public because the man said its good to use this one
    [SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;



    private void Awake()
    {   // => is lammbda operator, it creates a small nameless method
        //this method saves specifying another method, when we can just define what it does here


        //if server button clicked, start server
        serverButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
            Debug.Log("server pressed");
        });
        //if host button clicked, start host
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        //if client button clicked, start client
        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }

}
