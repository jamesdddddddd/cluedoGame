using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    public List<GameObject> neighbours = new List<GameObject>();
    public LayerMask tiles;
    public GameObject stage;

    void Start()
    {
        helloNeighbour();
    
    }

    public void helloNeighbour()
    {
    neighbours.Clear();
    Collider[] colls = Physics.OverlapSphere(transform.position, 1.1f); 
    foreach (var c in colls)
    {
        if (c.gameObject != this.gameObject && c.GetComponent<Tile>() != null)
        {
            neighbours.Add(c.gameObject);
        }
    }
    }
    
    public Vector3 GetTopPosition()
    {
        return new Vector3(transform.position.x, 0.5f, transform.position.z);
    }
}