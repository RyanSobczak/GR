using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class input : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        CheckInput();
	}

    void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {

        }
        else if (Input.GetKeyUp(KeyCode.W))
        {

        }
    }
}
