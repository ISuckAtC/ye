using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Vector2 turnSpeed;
    private Rigidbody rb;
    private MeshRenderer mr;
    public GameObject bullet;

    public GameObject head;
    public GameObject bulletSpawnPoint;

    public float bulletSpeed;
    public float bulletLifeTime;
    public float bulletCooldown;
    public int maxAmmo;
    private float bulletCooldownTimer;
    private int currentAmmo;

    private bool skipFirstFrame = true;
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
    }
}
