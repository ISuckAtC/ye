using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private enum Stages { RangedStage, MeleeStage, MiniJudgeStage, SupremeJudgeStage};
    Stages stages;

    bool rangedActive = false;
    bool meleeActive = false;
    bool miniJudgeActive = false;
    bool supremeJudgeActive = false;

    [SerializeField] GameObject rangedGO;
    [SerializeField] GameObject meleeGO;
    [SerializeField] GameObject miniJudgeGO;
    [SerializeField] GameObject supremeJudgeGO;

    float rangedTimer = 0;
    float meleeTimer = 0;
    float miniJudgeTimer = 0;
    float supremeTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        stages = Stages.RangedStage;
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
                        stages = Stages.MeleeStage;
                    }
                }
                break;
            case Stages.MeleeStage:
                if (!meleeActive)
                {
                    if (meleeTimer < 5f)
                    {
                        meleeTimer += Time.deltaTime;
                    }
                    else
                    {
                        foreach (Transform t in meleeGO.transform)
                        {
                            t.GetComponent<EnemyBouncer>().enabled = true;
                            t.GetComponent<Enemy>().invincible = false;
                            meleeActive = true;
                        }
                    }
                }
                break;
            case Stages.MiniJudgeStage:
                break;
            case Stages.SupremeJudgeStage:
                break;
            default:
                break;
        }
    }
}
