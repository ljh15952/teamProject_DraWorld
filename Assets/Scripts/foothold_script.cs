using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foothold_script : MonoBehaviour
{
    // Start is called before the first frame update
    public bool ispush;
    public Vector3 startpos;

    public GameObject footHoldDoor;

    void Start()
    {
        startpos = transform.position;
        ispush = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("foot"))
        {
            transform.position += new Vector3(0, -0.005f, 0);
            ispush = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("foot"))
        {
            ispush = false;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (!ispush && transform.position.y < startpos.y)
            transform.position += new Vector3(0, 0.005f, 0);
        if(transform.position.y  < startpos.y - 0.3f)
        {
            Debug.Log("OPEN THE DOOR");
            footHoldDoor.transform.position += new Vector3(0, 0.005f, 0);
        }
    }
}
