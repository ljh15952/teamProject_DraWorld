using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class StageSelectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
 

    public void Stage_Click(int stagenum)
    {
        Debug.Log(stagenum + "HIHI");
    }
}
