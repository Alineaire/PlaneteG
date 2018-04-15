using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class DataWrite : MonoBehaviour {
	
	string directory;
	string filename = "input_data.txt";

	void Start () {
		
		directory = Application.dataPath + "/../";

		if ( ! File.Exists (directory + filename)) {
			File.Create (directory + filename);
		}

	}

	public void AddLine(){
		StreamWriter writer = new StreamWriter (directory + filename, true);
		writer.WriteLine (System.DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss.fff"));
		writer.Close ();
	}

}
