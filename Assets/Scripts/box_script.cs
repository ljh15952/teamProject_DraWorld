using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box_script : MonoBehaviour
{
    public int box_hp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("weapon"))
        {
            Debug.Log("HITBOX");
            box_hp--;
            if (box_hp <= 0)
                Destroy(this.gameObject);
        }
    }
}
