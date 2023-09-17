using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;

public class Character : MonoBehaviourPunCallbacks
{
    public CharacterController controller;
    public WeaponManager weaponManager;

    public float speed;
    public float sprintSpeed;
    public float aimSpeed;
    public float gravity;
    public float jumpHeight;

    public Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;

    public bool isGrounded;

    public bool isSprinting;
    public bool isWalking;
    public Slider sprintSlider;
    public float Stamina;
    public float staminaDecay;
    public float staminaHeal;
    public float maxStamina;
    public bool emptyStamina;

    public string walkAnim;
    public string sprintAnim;
    public string sprintReturnAnim;
    public string sprintAnim2;
    public string jumpAnim;
    public string landAnim;
    public string shakeAnim;

    public GameObject sprintAnimObject;
    public GameObject sprintAnim2Object;
    public GameObject walkAnimObject;
    public GameObject jumpAnimObject;
    public GameObject landAnimObject;
    public GameObject shakeAnimObject;
    public bool LandFlick;

    public GameObject sprintSoundObject;
    public PhotonView PV;
    public bool aimed;

    public Camera mainCam;
    public int normalPOV;
    public int sprintPOV;
    public float povmultiplier;
    
    public GameObject crosshair;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            if(aimed)
            {
                crosshair.SetActive(false);
            }
            else
            {
                crosshair.SetActive(true);
            }

            if(isSprinting && mainCam.fieldOfView < sprintPOV)
            {
                mainCam.fieldOfView += povmultiplier * Time.deltaTime;
            }
            if(!isSprinting && mainCam.fieldOfView > normalPOV)
            {
                mainCam.fieldOfView -= povmultiplier * Time.deltaTime;
            }
            if (weaponManager.curentweapon == 1)
            {
                if (weaponManager.Primary.GetComponent<Gun>().aiming)
                {
                    aimed = true;
                }
                else
                {
                    aimed = false;
                }
            }

            if (weaponManager.curentweapon == 2)
            {
                if (weaponManager.Secondary.GetComponent<Gun>().aiming)
                {
                    aimed = true;
                }
                else
                {
                    aimed = false;
                }
            }

            if (isSprinting)
            {
                shakeAnimObject.GetComponent<Animation>().Play(shakeAnim);
            }

            if (Stamina < maxStamina)
            {
                sprintSlider.gameObject.SetActive(true);
            }
            else
            {
                sprintSlider.gameObject.SetActive(false);
            }
            //Audio
            if (isSprinting)
            {
                sprintSoundObject.SetActive(true);
            }
            else
            {
                sprintSoundObject.SetActive(false);
            }

            //Animations
            if (!isGrounded)
            {
                LandFlick = true;
            }

            if (LandFlick && isGrounded)
            {
                landAnimObject.GetComponent<Animation>().Play(landAnim);
                LandFlick = false;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                sprintAnimObject.GetComponent<Animation>().Play(sprintAnim);
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                sprintAnimObject.GetComponent<Animation>().Play(sprintReturnAnim);
            }

            if (isWalking && isGrounded && !aimed)
            {
                walkAnimObject.GetComponent<Animation>().Play(walkAnim);
            }

            if (isSprinting)
            {
                sprintAnim2Object.GetComponent<Animation>().Play(sprintAnim2);
            }


            //Sprinting
            sprintSlider.value = Stamina;
            sprintSlider.maxValue = maxStamina;

            if (Stamina > 10)
            {
                emptyStamina = false;
            }

            if (Stamina <= 0)
            {
                emptyStamina = true;
            }

            if (emptyStamina == true)
            {
                isSprinting = false;
            }

            if (Input.GetKey(KeyCode.LeftShift) && !emptyStamina)
            {
                isSprinting = true;
            }
            else
            {
                isSprinting = false;
            }

            if (isSprinting && Stamina > 0)
            {
                Stamina -= staminaDecay * Time.deltaTime;
            }
            else
            {
                if (Stamina < maxStamina)
                {
                    Stamina += staminaHeal * Time.deltaTime;
                }
            }

            //Movement
            if (Input.GetKey(KeyCode.W) && !isSprinting ||
                Input.GetKey(KeyCode.S) && !isSprinting ||
                Input.GetKey(KeyCode.A) && !isSprinting ||
                Input.GetKey(KeyCode.D) && !isSprinting)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            if (!isSprinting)
            {
                if (!aimed)
                {
                    controller.Move(move * speed * Time.deltaTime);
                }
                else
                {
                    controller.Move(move * aimSpeed * Time.deltaTime);
                }
            }
            else
            {
                controller.Move(move * sprintSpeed * Time.deltaTime);

            }

            velocity.y -= gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                Jump();
            }
        }
    }

    void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        jumpAnimObject.GetComponent<Animation>().Play(jumpAnim);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
