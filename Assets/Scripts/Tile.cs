using UnityEngine;
using UnityEngine.InputSystem;

public class Tile : MonoBehaviour
{


    void Start()
    {
    
    }


    // Returns the position the player should move to
    public Vector3 GetTopPosition()
    {
        // Adjust the '1f' if your player's feet are sinking into the tile
        return new Vector3(transform.position.x, 0.5f, transform.position.z);
    }
}