using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keydoor : MonoBehaviour
{
    public enum DoorColor
    {
        Red,
        Blue,
        Green,
        Yellow
    }
    public DoorColor color;

    public void Start()
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
    public void AttemptOpen()
    {
        if (GameObject.Find("Player").GetComponent<PlayerController>().keys.Contains(color))
        {
            GameObject.Find("Player").GetComponent<PlayerController>().keys.Remove(color);

            switch (color)
            {
                case DoorColor.Red:
                    GameObject.Find("GameManager").GetComponent<GameManager>().redDoorOpen = true;
                    break;
                case DoorColor.Blue:
                    GameObject.Find("GameManager").GetComponent<GameManager>().blueDoorOpen = true;
                    break;
                case DoorColor.Green:
                    GameObject.Find("GameManager").GetComponent<GameManager>().greenDoorOpen = true;
                    break;
                case DoorColor.Yellow:
                    GameObject.Find("GameManager").GetComponent<GameManager>().yellowDoorOpen = true;
                    break;
                default:
                    break;
            }

            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            AttemptOpen();
        }
    }
}
