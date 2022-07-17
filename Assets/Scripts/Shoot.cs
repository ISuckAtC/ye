using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour
{
    public float range = 100f, impactForce = 1f, fireRate = 15f, reloadTime = 1f;
    public int damage = 10;
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
    bool changeColorNow = false;

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


        if (changeColorNow)
        {
            if (turnToRed)
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
                    turnToRed = false;
                }


            }
            else
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

                    greenSlider = 0;
                    blueSlider = 0;
                    changeColorNow = false;
                }
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
            if (hit.transform.tag == "Target")
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce, ForceMode.VelocityChange);

                hit.collider.gameObject.GetComponent<Renderer>().material.SetFloat("_EnemyHit", 1);
            }
            Enemy enemy;
            if (hit.transform.TryGetComponent<Enemy>(out enemy))
            {

                bool isEnemyDead = hit.rigidbody.gameObject.GetComponentInParent<Enemy>().TakeDamage(damage);
                if (isEnemyDead)
                    currentAmmo = maxAmmo;

                hitMaterial = hit.collider.gameObject.GetComponent<Renderer>().material;
                turnToRed = true;
                changeColorNow = true;

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
