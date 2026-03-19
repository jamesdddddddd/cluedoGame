using UnityEngine;
using turnyWurny;

public class camToggle : MonoBehaviour
{
    public Camera cam1; // Board View
    public Camera cam2; // Dice View
    public TurnManager turns;

    private TurnStage lastCheckedPhase;

    void Start()
    {
        // Ensure both objects are active, but only one camera is "looking"
        cam1.gameObject.SetActive(true);
        cam2.gameObject.SetActive(true);
        
        // Default state
        cam1.enabled = false;
        cam2.enabled = true;

        lastCheckedPhase = turns.phase;
    }

    void Update()
    {
        // Check every frame if the manager's phase is different from our local record
        if (turns.phase != lastCheckedPhase)
        {
            Debug.Log("Phase changed to: " + turns.phase); // Check your console for this!
            UpdateCameraState();
            lastCheckedPhase = turns.phase;
        }
    }

    void UpdateCameraState()
    {
        if (turns.phase == TurnStage.MOVING)
        {
        Debug.Log("Switching to Board Cam (Cam1)");
        cam1.enabled = true;
        cam2.enabled = false;
        }
        else if (turns.phase == TurnStage.ROLLING)
        {
        Debug.Log("Switching to Dice Cam (Cam2)");
        cam1.enabled = false;
        cam2.enabled = true;
        }
}
}