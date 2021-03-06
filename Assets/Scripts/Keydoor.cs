using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                    GameObject.Find("GameManagerSL").GetComponent<GameManagerSL>().redDoorOpen = true;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    break;
                case DoorColor.Blue:
                    GameObject.Find("GameManager").GetComponent<GameManager>().blueDoorOpen = true;
                    Destroy(gameObject);
                    break;
                case DoorColor.Green:
                    GameObject.Find("GameManagerSL").GetComponent<GameManagerSL>().greenDoorOpen = true;

                    Destroy(gameObject);
                    break;
                case DoorColor.Yellow:
                    GameObject.Find("GameManager").GetComponent<GameManager>().yellowDoorOpen = true;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

                    break;
                default:
                    break;
            }

            
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
