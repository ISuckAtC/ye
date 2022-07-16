using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float interactRange;
    public float speed;
    public Vector2 turnSpeed;
    private Rigidbody rb;
    private MeshRenderer mr;
    public GameObject bullet;

    public GameObject head;
    public GameObject bulletSpawnPoint;

    public GameObject weaponMelee, weaponRange;



    public float bulletSpeed;
    public float bulletLifeTime;
    public float bulletCooldown;
    public int maxAmmo;
    private float bulletCooldownTimer;
    private int currentAmmo;

    private bool skipFirstFrame = true;


    public void PickupWeapon(GameObject weapon, Vector3 position, Quaternion rotation)
    {
        if (weapon.tag == "WeaponMelee")
        {
            if (weaponMelee)
            {
                DropWeapon(weaponMelee, position, rotation);
            }
            weaponMelee = weapon;
            weaponMelee.transform.position = position;
            weaponMelee.transform.rotation = rotation;
        }
        else if (weapon.tag == "WeaponRange")
        {
            if (weaponRange)
            {
                DropWeapon(weaponRange, position, rotation);
            }
            weaponRange = weapon;
            weaponRange.transform.position = position;
            weaponRange.transform.rotation = rotation;
        }
    }



    public void DropWeapon(bool range, Vector3 position, Quaternion rotation) // true to drop ranged weapon
    {
        if (range)
        {
            if (weaponRange != null)
            {
                weaponRange.transform.parent = null;
                weaponRange.transform.position = position;
                weaponRange.transform.rotation = rotation;
                weaponRange = null;
            }
        }
        else
        {
            if (weaponMelee != null)
            {
                weaponMelee.transform.parent = null;
                weaponMelee.transform.position = position;
                weaponMelee.transform.rotation = rotation;
                weaponMelee = null;
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();

        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (skipFirstFrame)
        {
            skipFirstFrame = false;
        }
        else
        {
            Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            transform.Rotate(Vector3.up, mouseInput.x * turnSpeed.x * Time.deltaTime);
            head.transform.Rotate(Vector3.right, -mouseInput.y * turnSpeed.y * Time.deltaTime);
        }


        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Vector3 movement = (input.x * transform.right + input.y * transform.forward);

        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);

        if (bulletCooldownTimer <= 0)
        {
            if (Input.GetMouseButton(0) && currentAmmo > 0)
            {
                bulletCooldownTimer = bulletCooldown;
                currentAmmo--;

                Vector3 direction = bulletSpawnPoint.transform.position - head.transform.position;

                GameObject bulletClone = Instantiate(bullet, bulletSpawnPoint.transform.position, Quaternion.identity);

                bulletClone.transform.forward = direction;

                bulletClone.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.transform.forward * bulletSpeed;
                Destroy(bulletClone, bulletLifeTime);
            }
        }
        else
        {
            bulletCooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentAmmo = maxAmmo;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactRange))
            {
                // TODO: put bool to indicate range or melee weapon selected
                DropWeapon(false, hit.point, Quaternion.Euler(Quaternion.Euler(0, 90, 0) * hit.normal));
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactRange))
            {
                if (hit.transform.tag == "WeaponMelee" || hit.transform.tag == "WeaponRange")
                {
                    PickupWeapon(hit.transform.gameObject, hit.transform.position, hit.transform.rotation);
                }
            }
        }
    }
}
