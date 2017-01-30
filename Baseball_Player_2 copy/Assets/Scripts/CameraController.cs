using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    This class is responsible for receiving swipe inputs and manage camera position based on camera_pivot game object
**/
public class CameraController : MonoBehaviour {

    public Transform[] cameraPivots;
    public float changeSpeed = 0.1f;
    private int currentPivot;



	// Use this for initialization
	void Start () {
        currentPivot = 0;
	}
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangePivot(-1);
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            ChangePivot(1);
        }
        if(this.transform.position != cameraPivots[currentPivot].position)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, cameraPivots[currentPivot].position, changeSpeed);
            this.transform.rotation = cameraPivots[currentPivot].rotation;
        }
		
	}
    // @param side to go: 1 for right, -1 for left (getted by user input)
    public int ChangePivot(int changeValue) {
        currentPivot += changeValue;
        if (currentPivot < 0) {
            currentPivot = 3;
        } else if(currentPivot > 3) {
            currentPivot = 0;
        }
        return currentPivot;
    }
}
