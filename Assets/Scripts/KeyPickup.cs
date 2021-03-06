using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public Keydoor.DoorColor color;
    // Start is called before the first frame update
    void Start()
    {
        Material m = GetComponent<Renderer>().material;
        switch (color)
        {
            case Keydoor.DoorColor.Red:
                m.color = Color.red;
                break;
            case Keydoor.DoorColor.Blue:
                m.color = Color.blue;
                break;
            case Keydoor.DoorColor.Green:
                m.color = Color.green;
                break;
            case Keydoor.DoorColor.Yellow:
                m.color = Color.yellow;
                break;
        }
        GetComponent<Renderer>().material = m;
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameObject.Find("Player").GetComponent<PlayerController>().keys.Add(color);
            Destroy(gameObject);
        }
    }
}
