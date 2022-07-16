using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour
{
    public float range = 100f, dmg = 10f, impactForce = 1f, fireRate = 15f, reloadTime = 1f;
    public int maxAmmo = 10;
    private int currentAmmo;
    private bool isReloading = false;
    private float nextTimeToFire = 0f;
    private Camera cam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffectEnemy, impactEffectObj;
    public Animator animator;
    bool turnToRed;
    float greenSlider = 1;
    float blueSlider = 1;
    Material hitMaterial;

    private void Start()
    {
        currentAmmo = maxAmmo;
        cam = Camera.main;
    }

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());

            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && transform.parent?.gameObject.tag == "MainCamera")
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Fire();
        }

        if (turnToRed)
        {
            if (greenSlider > 0 && blueSlider > 0)
            {
                greenSlider -= Time.deltaTime * 5;
                blueSlider -= Time.deltaTime * 5;


                hitMaterial.SetFloat("_GreenFloat", greenSlider);
                hitMaterial.SetFloat("_BlueFloat", blueSlider);
            }
            else
            {
                turnToRed = false;
            }


        }
        else
        {

            if (greenSlider < 1 && blueSlider < 1)
            {
                greenSlider += Time.deltaTime * 5;
                blueSlider += Time.deltaTime * 5;

                hitMaterial.SetFloat("_GreenFloat", greenSlider);
                hitMaterial.SetFloat("_BlueFloat", blueSlider);
            }
            else
            {

                greenSlider = 1;
                blueSlider = 1;
            }
        }
    }

    void Fire()
    {
        muzzleFlash.Play();

        currentAmmo--;

        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            Debug.Log("bbb");
            if (hit.transform.tag == "Target" && hit.transform != null)
            {
                Debug.Log(hit.rigidbody);
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce, ForceMode.VelocityChange);
                    Debug.Log("aaa");
                    Debug.Log(hit.collider.gameObject.name);

                    hit.collider.gameObject.GetComponent<Renderer>().material.SetFloat("_EnemyHit", 1);
                    turnToRed = true;
                    
                    // Do the enemy check separately to add force to other things when hit by a bullet
                    if (LayerMask.LayerToName(hit.rigidbody.gameObject.layer) == "Enemy")
                    {
                        bool isEnemyDead = hit.rigidbody.gameObject.GetComponent<Enemy>().TakeDamage(Mathf.RoundToInt(dmg));
                        if (isEnemyDead)
                            currentAmmo = maxAmmo;

                        hitMaterial = hit.collider.gameObject.GetComponent<Renderer>().material;

                    }
                }
            }

            GameObject impactObjGameObject = Instantiate(impactEffectObj, hit.point, Quaternion.LookRotation(Vector3.Reflect(cam.transform.forward, hit.normal)));

            ParticleSystem.MainModule mainModule = impactObjGameObject.GetComponent<ParticleSystem>().main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(hit.transform.GetComponent<Renderer>().material.color);

            Destroy(impactObjGameObject, 2f);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;

        Debug.Log("Reloading...");

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime + 1.5f);

        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(1f);

        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
