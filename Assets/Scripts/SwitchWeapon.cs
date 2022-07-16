using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapon : MonoBehaviour
{
    public int selectedWeapon = 0;
    private PlayerController pC;

    // Start is called before the first frame update
    void Start()
    {
        pC = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;

            switch (selectedWeapon)
            {
                case 0:
                    if (pC.weaponMelee)
                    {
                        pC.weaponMelee.SetActive(true);
                        pC.weaponRange.SetActive(false);
                    }
                    else
                        selectedWeapon = 1;
                    break;

                case 1:
                    if (pC.weaponRange)
                    {
                        pC.weaponRange.SetActive(true);
                        pC.weaponMelee.SetActive(false);
                    }
                    else
                        selectedWeapon = 0;
                    break;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = 1;
            else
                selectedWeapon--;

            switch (selectedWeapon)
            {
                case 0:
                    if (pC.weaponMelee)
                    {
                        pC.weaponMelee.SetActive(true);
                        pC.weaponRange.SetActive(false);
                    }
                    else
                        selectedWeapon = 1;
                    break;

                case 1:
                    if (pC.weaponRange)
                    {
                        pC.weaponRange.SetActive(true);
                        pC.weaponMelee.SetActive(false);
                    }
                    else
                        selectedWeapon = 0;
                    break;
            }
        }
    }
}
