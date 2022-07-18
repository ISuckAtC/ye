using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private enum Stages { RangedStage, JuryStage, MixLeftStage, MixRightStage, MiniJudgeStage, SupremeJudgeStage, Corridor};
    Stages stages;

    bool rangedActive = false;
    bool juryActive = false;
    bool mixLeftActive = false;
    bool mixRightActive;
    bool miniJudgeActive = false;
    bool supremeJudgeActive = false;
    bool corridorActive = false;

    [SerializeField] GameObject rangedGO;
    [SerializeField] GameObject juryGO;
    [SerializeField] GameObject mixLeftGO;
    [SerializeField] GameObject mixRightGO;
    [SerializeField] GameObject miniJudgeGO;
    [SerializeField] GameObject supremeJudgeGO;
    [SerializeField] GameObject corridorMixGO;

    float rangedTimer = 0;
    float juryTimer = 0;
    float mixLeftTimer = 0;
    float mixRightTimer = 0;
    float miniJudgeTimer = 0;
    float supremeTimer = 0;
    float corridorMixTimer = 0;

    public bool blueDoorOpen = false;
    public bool redDoorOpen = false;
    public bool greenDoorOpen = false;
    public bool yellowDoorOpen = false;



    // Start is called before the first frame update
    void Start()
    {
        stages = Stages.SupremeJudgeStage;
    }

    // Update is called once per frame
    void Update()
    {

        

        switch (stages)
        {
            case Stages.RangedStage:
                if (!rangedActive)
                {
                    if (rangedTimer < 2f)
                    {
                        rangedTimer += Time.deltaTime;
                    }
                    else
                    {
                        foreach (Transform t in rangedGO.transform)
                        {
                            t.GetComponent<EnemyOfficer>().enabled = true;
                            rangedActive = true;
                        }

                        
                    }
                }
                else
                {
                    if (rangedGO.transform.childCount == 0)
                    {
                        stages = Stages.JuryStage;
                    }
                }
                break;
            case Stages.JuryStage:
                if (!juryActive)
                {
                    if (juryTimer < 5f)
                    {
                        juryTimer += Time.deltaTime;
                    }
                    else
                    {
                        foreach (Transform t in juryGO.transform)
                        {
                            t.GetComponent<EnemyBouncer>().enabled = true;
                            t.GetComponent<Enemy>().invincible = false;
                            juryActive = true;

                        }
                    }
                }
                else
                {
                    if (juryGO.transform.childCount == 0)
                    {
                        stages = Stages.MixLeftStage;
                    }
                }
                break;
            case Stages.MixLeftStage:
                if (!mixLeftActive)
                {
                    if (mixLeftTimer < 5f)
                    {
                        mixLeftTimer += Time.deltaTime;
                    }
                    else
                    {
                        foreach (Transform t in mixLeftGO.transform)
                        {
                            if (t.name.Contains("Bouncer"))
                            {
                                t.GetComponent<EnemyBouncer>().enabled = true;
                                t.GetComponent<Enemy>().invincible = false;
                            }

                            if (t.name.Contains("Grabber"))
                            {
                                t.GetComponent<EnemyHugger>().enabled = true;
                                t.GetComponent<Enemy>().invincible = false;
                            }

                            mixLeftActive = true;
                        }
                    }
                }
                else
                {
                    if (mixLeftGO.transform.childCount == 0)
                    {
                        stages = Stages.MixRightStage;
                    }
                }
                break;
            case Stages.MixRightStage:
                if (!mixRightActive)
                {
                    if (mixRightTimer < 5f)
                    {
                        mixRightTimer += Time.deltaTime;
                    }
                    else
                    {
                        foreach (Transform t in mixRightGO.transform)
                        {
                            if (t.name.Contains("Bouncer"))
                            {
                                t.GetComponent<EnemyBouncer>().enabled = true;
                                t.GetComponent<Enemy>().invincible = false;
                            }

                            if (t.name.Contains("Grabber"))
                            {
                                t.GetComponent<EnemyHugger>().enabled = true;
                                t.GetComponent<Enemy>().invincible = false;
                            }

                            mixRightActive = true;
                        }
                    }
                }
                else
                {
                    if (mixRightGO.transform.childCount == 0)
                    {
                        stages = Stages.MiniJudgeStage;
                    }
                }
                break;
            case Stages.MiniJudgeStage:
                if (!miniJudgeActive)
                {
                    if (miniJudgeTimer < 5f)
                    {
                        miniJudgeTimer += Time.deltaTime;
                    }
                    else
                    {
                        foreach (Transform t in miniJudgeGO.transform)
                        {
                            t.GetComponent<EnemyBouncer>().enabled = true;
                            t.GetComponent<Enemy>().invincible = false;

                            miniJudgeActive = true;
                        }
                    }
                }
                else
                {
                    if (miniJudgeGO.transform.childCount == 0)
                    {
                        stages = Stages.SupremeJudgeStage;
                    }
                }
                break;
            case Stages.SupremeJudgeStage:
                if (!supremeJudgeActive)
                {
                    if (supremeTimer < 5f)
                    {
                        supremeTimer += Time.deltaTime;
                    }
                    else
                    {
                        foreach (Transform t in supremeJudgeGO.transform)
                        {
                            t.GetComponent<EnemyBouncer>().enabled = true;
                            t.GetComponent<Enemy>().invincible = false;

                            supremeJudgeActive = true;
                        }
                    }
                }
                else
                {
                    if (supremeJudgeGO.transform.childCount == 0)
                    {
                        stages = Stages.Corridor;
                    }
                }
                break;
            case Stages.Corridor:

                if (blueDoorOpen)
                {
                    if (!corridorActive)
                    {
                        if (corridorMixTimer < 5f)
                        {
                            corridorMixTimer += Time.deltaTime;
                        }
                        else
                        {
                            foreach (Transform t in corridorMixGO.transform)
                            {
                                if (t.name.Contains("Officer"))
                                {
                                    t.GetComponent<EnemyOfficer>().enabled = true;
                                    t.GetComponent<Enemy>().invincible = false;
                                }

                                if (t.name.Contains("Bouncer"))
                                {
                                    t.GetComponent<EnemyBouncer>().enabled = true;
                                    t.GetComponent<Enemy>().invincible = false;
                                }

                                if (t.name.Contains("Grabber"))
                                {
                                    t.GetComponent<EnemyHugger>().enabled = true;
                                    t.GetComponent<Enemy>().invincible = false;
                                }

                                corridorActive = true;
                            }
                        }
                    }
                    else
                    {
                        if (corridorMixGO.transform.childCount == 0)
                        {

                            //What happens when everything in the corridor gets killed
                            Debug.Log("a");
                        }
                    }
                }

                
                break;
            default:
                break;
        }
    }
}
