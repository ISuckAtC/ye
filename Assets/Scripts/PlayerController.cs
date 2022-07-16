using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;
    public MeshRenderer mr;
    public GameObject bullet;

    public GameObject bulletSpawnPoint;

    public float bulletSpeed;
    public float bulletLifeTime;
    public float bulletCooldown;
    public int maxAmmo;
    private float bulletCooldownTimer;
    private int currentAmmo;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        rb.velocity = new Vector3(input.x * speed, rb.velocity.y, input.y * speed);

        if (bulletCooldownTimer <= 0)
        {
            if (Input.GetMouseButton(0) && currentAmmo > 0)
            {
                bulletCooldownTimer = bulletCooldown;
                currentAmmo--;

                Vector3 direction = bulletSpawnPoint.transform.position - Camera.main.transform.position;

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
    }
}
