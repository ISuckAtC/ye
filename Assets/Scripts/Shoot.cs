using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float range = 100f, dmg = 10f, impactForce = 35f;
    public Camera cam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffectEnemy, impactEffectObj;

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

                if (hit.rigidbody != null)
                    hit.rigidbody.AddForce(-hit.normal * impactForce, ForceMode.VelocityChange);

                GameObject impactEnemyGameObject = Instantiate(impactEffectEnemy, hit.point, Quaternion.LookRotation(Vector3.Reflect(cam.transform.forward, hit.normal)));

                Destroy(impactEnemyGameObject, 2f);
            }
            else if (hit.transform.tag == "NotTarget" && hit.transform != null)
            {
                GameObject impactObjGameObject = Instantiate(impactEffectObj, hit.point, Quaternion.LookRotation(Vector3.Reflect(cam.transform.forward, hit.normal)));

                Destroy(impactObjGameObject, 2f);
            }
        }
    }
}
