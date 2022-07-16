using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float range = 100f, dmg = 10f;
    public Camera cam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    void Fire()
    {
        muzzleFlash.Play();

        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            if (hit.transform.tag == "Target" && hit.transform != null)
            {
                Debug.Log("Damage Taken");

                //toDo: damage functionality

                GameObject impactGameObject = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(Vector3.Reflect(cam.transform.forward, hit.normal)));

                Destroy(impactGameObject, 2f);
            }
        }
    }
}
