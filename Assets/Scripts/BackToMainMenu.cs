using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToMainMenu(){
        SceneManager.LoadScene(0); // if you have time then just make it go to a copy of scene 1 with no dialogue
    }
}
