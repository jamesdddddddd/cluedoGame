using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using UnityEngine.Assertions.Must;
using Unity.Netcode;
using turnyWurny;

public class Movement : NetworkBehaviour
{   
    //this is to specify the right camera to use for the raycast
    //hopefully this fixes the multiplayer clients
    //must now assign this for each player i believe
    private Camera playerCamera;  
    

    // Fields for rooms and door UI for entry and exit
    // Each one of these contains a room platform object for the player to teleport to when a door is found.
    public DoorUI doorui;
    public GameObject kitchen;
    public GameObject ball;
    public GameObject conserve;
    public GameObject dining;
    public GameObject billiard;
    public GameObject library;
    public GameObject lounge;
    public GameObject hall;
    public GameObject study;

    // For turn based
    public GameObject TurnMan;
    public TurnManager whomst;

    public Camera cam1;

    // Move speed for the player pieces.
    public float moveSpeed = 5f;

    // Fields for current tile and valid nearby ones.
    public GameObject stage; 
    public List<GameObject> nearby = new List<GameObject>();
    private bool onWhite = false;
    
    // Fields for movement target and switch.
    private Vector3 targetPosition;
    private bool isMoving = false;

    public int infinite_move = 1000;
    
    // TBC - Just helps us visualise and test movement atm, we will need to link this up with the dice mechanics.
    public int move_tokens = 0;
    public diceManager DM;
    private bool logged = false;




    // Makes sure that the player is in the designated starting position when the game begins.
    public override void OnNetworkSpawn() 
    {

        playerCamera = GetComponentInChildren<Camera>();
        if (IsOwner)
        {
            playerCamera.enabled = true;
            playerCamera.tag = "MainCamera";

        }
        else
        {
            playerCamera.enabled = false;
            // It's good practice to untag non-owner cameras
            playerCamera.tag = "Untagged";
        }

        // Search the scene for the diceManager script
        //this prevents the dice from throwing null execptions
        whomst = TurnMan.GetComponent<TurnManager>();
        DM = GameObject.FindAnyObjectByType<diceManager>();
        transform.position = new Vector3(4.5f, 0.5f, 0.5f);
        whereWeAt();
        //if doorUI is null, find it automatically to avoid nullreference errors
        if (doorui == null)
        {
            doorui = GameObject.FindAnyObjectByType<DoorUI>();
        }
    }

    // Checks every frame if the mouse was clicked to initiate movement if valid.
    void Update() 
    {

        playerCamera = GetComponentInChildren<Camera>();




        //add the isOwner guard, so this will not run if the user does not own the object the script is being run on
        if (!IsOwner) return;
        if (!IsSpawned) return;

        

        //protecet against update running before onnetworkspawn TESTING MAY BE TEMP
        if (DM == null) DM = GameObject.FindAnyObjectByType<diceManager>();

        //check if any of this is null, if so exit out of the loop to prevent errors till they are initialised in OnNetworkSpawn()

        if (DM == null || doorui == null || playerCamera == null || stage == null) return;


        //ensures the client is using raycasting from its camera rather than any other
        playerCamera = GetComponentInChildren<Camera>();

        //MORE TESTING 
        // Safety: If stage hasn't been found yet, try to find it and STOP here
        if (stage == null)
        {
            whereWeAt();
            return; // Exit Update early so we don't hit the crash on Line 100
        }




        if (stage.GetComponent<Door>() == null)
        {
            doorui.noMoDoor();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && !isMoving) 
        {
            if (whomst.phase == TurnStage.MOVING)
            {
                checkInput();
            }
        }

        if (isMoving) {
            movePlayer();
        }

        // Checks if the tile under the player is a door and prompts option of entry if so.
        if (stage.GetComponent<Door>() != null)
        {
            doorui.notifDoor("Room Found!");
        }
        
        
        // Clears door notification and button when player is no longer on door tile.
        if (stage.GetComponent<Door>() == null)
        {
            doorui.noMoDoor();
        }
       
    }


    // Method for entering a room, checks if the current tile under the player is a designated door of a room.
    public void roomEntry()
    {
        if (stage.GetComponent<KitchenDoor>() != null)
        {
         Kitchen kitchRoom = kitchen.GetComponent<Kitchen>();
         transform.position = kitchRoom.roomMove();
        }
        if (stage.GetComponent<BallDoor>() != null)
        {
         Ball ballRoom = ball.GetComponent<Ball>();
         transform.position = ballRoom.roomMove();
        }
        if (stage.GetComponent<ConsDoor>() != null)
        {
         Conservatory consRoom = conserve.GetComponent<Conservatory>();
         transform.position = consRoom.roomMove();
        }
        if (stage.GetComponent<DineDoor>() != null)
        {
         Dining dineRoom = dining.GetComponent<Dining>();
         transform.position = dineRoom.roomMove();
        }
        if (stage.GetComponent<BilliarDoor>() != null)
        {
         Billiard billRoom = billiard.GetComponent<Billiard>();
         transform.position = billRoom.roomMove();
        }
        if (stage.GetComponent<LibDoor>() != null)
        {
         Library libRoom = library.GetComponent<Library>();
         transform.position = libRoom.roomMove();
        }
        if (stage.GetComponent<LoungeDoor>() != null)
        {
         Lounge louRoom = lounge.GetComponent<Lounge>();
         transform.position = louRoom.roomMove();
        }
        if (stage.GetComponent<HallDoor>() != null)
        {
         Hall hallRoom = hall.GetComponent<Hall>();
         transform.position = hallRoom.roomMove();
        }
        if (stage.GetComponent<StudDoor>() != null)
        {
         Study studRoom = study.GetComponent<Study>();
         transform.position = studRoom.roomMove();
        }
        whereWeAt();
        doorui.roomExit();
    }



    // Uses raycasting to check what object the player is intending to click.
    void checkInput() 
    {

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = playerCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit)) 
        {
          
            if (!logged)
            {
                move_tokens = DM.diceSum;
                if (move_tokens > 0)
                {
                    logged = true;
                }
            }

            // Chekcs if the object clicked by the player is a Tile, if not, throws an error.
            Tile clickedTile = hit.collider.GetComponent<Tile>();
            if (clickedTile == null) 
            {
                Debug.LogWarning("Not a tile");
                return;
            }

            // If current tile of player is not found, throws error and updates the current tile.
            if (stage == null) 
            {
                Debug.LogError("Current tile not found");
                whereWeAt();
                return; 
            }

            if (stage.GetComponent<Tile>() != null)
            {
                // Accesses the script of the current tile  and checks for the neighbours.
                Tile currentStand = stage.GetComponent<Tile>();
                nearby = currentStand.neighbours;
            }

            bool isNeighbor = nearby.Contains(hit.collider.gameObject);
          
            // The logic of the board game is that you can only move vertically or horizontally, a checkerboard pattern helps a simple script facilitate this.
            bool isWhite = hit.collider.GetComponent<White>() != null;
            bool isBlack = hit.collider.GetComponent<Black>() != null;

            // Checks if the selected tile is valid: a vertical or horizontal neighbour of the current tile.
            if (isNeighbor) 
            {
                if (isWhite && !onWhite) 
                {
                    if (infinite_move > 0)
                    {
                    initiateMove(clickedTile.getTopPosition(), true); 
                    move_tokens = move_tokens - 1;  
                    Debug.LogError(move_tokens + " moves left!");
                    }
                }
                else if (isBlack && onWhite)
                {
                    if (infinite_move > 0)
                    {
                    initiateMove(clickedTile.getTopPosition(), false);
                    move_tokens = move_tokens -1;
                    Debug.LogError(move_tokens + " moves left!");
                    }
                }
                if (move_tokens == 0 && whomst.phase == TurnStage.MOVING)
                {
                    // Debug.LogError("Rerolling..."); This code was to allow further movement and creation of random values to imitate dice.
                    // move_tokens = Random.Range(2, 12);
                    whomst.phase = TurnStage.SUGGESTING;
                    Debug.LogError("You now have " + move_tokens + " moves left");

                }
            }
        } 
    }


    // Sets the target destination, flips the boolean for what tile type the player is currently on, and initates movement.
    void initiateMove(Vector3 destination, bool landingOnWhite)
    {
        targetPosition = destination;
        isMoving = true;
        onWhite = landingOnWhite;
    }

    
    // Initiates movement for the player, halts it once it is close enough to its target, and then updates the field for the tile under the player.
    void movePlayer() 
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.001f) 
        {
            transform.position = targetPosition;
            isMoving = false;
            whereWeAt();
        }
    }

    // Updates the field for current tile where the player is staged.
    public void whereWeAt()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2.0f))
        {
            stage = hit.collider.gameObject;
            Debug.Log(stage);
        }
    }

}