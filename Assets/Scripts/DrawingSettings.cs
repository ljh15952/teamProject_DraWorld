using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DrawingSettings : MonoBehaviour {

    public float Transparency = 1f;

    public static Stack<GameObject> nowDraw = new Stack<GameObject>();

    private void Awake()
    {
      //  nowDraw.Push(gameObject);

        if (Application.platform == RuntimePlatform.Android)
        {
            if (!Directory.Exists(Application.persistentDataPath + "/Saved_Drawing_Charoctor"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Saved_Drawing_Charoctor");
            }
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (!Directory.Exists(Application.dataPath + "/Saved_Drawing_Charoctor"))
            {
                Directory.CreateDirectory(Application.dataPath + "/Saved_Drawing_Charoctor");
            }
        }
        else
        {
            if (!Directory.Exists(Application.dataPath + "/Saved_Drawing_Charoctor"))   // 폴더 없으면 생성
            {
                Directory.CreateDirectory(Application.dataPath + "/Saved_Drawing_Charoctor");
            }
            else
            {
                Debug.Log("you have save Directory");
            }
        }
    }

    public void SetMarkerWidth(float new_width)
    {
        DrawAble.Pen_Width = (int)new_width;
    }

    public void SetTransparency(float amount)
    {
        Transparency = amount;
        DrawAble.Pen_Color.a = Transparency;
    }


    public void SetMarkerRed()
    {
        Color c = Color.red;
        c.a = Transparency;
        DrawAble.Pen_Color = c;
    }
    public void SetMarkerGreen()
    {
        Color c = Color.green;
        c.a = Transparency;
        DrawAble.Pen_Color = c;
    }
    public void SetMarkerBlue()
    {
        Color c = Color.blue;
        c.a = Transparency;
        DrawAble.Pen_Color = c;
    }
    public void SetEraser()
    {
        Color c = new Color(255f, 255f, 255f, 0f);
        DrawAble.Pen_Color = c;
    }

    public void asdasd()
    {
        GameObject G = nowDraw.Peek();
        nowDraw.Pop();
        G.GetComponent<DrawAble>().BackBt();
    }


    public void HI(GameObject g)
    {
        g.transform.GetChild(0).GetComponent<Text>().text = Application.persistentDataPath;
        g.transform.GetChild(1).GetComponent<Text>().text = Application.dataPath;
    }
 

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            SceneManager.LoadScene("GameScene");
        }
    }

}

