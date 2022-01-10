using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    public Animator animController;
    public Transform cam;

    public float speed = 6f;
    public float turnSmooth = 0.1f;
    float turnSmoothVelocity;

    Vector3 imagePos;
    public GameObject rangeCanvas;
    public Transform targetHolder;
    public Image targetImage;
    public Image rangeCircle;
    private Vector3 posUp;
    public float maxThrowDist;

    public GameObject monster;
    private GameObject monsterOnField;

    public int gameState;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        gameState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        switch (gameState)
        {
            case 0:

                //moving character
                if (direction.magnitude >= 0.1f)
                {
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

                    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    controller.Move(moveDir.normalized * speed * Time.deltaTime);

                    animController.SetBool("IsRunning", true);
                }
                else
                {
                    animController.SetBool("IsRunning", false);
                }
                break;

            case 1:

                // aiming monster capsule
                targetHolder.Translate(direction.normalized * speed * Time.deltaTime);

                // throwing monster capsule
                if (Input.GetKey("joystick button 0"))
                {
                    monsterOnField = Instantiate(monster, targetHolder.position, Quaternion.identity);
                    gameState = 2;
                }
                break;
            case 2:
                // Placeholder for battle sequence
                StartCoroutine(Battle());
                break;
        }


        // Throwing
        if (Input.GetKey("joystick button 5"))
        {
            if (gameState != 2) {
                gameState = 1;
                rangeCanvas.SetActive(true);
                rangeCanvas.transform.rotation = Quaternion.identity;
            }
        }
        else
        {
            gameState = 0;
            rangeCanvas.SetActive(false);
        }
    }

    IEnumerator Battle()
    {
        yield return new WaitForSeconds(3);
        Destroy(monsterOnField);
        gameState = 0;
    }
}
