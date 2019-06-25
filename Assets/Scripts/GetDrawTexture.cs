using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDrawTexture : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        WWW www;

        string temp_url = "file://"; /// www 를 쓰기 위해선 uri 경로 앞에 file:// 붙어야함
        string url = Application.dataPath + "/Saved_Drawing_Charoctor/" + this.name + ".png"; // 사진이 있는 경로
        www = new WWW(temp_url + url);
        Texture2D t = www.texture;
        Rect rect = new Rect(0, 0, t.width, t.height);
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(t, rect, new Vector2(0.5f, 0.5f));


    }

    // Update is called once per frame
    void Update () {
		
	}
}
