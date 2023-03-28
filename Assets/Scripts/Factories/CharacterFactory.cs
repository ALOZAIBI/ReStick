using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFactory : MonoBehaviour
{
    public GameObject playerParty;
    public List<GameObject> characters = new List<GameObject>();
    public List<string> names = new List<string>();
    public static int prefabIndex = 0;
    //self explanatory name
    //maybe add parameter that chooses what team the character isin in case I want to create random enemy units as well etc..
    public void addRandomCharacterAsChild(GameObject parent) {
        //instantiates a random character
        int index = Random.Range(0, characters.Count - 1);
        Character temp =Instantiate(characters[index]).GetComponent<Character>();
        //sets its parent
        temp.transform.parent = parent.transform;
        //give it a random name
        index = Random.Range(0, names.Count - 1);
        temp.name = names[index];
        //maybe give it random color
    }
    //on starts adds all children to the abilities list and sets the index 
    private void Start() {
        foreach (Transform child in transform) {
            characters.Add(child.gameObject);
            Character temp = child.gameObject.GetComponent<Character>();
            temp.prefabIndex = prefabIndex++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
