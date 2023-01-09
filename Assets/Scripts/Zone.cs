using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    //List of characters inside the Zone
    public List<Character> charactersInside = new List<Character>();

    //to detect which players in the zone
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Character")) {
            charactersInside.Add(collision.GetComponent<Character>());
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    void FixedUpdate()
    {
        
    }
}
