using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLogic : MonoBehaviour
{
    Animator anim;

    public InputSystem controls;

    private void Awake()
    {
        controls = new InputSystem();
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
        controls.CameraStates.ChangeView.performed += ctx => Test();
    }

    // Update is called once per frame
    void Update()
    {

       
    }

    void Test()
    {
        Debug.Log("you did the thing");
        if (anim.GetBool("lookatbook") == false)
        {
            anim.SetBool("lookatbook", true);
        }
        else
        anim.SetBool("lookatbook", false);
        Debug.Log(anim.GetBool("lookatbook"));
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
