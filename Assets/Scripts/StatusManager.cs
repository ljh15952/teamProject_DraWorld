using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public float Hp;
    public float Mp;
    public float Stamina;

    public Image Hp_i;
    public Image Mp_i;
    public Image Stamina_i;


    private void Start()
    {
        Hp = Hp_i.fillAmount * 100;
        Mp = Mp_i.fillAmount * 100;
        Stamina = Stamina_i.fillAmount * 100;
    }

    public void ControlHp(float Control)
    {
        Hp += Control;
        Hp_i.fillAmount = Hp;
    }

    public void ControlMp(float Control)
    {
        Mp += Control;
        Mp_i.fillAmount = Mp/100;
    }
    public void ControlStamina(float Control)
    {
        Stamina += Control;
        Stamina_i.fillAmount = Stamina;
    }
}
