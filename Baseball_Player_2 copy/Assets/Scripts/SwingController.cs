using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.Net; 
using System.IO; 


// This class is responsible for handling requests for the API and apply them on Swing Animations
public class SwingController : MonoBehaviour {

	/** Payload data getted from API
         payload = {"swingname": swing_file_name,"rotX":rolls, "rotY":aimAngles, "rotZ":elevationAngles,
        "posX":xpositionVector, "posY":ypositionVector, "posZ":zpositionVector,
        "speed":sweetSpotVelocity}
     **/
	private string hip_url = "https://obscure-headland-45385.herokuapp.com/hips"; // right number
	private string url = "https://obscure-headland-45385.herokuapp.com/swings"; // left number
	private string url_start = "https://smartbat.herokuapp.com/start"; 
	private string url_stop = "https://smartbat.herokuapp.com/stop";
	private GameObject player;
	private Animator playerAnimator;
	public Text txt_debug;
	public Text hip_debug;
	public Text swing_accel;
	public Text hip_accel;
	public Text rhythm_score; 
	private double swing_data, hip_data; 
	private BatManager batManager;

	void Start()
	{
		player = GameObject.FindWithTag("Player");
		playerAnimator = player.GetComponent<Animator>();
//		batManager = GameObject.FindGameObjectWithTag("Bat").GetComponent<BatManager>();

	}
	public IEnumerator GetData(WWW conn)
	{

		Debug.Log("Connection Began");
		txt_debug.text = "Connection Began";
		yield return conn;
		if (conn.error == null) {
			// Request OK
			Debug.Log("200");
			Debug.Log("json:"+conn.text);
			playerAnimator.SetTrigger("swingIt");
			// Convert Json file into Jsonnode Object
			JSONNode jsonInput = JSON.Parse(conn.text);
			// Here you can handle multiple swings from the call, depending on your needs. Could be setted by button, by a for loop, etc;
			int swinglist = jsonInput.Count;
	


			JSONArray accelXJson = jsonInput[swinglist-1]["accelx"].AsArray;
			JSONArray accelYJson = jsonInput[swinglist-1]["accely"].AsArray;
			JSONArray accelZJson = jsonInput[swinglist-1]["accelz"].AsArray;
			Debug.Log ("swing list x" + (accelXJson).ToString());
			Debug.Log ("swing list y " + (accelYJson).ToString());
			Debug.Log ("swing list z" + (accelZJson).ToString());


			float[] accelX, accelY, accelZ;
			accelX = new float[accelXJson.Count];
			accelY = new float[accelYJson.Count];
			accelZ = new float[accelZJson.Count];
			//Gathers the rotation data
			for (int x = 0; x < accelZ.Length; x++) {
				accelX[x] = accelXJson[x].AsFloat;
				accelY[x] = accelYJson[x].AsFloat;
				accelZ[x] = accelZJson[x].AsFloat;
			}

			List<int> magnitude = vector_mag (accelX, accelY, accelZ);
			int max_accel = max_value (magnitude); 
			swing_accel.text = (((int)max_accel).ToString() + "Gs");
			swing_data =(double) max_accel;



			//Data for rotation and changing the position 
//			int i = 9;
//			// Debug
//			string testID = jsonInput[i]["_id"];
//			txt_debug.text = "Swing Number: " + (i + 1);
//			Debug.Log(jsonInput[0]["rotX"]);
//
//			// Create float arrays and convert json arrays from the input to float arrays. 
//			float[] rotX, rotY, rotZ;
//			float[] posX, posY, posZ; 
//			JSONArray rotXJson = jsonInput[swinglist-1]["rotX"].AsArray;
//			JSONArray rotYJson = jsonInput[swinglist-1]["rotY"].AsArray;
//			JSONArray rotZJson = jsonInput[swinglist-1]["rotZ"].AsArray;
//			JSONArray posXJson = jsonInput[swinglist-1]["posX"].AsArray;
//			JSONArray posYJson = jsonInput[swinglist-1]["posY"].AsArray;
//			JSONArray posZJson = jsonInput[swinglist-1]["posZ"].AsArray;
//			rotX = new float[rotXJson.Count];
//			rotY = new float[rotYJson.Count];
//			rotZ = new float[rotZJson.Count];
//			posX = new float[rotXJson.Count];
//			posY = new float[rotYJson.Count];
//			posZ = new float[rotZJson.Count];
//
//			//Gathers the rotation data
//			for (int x = 0; x < rotX.Length; x++) {
//				rotX[x] = rotXJson[x].AsFloat;
//				rotY[x] = rotYJson[x].AsFloat;
//				rotZ[x] = rotXJson[x].AsFloat;
//			}
//
//
//			//gathers the position data 
//			for (int z = 0; z < rotZ.Length; z++) {
//				posX[z] = posXJson[z].AsFloat;
//				posY[z] = posYJson[z].AsFloat; 
//				posZ[z] = posZJson[z].AsFloat;
//			}
//
//
//			// Set Arrays
//			batManager.SetRotationArrays(rotX, rotY, rotZ);
//			batManager.SetPositionArrays(posX, posY, posZ);
//			// Execute Bat Animation
			//batManager.GenerateAnimation();
//			Debug.Log ("hip and swing data log inside of function");
//			Debug.Log (hip_data);
//			Debug.Log (swing_data);
//			if (hip_data >= swing_data) {
//				rhythm_score.text = ((double)swing_data / hip_data * 100).ToString ();
//			} else { rhythm_score.text = ((double)hip_data/swing_data *100).ToString();
//			}


		}
		else
		{
			// Request Error
			Debug.LogError("Connection Error: " + conn.error);
			txt_debug.text = "Connection Error: " + conn.error;

		}
	}

	private List<int> vector_mag(float[] x, float[] y, float[] z){
		List<int> vector_magnitude = new List<int>();
		for (int i =0; i < x.Length; i++){
			float vector_square = Mathf.Sqrt (Mathf.Pow (x [i], 2) + Mathf.Pow (y [i], 2) + Mathf.Pow (z [i], 2));
			vector_magnitude.Add((int)vector_square);
		}
			
		return vector_magnitude;
	}

	private int max_value(List<int> vector_values){
		int max_val = 0; 
		for (int i=0; i<vector_values.Count; i++){
			if (vector_values[i] > max_val){
				max_val = vector_values [i];
			}
		}
	
		return max_val;
	}

	public IEnumerator GetDataHips(WWW conn)
	{

		Debug.Log("Hip Connection Began");
		hip_debug.text = "Connection Began";
		yield return conn;
		if (conn.error == null) {
			// Request OK
			Debug.Log("200");
			playerAnimator.SetTrigger("swingIt");
			// Convert Json file into Jsonnode Object
			JSONNode jsonInput = JSON.Parse(conn.text);
		
			int swinglist = jsonInput.Count; 




			JSONArray accelXJson = jsonInput[swinglist-1]["accelx"].AsArray;
			JSONArray accelYJson = jsonInput[swinglist-1]["accely"].AsArray;
			JSONArray accelZJson = jsonInput[swinglist-1]["accelz"].AsArray;
			Debug.Log ("hip list x" + (accelXJson).ToString());
			Debug.Log ("hip list y " + (accelYJson).ToString());
			Debug.Log ("hip list z" + (accelZJson).ToString());


			float[] accelX, accelY, accelZ;
			accelX = new float[accelXJson.Count];
			accelY = new float[accelYJson.Count];
			accelZ = new float[accelZJson.Count];
			//Gathers the rotation data
			for (int x = 0; x < accelZ.Length; x++) {
				accelX[x] = accelXJson[x].AsFloat;
				accelY[x] = accelYJson[x].AsFloat;
				accelZ[x] = accelZJson[x].AsFloat;
			}

			List<int> magnitude = vector_mag (accelX, accelY, accelZ);
			int max_accel = max_value (magnitude); 
			hip_accel.text = (((int)max_accel).ToString() + "Gs");
			hip_data = (double) max_accel;
			Debug.Log ("hip data");
			Debug.Log (hip_data);
			Debug.Log ("hip and swing data log inside of function");
			Debug.Log (hip_data);
			Debug.Log (swing_data);
			if (hip_data >= swing_data) {
				rhythm_score.text = ((int) ((double)swing_data / hip_data * 100)).ToString ();
			} else { rhythm_score.text = ((int) ((double)hip_data/swing_data *100)).ToString();
			}

			 
		}
		else
		{
			// Request Error
			Debug.LogError("Connection Error: " + conn.error);
			txt_debug.text = "Connection Error: " + conn.error;

		}
	}

	public void StartRecording()
	{
		SendRequest (url_start);	

	}

	public void StopRecording()
	{
		SendRequest (url_stop);

	}

	public string SendRequest(string url)
	{
		// Post request and in the future mongodb setup 
		HttpWebRequest req = (HttpWebRequest) WebRequest.Create (url);
		req.Method = "GET";
		string responseData;  
		using (var response = req.GetResponse ())
		using (var responseStream = response.GetResponseStream ()) {
			using (var reader = new StreamReader (responseStream)) { 
				responseData = reader.ReadToEnd ();

			}
		}
		return responseData;
	}

	public string PostToDatabase(string type)
	{
		// Post request and in the future mongodb setup 
		HttpWebRequest req = (HttpWebRequest) WebRequest.Create (url);
		req.Method = "POST";
		req.ContentType = "application/json";
		string responseData;  
		using (var stream = req.GetRequestStream ()) 
		//adds the type that we want to send the database 
		using (var writer = new StreamWriter (stream)) {
			var data = @"{""recording"":"""+type+@"""}";
			writer.Write(data);
		}
		using (var response = req.GetResponse ())
		using (var responseStream = response.GetResponseStream ()) {
			using (var reader = new StreamReader (responseStream)) { 
				responseData = reader.ReadToEnd ();

			}
		}
		return responseData;
	}

	public void GetAPIData()
	{
		WWW conn = new WWW(url);
		StartCoroutine(GetData(conn));
		conn = new WWW (hip_url);
		StartCoroutine(GetDataHips(conn));
	}

}
