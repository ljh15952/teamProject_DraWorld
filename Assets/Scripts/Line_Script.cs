using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Line_Script : MonoBehaviour {

    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCol;

    public float Destroytimer;
    float BlinkTimer = 0;
    float BlinkEndTimer = 4;

    public bool pUp = false;
    bool BlinkBool = true;

    List<Vector2> points;

    public void UpdateLine(Vector2 mousePos)
    {
        if (points == null)
        {
            points = new List<Vector2>();
            SetPoint(mousePos);
            return;
        }

        if (Vector2.Distance(points.Last(), mousePos) > 0.1f)
        {
            SetPoint(mousePos);
        }
    }

    private void Update()
    {
        if (pUp == false)
            return;

        Destroytimer -= Time.deltaTime;
        BlinkTimer += Time.deltaTime;

        if (Destroytimer < 0)
        {
            Destroy(this.gameObject);
        }

        if (BlinkTimer > BlinkEndTimer)
        {
            Debug.Log("ASDASDASD");
            if (BlinkBool)
            {
                BlinkBool = !BlinkBool;
            }
            else if (!BlinkBool)
            {
                BlinkBool = !BlinkBool;
            }
            BlinkEndTimer = 0.1f;
            BlinkTimer = 0;
        }
        GetComponent<LineRenderer>().enabled = BlinkBool;
    }

    void SetPoint(Vector2 point)
    {
        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);

        if (points.Count > 1)
            edgeCol.points = points.ToArray();
    }
}
