using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamCtrl2 : MonoBehaviour {

	public GameObject canvas;

	Transform rota;
	Transform frameParent;
	MeshRenderer debugFrame;
	Camera c;

	public Material sky;
	HSVtoRGB hsv;

	public Slider[] sliders;
	public InputField[] fields;
	float[] values = {0,0,0,0,0,0}; // 0 = UDpos, 1 = LRpos, 2 = Dist, 3 = FrameSize, 4 = FOV, 5 = rotate
	float[] valuemuls = { 20, 20, 10, 2.5f, 60, 90 };
	float[] valuesubs = { 10, 10, 0, -0.5f, -30, 45 };

	void Start () {
		c = GetComponent<Camera> ();
		rota = transform.parent ;
		DontDestroyOnLoad (rota.parent.gameObject);
		frameParent = GameObject.Find ("QuadMaster").transform;
		debugFrame = GameObject.Find ("Debug").GetComponent<MeshRenderer> ();
		hsv = gameObject.AddComponent<HSVtoRGB> ();

		debugFrame.enabled = false;
		canvas.SetActive (false);

		SetSliders ();
	}

	void Update () {
		
		UpdateSky ();

		if (Input.GetKeyDown (KeyCode.F5)) {
			canvas.SetActive (!canvas.activeSelf);
			debugFrame.enabled = !debugFrame.enabled;
		}
	}

	void SetSliders() {
		for (int i = 0; i < 6; i++) {
			UpdateParamFromSlider (i);
		}
	}

	public void UpdateParamFromSlider(int parameterID){
		UpdateParam (parameterID, true);
	}
	public void UpdateParamFromField(int parameterID){
		UpdateParam (parameterID, false);
	}


	void UpdateParam (int parameterID, bool fromSlider) {
		// 0 = UDpos, 1 = LRpos, 2 = Dist, 3 = FrameSize, 4 = FOV, 5 = rotate

		if (fromSlider) {
			fields [parameterID].text = 
				(sliders [parameterID].value * valuemuls [parameterID] - valuesubs [parameterID]).ToString();
		} else {
			fields [parameterID].text = ( Mathf.Clamp 
				(float.Parse (fields [parameterID].text),
					-valuesubs [parameterID], 
					valuemuls [parameterID] - valuesubs [parameterID])
			).ToString();

			sliders [parameterID].value = 
				(float.Parse (fields [parameterID].text) + valuesubs [parameterID]) / valuemuls [parameterID];
		}

		values [parameterID] = float.Parse (fields [parameterID].text);

		switch (parameterID) {
		case 0 :
			Vector3 temp = transform.localPosition;
			temp.y = values [0];
			transform.localPosition = temp;
			break;
		case 1 :
			temp = transform.localPosition;
			temp.x = values [1];
			transform.localPosition = temp;
			break;
		case 2 :
			temp = transform.localPosition;
			temp.z = values [2] * -1;
			transform.localPosition = temp;
			break;
		case 3 :
			frameParent.localScale = Vector3.one * values [3];
			break;
		case 4 : 
			c.fieldOfView = values [4];
			break;
		case 5:
			rota.eulerAngles = Vector3.right * values [5];
			break;
		}
	}

	void UpdateSky(){
		float r = sky.GetFloat ("_Rotation");
		r += Time.deltaTime * 0.5f;
		sky.SetFloat ("_Rotation", r);

		float s = Mathf.Sin (Time.realtimeSinceStartup * 0.05f) * 0.5f + 0.8f;
		sky.SetFloat ("_Exposure", s);

		// also have sky change tint
		Color c = hsv.Convert(Time.realtimeSinceStartup * 0.6f, 1, 0.69f);
		sky.SetColor ("_Tint", c);
	}
}
