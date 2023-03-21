using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadToGame : MonoBehaviour
{
    
    void Start()
    {
        SceneManager.LoadScene("World");
    }

    
}
