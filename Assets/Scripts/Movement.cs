using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start() {
        transform.position = new Vector3(4.5f, 0.5f, 0.5f);
    }

void Update() {
    // Check if the left mouse button was pressed this frame
    if (Mouse.current.leftButton.wasPressedThisFrame) {
        SetTargetPosition();
    }

    if (isMoving) {
        MovePlayer();
    }
}

void SetTargetPosition() {
    // Get mouse position from the new systemA
    Vector2 mousePos = Mouse.current.position.ReadValue();
    Ray ray = Camera.main.ScreenPointToRay(mousePos);
    
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit)) {
        Tile tile = hit.collider.GetComponent<Tile>();
        if (tile != null) {
            targetPosition = tile.GetTopPosition();
            isMoving = true;
        }
    }
}

    void MovePlayer() {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.001f) {
            isMoving = false;
        }
    }




    
}