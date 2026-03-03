using UnityEngine;

public class camToggle : MonoBehaviour
{
    public Camera boardCam;
    public Camera cam2;

    public void Start()
    {   boardCam = GameObject.Find("Dice cam").GetComponent<Camera > ();
        cam2 = GameObject.Find("player cam").GetComponent<Camera>();
        boardCam.gameObject.SetActive(true) ;
        cam2.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            boardCam.gameObject.SetActive(!boardCam.gameObject.activeSelf);
            cam2.gameObject.SetActive(!cam2.gameObject.activeSelf);
        }

    }
}
