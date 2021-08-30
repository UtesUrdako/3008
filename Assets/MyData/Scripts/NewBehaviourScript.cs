using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        On();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void On()
    {

        Debug.Log("Log");
        Debug.LogWarning("LogWarning");
        Debug.LogError("LogError");
    }
}
