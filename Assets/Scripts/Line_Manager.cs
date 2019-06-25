using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line_Manager : MonoBehaviour {

    public GameObject LinePrefab;
    Line_Script ActiveLine;

    public bool isDrawing;

    public StatusManager StatusMng;

    private void Start()
    {
        isDrawing = false;
    }

    public void SetDrawingButton()
    {
        isDrawing = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonUp(0))
        {
            if (ActiveLine)
            {
                ActiveLine.pUp = true;
                ActiveLine = null;
                isDrawing = false;
            }
        }


        if (Input.GetMouseButtonDown(0)&&isDrawing)
        {
            GameObject LineStart = Instantiate(LinePrefab);
            ActiveLine = LineStart.GetComponent<Line_Script>();
        }



        if (ActiveLine)
        {
            if (StatusMng.Mp < 0.1f)
            {
                ActiveLine.pUp = true;
                ActiveLine = null;
                isDrawing = false;
                return;
            }
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ActiveLine.UpdateLine(mousePos);
            //minus Mp;
            StatusMng.ControlMp(-0.3f);
        }
    }
}
