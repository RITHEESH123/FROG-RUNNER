using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float jumpPower = 10f;
    public float secondJumpPower = 10f;
    public Transform groundCheckPosition;
    public float radius = 0.3f;
    public LayerMask layerGround;
    public GameObject smokePosition;

    private Rigidbody myBody;
    private bool isGrounded;
    private bool playerJumped;
    private bool canDoubleJump;

    private PlayerAnimation playerAnim;
    private BGScrollerScript bgScroller;
    private PlayerHealthDamageShoot playerShoot;
    private GameplayController gameplayController;

    private bool gameStarted;

    private Button jumpBtn;

    // Start is called before the first frame update
    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
        playerAnim = GetComponent<PlayerAnimation>();
        bgScroller = GameObject.Find("Background").GetComponent<BGScrollerScript>();
        playerShoot = GetComponent<PlayerHealthDamageShoot>();
        gameplayController = GameObject.Find("Gameplay Controller").GetComponent<GameplayController>();

        jumpBtn = GameObject.Find("JumpBtn").GetComponent<Button>();
        jumpBtn.onClick.AddListener(() => Jump());
    }

    void Start()
    {
        StartCoroutine(GameStarted());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameStarted)
        {
            PlayerMove();
            PlayerGrounded();
            PlayerJump();
        }
    }

    void PlayerMove()
    {
        myBody.velocity = new Vector3(movementSpeed, myBody.velocity.y, 0f);
    }

    void PlayerGrounded()
    {
        isGrounded = Physics.OverlapSphere(groundCheckPosition.position, radius, layerGround).Length > 0;
    }

    void PlayerJump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && !isGrounded && canDoubleJump)
        {
            canDoubleJump = false;
            myBody.AddForce(new Vector3(0, secondJumpPower, 0));
        }
        else if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            playerAnim.DidJump();
            myBody.AddForce(new Vector3(0, jumpPower, 0));
            playerJumped = true;
            canDoubleJump = true;
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerAnim.DidJump();
            myBody.AddForce(new Vector3(0, jumpPower, 0));
            playerJumped = true;
            canDoubleJump = true;
        }
        if (!isGrounded && canDoubleJump)
        {
            canDoubleJump = false;
            myBody.AddForce(new Vector3(0, secondJumpPower, 0));
        }
    }

    IEnumerator GameStarted()
    {
        yield return new WaitForSeconds(2f);
        gameStarted = true;
        bgScroller.canScroll = true;
        playerShoot.canShoot = true;
        gameplayController.canCountScore = true;
        smokePosition.SetActive(true);        
        playerAnim.PlayerRun();
    }

    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag == Tags.PLATFORM_TAG)
        {
            if (playerJumped)
            {
                playerJumped = false;
                playerAnim.DidLand();
            }
        }
    }


}//end
