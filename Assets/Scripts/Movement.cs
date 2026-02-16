using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject stage; 
    public List<GameObject> nearby = new List<GameObject>();
    
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool onWhite = false;

    private int move_tokens = 8;

    void Start() 
    {
        transform.position = new Vector3(4.5f, 0.5f, 0.5f);
        UpdateCurrentTile();
    }

    void Update() 
    {
    
        if (Mouse.current.leftButton.wasPressedThisFrame && !isMoving) 
        {
            checkInput();
        }

        if (isMoving) {
            movePlayer();
        }
    }

    void checkInput() 
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        
        if (Physics.Raycast(ray, out RaycastHit hit)) 
        {

            Tile clickedTile = hit.collider.GetComponent<Tile>();
            if (clickedTile == null) {
                Debug.LogWarning("Not a tile");
                return;
            }

            if (stage == null) {
                Debug.LogError("Current tile not found");
                UpdateCurrentTile();
                return; 
            }

            Tile currentStand = stage.GetComponent<Tile>();
            nearby = currentStand.neighbours;

            bool isNeighbor = nearby.Contains(hit.collider.gameObject);
          
            // The logic of the board game is that you can only move vertically or horizontally, a checkerboard pattern helps a simple script facilitate this.
            bool isWhite = hit.collider.GetComponent<White>() != null;
            bool isBlack = hit.collider.GetComponent<Black>() != null;

            // Checks if the selected tile is valid: a vertical or horizontal neighbour of the current tile.
            if (isNeighbor) 
            {
                if (isWhite && !onWhite) 
                {
                    if (move_tokens > 0)
                    {
                    InitiateMove(clickedTile.GetTopPosition(), true); 
                    move_tokens = move_tokens - 1;  
                    Debug.LogError(move_tokens + " moves left!");
                    }
                }
                else if (isBlack && onWhite) 
                {
                    if (move_tokens > 0)
                    {
                    InitiateMove(clickedTile.GetTopPosition(), false);
                    move_tokens = move_tokens -1;
                    Debug.LogError(move_tokens + " moves left!");
                    }
                }
                if (move_tokens == 0)
                {
                    // Debug.LogError("Rerolling...");
                    // move_tokens = Random.Range(2, 12);
                    Debug.LogError("You now have " + move_tokens + " moves left");

                }
            }
        } 
    }

    void InitiateMove(Vector3 destination, bool landingOnWhite)
    {
        targetPosition = destination;
        isMoving = true;
        onWhite = landingOnWhite;
    }

    void movePlayer() 
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.001f) 
        {
            transform.position = targetPosition;
            isMoving = false;
            UpdateCurrentTile();
        }
    }

    public void UpdateCurrentTile()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2.0f))
        {
            stage = hit.collider.gameObject;
        }
    }
}