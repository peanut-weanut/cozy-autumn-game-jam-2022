using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigScript : MonoBehaviour
{
    void Update(){
        transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.y * Vector3.up);
    }
}
