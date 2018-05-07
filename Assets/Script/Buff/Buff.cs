using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class Buff :NetworkBehaviour  {

    public string buffName;
    [Range(0,5)]
    public int actionTime;
    public Sprite sprite;
    [HideInInspector]
    public Image newImg;
    [HideInInspector]
    public float step;
    public virtual void ActiveBuff() {; }

    public void UpdateImage()
    {
        if(newImg!=null)
        newImg.fillAmount -= step;
    }

    public virtual void CalculateStep()
    {
        step = Time.deltaTime / actionTime;
    }

    public void SetImage()
    {
        GameObject gm = new GameObject(buffName);

        Image img = gm.AddComponent<Image>();
        img.sprite = sprite;
        img.rectTransform.sizeDelta = new Vector2(60, 60);
        img.type = Image.Type.Filled;
        img.fillAmount = 1;

        GameObject parentPlane = GameObject.Find("BuffPanel");
        gm.GetComponent<RectTransform>().SetParent(parentPlane.transform);


        if (actionTime!=0)
            Destroy(gm, actionTime);

        newImg = img;
    }
}
