using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public float interactRange;
    public float speed;
    public Vector2 turnSpeed;
    private Rigidbody rb;
    private MeshRenderer mr;

    public GameObject head;

    public GameObject weaponMelee, weaponRange;
    public float health = 100;

    private float targetFOV;
    public float FOVChangeSpeedUp, FOVChangeSpeedDown;

    public float cameraBopAdjustSpeed;
    public float cameraBopSpeed;
    public float cameraBopLength;
    public float cameraBopHeight;
    private Vector3 cameraBopTarget;

    private bool skipFirstFrame = true;

    public float mouseSensitivity;

    private Vector3 meleeRotation, meleePosition, rangeRotation, rangePosition;

    public List<Keydoor.DoorColor> keys = new List<Keydoor.DoorColor>();

    public Material eyeVignetteMtrl;
    float gainHealthTimer = 5f;
    float initialHealthTimer;
    int c = 0;
    bool dying = false;
    float eyesApperture;
    bool invincible = false;

    public GameObject audioController;
    private GameObject camera;

    public void TakeDamage(int damage)
    {
        if (!invincible)
        {
            c++;
            gainHealthTimer = Mathf.Min(initialHealthTimer + c, 8f);
            health -= damage;
            if (health <= 0)
            {
                Debug.Log("YOU ARE DEAD!");
                dying = true;
                speed = 0;
                eyesApperture = (100 - health) / 200;
                camera.SetActive(false);
                SoundController.sounds.PlaySound(SoundController.sounds.Death, transform.position);
            }
            else
            {
                SoundController.sounds.PlaySound(SoundController.sounds.Hurt, transform.position);
            }
            eyeVignetteMtrl.SetFloat("_Exponential", (100 - health) / 200);
        }

    }

    public void GainHealth()
    {
        health += 10;
        gainHealthTimer = initialHealthTimer;
        eyeVignetteMtrl.SetFloat("_Exponential", (100 - health) / 200);

    }



    public void PickupWeapon(GameObject weapon, Vector3 position, Quaternion rotation)
    {
        if (weapon.tag == "WeaponMelee")
        {
            if (GetComponent<SwitchWeapon>().selectedWeapon != 0)
            {
                GetComponent<SwitchWeapon>().selectedWeapon = 0;
                weaponMelee.SetActive(true);
                weaponRange.SetActive(false);
            }

            if (weaponMelee)
            {
                DropWeapon(weaponMelee, position, rotation);
            }
            weapon.GetComponent<Rigidbody>().isKinematic = true;
            weapon.GetComponent<Collider>().enabled = false;
            weaponMelee = weapon;
            weaponMelee.transform.parent = Camera.main.transform;
            weaponMelee.transform.localPosition = meleePosition;
            weaponMelee.transform.localEulerAngles = meleeRotation;


        }
        else if (weapon.tag == "WeaponRange")
        {
            if (GetComponent<SwitchWeapon>().selectedWeapon != 1)
            {
                GetComponent<SwitchWeapon>().selectedWeapon = 1;
                weaponMelee.SetActive(false);
                weaponRange.SetActive(true);
            }

            if (weaponRange)
            {
                DropWeapon(weaponRange, position, rotation);
            }
            weapon.GetComponent<Rigidbody>().isKinematic = true;
            weapon.GetComponent<Collider>().enabled = false;
            weaponRange = weapon;
            weaponRange.transform.parent = Camera.main.transform;
            weaponRange.transform.localPosition = rangePosition;
            weaponRange.transform.localEulerAngles = rangeRotation;


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
                weaponRange.GetComponent<Rigidbody>().isKinematic = false;
                weaponRange.GetComponent<Collider>().enabled = true;
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
                weaponMelee.GetComponent<Rigidbody>().isKinematic = false;
                weaponMelee.GetComponent<Collider>().enabled = true;
                weaponMelee = null;
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("FPSCamera");
        mouseSensitivity = PlayerPrefs.GetFloat("sensitivity", 1);
        initialHealthTimer = gainHealthTimer;
        eyeVignetteMtrl.SetFloat("_Exponential", 0);
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();

        Cursor.lockState = CursorLockMode.Locked;


        if (weaponMelee)
        {
            meleeRotation = weaponMelee.transform.localEulerAngles;
            meleePosition = weaponMelee.transform.localPosition;
        }

        if (weaponRange)
        {
            rangeRotation = weaponRange.transform.localEulerAngles;
            rangePosition = weaponRange.transform.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (dying)
        {
            if (eyesApperture < 10)
            {
                eyesApperture += Time.deltaTime * 2;
                invincible = true;
                eyeVignetteMtrl.SetFloat("_Exponential", eyesApperture);

            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                invincible = false;
                eyesApperture = 0;
                health = 100;
                eyeVignetteMtrl.SetFloat("_Exponential", eyesApperture);
                camera.SetActive(true);

                dying = false;
            }
        }

        if (health < 100)
        {
            if (gainHealthTimer > 0)
            {
                gainHealthTimer -= Time.deltaTime;
            }
            else
            {
                GainHealth();
                gainHealthTimer = 5f;
                c = 0;
            }
        }


        if (skipFirstFrame)
        {
            skipFirstFrame = false;
        }
        else
        {
            Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

            float oldAngle = Mathf.DeltaAngle(head.transform.localEulerAngles.x, 0);

            transform.Rotate(Vector3.up, mouseInput.x * turnSpeed.x * Time.deltaTime);
            head.transform.Rotate(Vector3.right, -mouseInput.y * turnSpeed.y * Time.deltaTime);

            

            float newAngle = Mathf.DeltaAngle(head.transform.localEulerAngles.x, 0);

            //Debug.Log(newAngle);

            if (mouseInput.y > 0 && newAngle < oldAngle)
            {
                head.transform.localEulerAngles = new Vector3(-90, head.transform.localEulerAngles.y, head.transform.localEulerAngles.z);
            }
            else if (mouseInput.y < 0 && newAngle > oldAngle)
            {
                head.transform.localEulerAngles = new Vector3(90, head.transform.localEulerAngles.y, head.transform.localEulerAngles.z);
            }
        }


        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (input.magnitude > 0)
        {
            if (targetFOV != 60 + speed)
            {
                targetFOV = 60 + speed;
            }

            float cameraX = Mathf.PingPong(Time.timeSinceLevelLoad * cameraBopSpeed, 4f) - 2f;

            cameraBopTarget = new Vector3(cameraX * cameraBopLength, Mathf.Sqrt(4f - (cameraX * cameraX)) * cameraBopHeight, 0f);
        }
        else
        {
            if (targetFOV != 60)
            {
                targetFOV = 60;
            }

            cameraBopTarget = Vector3.zero;
        }

        if (Camera.main.transform.localPosition != cameraBopTarget)
        {
            Camera.main.transform.localPosition = Vector3.MoveTowards(Camera.main.transform.localPosition, cameraBopTarget, cameraBopAdjustSpeed * Time.deltaTime);
        }

        if (Camera.main.fieldOfView < targetFOV)
        {
            Camera.main.fieldOfView += FOVChangeSpeedUp * Time.deltaTime;
            if (Camera.main.fieldOfView > targetFOV)
            {
                Camera.main.fieldOfView = targetFOV;
            }
        }
        else if (Camera.main.fieldOfView > targetFOV)
        {
            Camera.main.fieldOfView -= FOVChangeSpeedDown * Time.deltaTime;
            if (Camera.main.fieldOfView < targetFOV)
            {
                Camera.main.fieldOfView = targetFOV;
            }
        }

        Vector3 movement = (input.x * transform.right + input.y * transform.forward);

        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);


        if (Input.GetKeyDown(KeyCode.X))
        {
            //audioController.GetComponent<AudioController>().defineNewLoops(new int[] { 1, 3, 4, });
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactRange))
            {
                // TODO: put bool to indicate range or melee weapon selected

                bool isRange = (GetComponent<SwitchWeapon>().selectedWeapon == 1);

                DropWeapon(isRange, hit.point, Quaternion.Euler(Quaternion.Euler(0, 90, 0) * hit.normal));
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactRange))
            {
                Debug.Log("Hit " + hit.collider.name);
                if (hit.transform.tag == "WeaponMelee" || hit.transform.tag == "WeaponRange")
                {
                    Debug.Log("Picking up weapon");
                    PickupWeapon(hit.transform.gameObject, hit.transform.position, hit.transform.rotation);
                }
            }
        }
    }


}
