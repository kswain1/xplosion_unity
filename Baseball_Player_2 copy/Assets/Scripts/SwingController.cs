using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

// This class is responsible for handling requests for the API and apply them on Swing Animations
public class SwingController : MonoBehaviour {

    /** Payload data getted from API
         payload = {"swingname": swing_file_name,"rotX":rolls, "rotY":aimAngles, "rotZ":elevationAngles,
        "posX":xpositionVector, "posY":ypositionVector, "posZ":zpositionVector,
        "speed":sweetSpotVelocity}
     **/
    private string url = "https://obscure-headland-45385.herokuapp.com/swings";
    private GameObject player;
    private Animator playerAnimator;
    public Text txt_debug;
    private BatManager batManager;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();
        batManager = GameObject.FindGameObjectWithTag("Bat").GetComponent<BatManager>();
    }
    public IEnumerator GetData(WWW conn)
    {
        
        Debug.Log("Connection Began");
        txt_debug.text = "Connection Began";
        yield return conn;
        if (conn.error == null) {
            // Request OK
            Debug.Log("200");
            playerAnimator.SetTrigger("swingIt");
            // Convert Json file into Jsonnode Object
            JSONNode jsonInput = JSON.Parse(conn.text);
            // Here you can handle multiple swings from the call, depending on your needs. Could be setted by button, by a for loop, etc;
            int i = 9;
            // Debug
            string testID = jsonInput[i]["_id"];
            txt_debug.text = "Swing Number: " + (i + 1);
            Debug.Log(jsonInput[0]["rotX"]);

            // Create float arrays and convert json arrays from the input to float arrays. 
			float[] rotX, rotY, rotZ;
			float[] posX, posY, posZ; 
            JSONArray rotXJson = jsonInput[i]["rotX"].AsArray;
            JSONArray rotYJson = jsonInput[i]["rotY"].AsArray;
            JSONArray rotZJson = jsonInput[i]["rotZ"].AsArray;
            JSONArray posXJson = jsonInput[i]["posX"].AsArray;
            JSONArray posYJson = jsonInput[i]["posY"].AsArray;
            JSONArray posZJson = jsonInput[i]["posZ"].AsArray;
            rotX = new float[rotXJson.Count];
            rotY = new float[rotYJson.Count];
            rotZ = new float[rotZJson.Count];
			posX = new float[rotXJson.Count];
			posY = new float[rotYJson.Count];
			posZ = new float[rotZJson.Count];

			//Gathers the rotation data
            for (int x = 0; x < rotX.Length; x++) {
                rotX[x] = rotXJson[x].AsFloat;
            }
            for (int y = 0; y < rotY.Length; y++) {
                rotY[y] = rotYJson[y].AsFloat;
            }
            for (int z = 0; z < rotZ.Length; z++) {
                rotZ[z] = rotXJson[z].AsFloat;
            }
				
            //gathers the position data 
            for (int z = 0; z < rotZ.Length; z++) {
                posX[z] = posXJson[z].AsFloat;
                posY[z] = posYJson[z].AsFloat; 
                posZ[z] = posZJson[z].AsFloat;
            }
            

            // Set Arrays
            batManager.SetRotationArrays(rotX, rotY, rotZ);
            batManager.SetPositionArrays(posX, posY, posZ);
            // Execute Bat Animation
            batManager.GenerateAnimation();
        }
        else
        {
            // Request Error
            Debug.LogError("Connection Error: " + conn.error);
            txt_debug.text = "Connection Error: " + conn.error;

        }
    }
    public void GetAPIData()
    {
        WWW conn = new WWW(url);
        StartCoroutine(GetData(conn));
    }

}
