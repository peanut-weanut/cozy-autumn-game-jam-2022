using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DrawingCamera : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    public InputSystem controls;
    [SerializeField]
    private bool dragging;
    private void Awake()
    {
        controls = new InputSystem();
        cam = transform.GetComponent<CinemachineVirtualCamera>();
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    void Start()
    {
        //cameras = GameObject.FindGameObjectsWithTag("Camera");
        controls.mouse.Drag.started += context => ToggleMovement();
        controls.mouse.Drag.canceled += context => ToggleMovement();
        
                
        CinemachineCore.GetInputAxis = GetAxisCustom;
         
        
    }
    
    void ToggleMovement(){
        if(dragging){
            dragging = false;
            // cam.m_Follow = null; // this is how we inherit rotation IN THE HOOD
        }
        else{
            dragging = true;
            // cam.m_Follow = Camera.main.transform;
        }
    }


    public float GetAxisCustom(string axisName)
    {
        if(dragging)
            return controls.mouse.MouseLook.ReadValue<Vector2>().y;
        else 
            return 0;
    }
}
