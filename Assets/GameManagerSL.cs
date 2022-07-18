using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSL : MonoBehaviour
{
    private enum Stages { Everyone, KeyRoom, PlungerRoom};
    Stages stages;

    public GameObject supremeJudgeRoom;

    public bool redDoorOpen = false;
    public bool greenDoorOpen = false;

    bool plunderRoomActive = false;

    // Update is called once per frame
    void Update()
    {
        if (greenDoorOpen)
        {
            supremeJudgeRoom.GetComponent<EnemyBouncer>().enabled = true;
            greenDoorOpen = false;
        }
    }
}
