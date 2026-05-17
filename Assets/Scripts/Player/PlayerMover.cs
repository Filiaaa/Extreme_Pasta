using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMover : MonoBehaviour
{
    [Header("UI & References")]
    public GameObject menu;
    public GameObject deathAnima;
    public GameObject deadCanvas;
    public GameObject jumpAnima;
    public GameObject learning;
    public CheckpointSystem checkpointSystem;
    public CameraMovement cameraMovement;

    [Header("Audio")]
    public AudioSource walkSound;
    public AudioSource jumpSound;

    [Header("Movement Settings")]
    public float movingSpeed = 5f;
    public float jumpImpulse = 10f;
    public float climbingSpeed = 3f;
    public float waitBeforeRestartTime = 2f;

    [Header("Climbing")]
    public Transform upStringPoint;
    public Transform downStringPoint;
    public float distBetwPlayerAndString;

    [Header("Special States")]
    public bool isClimbing;
    public bool onAirPlane;
    public bool sittingInJumper;

    public bool onGround { get; set; } = true;

    private Rigidbody2D rb;
    private Animator animator;
    private float defaultSpeed;
    private float horizontalInput;
    private float verticalInput;
    private Vector2 extraVelocity;
    private bool jumpRequested;
    private bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        defaultSpeed = movingSpeed;
    }

    private void Update()
    {
        HandleUIInput();
        CollectJumpInput();
    }

    private void FixedUpdate()
    {
        if (isDead || sittingInJumper) return;

        HandleMovement();
        HandleRotation();
        UpdateAnimations();
    }

    private void HandleUIInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isDead)
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void CollectJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
        }
    }

    private void HandleMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (isClimbing)
        {
            HandleClimbing();
            return;
        }

        if (onGround)
        {
            HandleGroundMovement();
        }
        else
        {
            HandleAirMovement();
        }

        TryJump();
    }

    private void HandleClimbing()
    {
        verticalInput = Input.GetAxis("Vertical");

        if (transform.position.y <= upStringPoint.position.y && transform.position.y >= downStringPoint.position.y)
        {
            rb.velocity = new Vector2(0, verticalInput * climbingSpeed);
        }
        else
        {
            ClampClimbingPosition();
            rb.velocity = Vector2.zero;
        }

        animator.SetBool("climbingStay", Mathf.Abs(verticalInput) < 0.1f);


        TryJump();
    }

    private void ClampClimbingPosition()
    {
        float clampedY = Mathf.Clamp(transform.position.y, downStringPoint.position.y, upStringPoint.position.y);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
    }

    private void HandleGroundMovement()
    {
        extraVelocity = Vector2.zero;
        rb.velocity = new Vector2(horizontalInput * movingSpeed, rb.velocity.y);
    }

    private void HandleAirMovement()
    {
        rb.velocity = new Vector2(horizontalInput * movingSpeed + extraVelocity.x, rb.velocity.y);
    }

    private void TryJump()
    {
        if (!jumpRequested) return;

        if (onGround || isClimbing)
        {
            ExecuteJump();
        }

        jumpRequested = false;
    }

    private void ExecuteJump()
    {
        if (!jumpSound.isPlaying) jumpSound.Play();

        transform.parent = null;
        sittingInJumper = false;
        isClimbing = false;
        rb.isKinematic = false;

        animator.SetBool("climbing", false);
        animator.SetBool("climbingStay", false);

        rb.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
    }

    private void HandleRotation()
    {
        if (isClimbing) return;

        if (horizontalInput > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void UpdateAnimations()
    {
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f && !isClimbing && !sittingInJumper;
        animator.SetBool("walk", isMoving);

        if (isMoving && !walkSound.isPlaying && onGround)
            walkSound.Play();
        else if (!isMoving || !onGround)
            walkSound.Stop();

        if (onGround)
        {
            jumpSound.Stop();
            jumpAnima.SetActive(false);
            animator.SetBool("jump", false);
        }
        else if (!isClimbing && !sittingInJumper)
        {
            jumpAnima.SetActive(true);
            animator.SetBool("jump", true);
        }
    }

    private void Die(string deathType)
    {
        if (isDead) return;
        isDead = true;

        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        movingSpeed = 0;

        animator.enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        deathAnima.SetActive(true);
        var deathAnimator = deathAnima.GetComponent<Animator>();
        deathAnimator.SetBool("water", deathType == "water");
        deathAnimator.SetBool("slice", deathType == "slice");
        deathAnimator.SetBool("gas", deathType == "gas");

        StartCoroutine(WaitBeforeRestart());
    }

    private System.Collections.IEnumerator WaitBeforeRestart()
    {
        yield return new WaitForSeconds(waitBeforeRestartTime);

        //checkpointSystem?.RespawnPlayer();
        //ResetPlayerState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetPlayerState()
    {
        isDead = false;
        rb.isKinematic = false;
        movingSpeed = defaultSpeed;
        extraVelocity = Vector2.zero;

        animator.enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;

        deathAnima.SetActive(false);
        var deathAnimator = deathAnima.GetComponent<Animator>();
        deathAnimator.SetBool("water", false);
        deathAnimator.SetBool("slice", false);
        deathAnimator.SetBool("gas", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        switch (collision.tag)
        {
            case "end":
                HandleLevelEnd();
                break;
            case "fire":
                Die("gas");
                break;
            case "water":
                Die("water");
                break;
            case "downDeath":
                Die("fall");
                break;
            case "deathCheeseGrater":
                Die("slice");
                break;
            case "checkPoint":
                HandleCheckpoint(collision, false);
                break;
            case "secondCheckPoint":
                HandleCheckpoint(collision, true);
                break;
        }
    }

    private void HandleLevelEnd()
    {
        PlayerPrefs.SetInt("end", 1);
        cameraMovement?.StartEnd();
        enabled = false;
    }

    private void HandleCheckpoint(Collider2D collision, bool isSecondWay)
    {
        var collider2D = collision.GetComponent<BoxCollider2D>();
        if (collider2D == null)
            return;

        if (checkpointSystem != null && !checkpointSystem.TryAdvanceCheckpoint(collider2D, isSecondWay))
            return;

        if (cameraMovement != null && cameraMovement.pointNumb == -1)
            learning.SetActive(false);

        if (cameraMovement != null)
        {
            cameraMovement.pointNumb++;
            cameraMovement.MoveToNextPoint();
        }

        collision.tag = "Untagged";
        collision.gameObject.SetActive(false);

        animator.SetBool("walk", false);
        walkSound.Stop();
        enabled = false;
    }

    public void SitOnAirPlane(Transform stayInJumperPos)
    {
        PrepareForSpecialState(stayInJumperPos);
    }

    public void StayUpAirPlane(Transform pointToArriving)
    {
        ExitSpecialState(pointToArriving);
    }

    public void SitInHighJumper(Transform stayInJumperPos)
    {
        PrepareForSpecialState(stayInJumperPos);
    }

    public void JumpOnHighJumper(float jumperSpeed, Animator jumperAnimator)
    {
        sittingInJumper = false;
        rb.isKinematic = false;
        extraVelocity = Vector2.right;
        rb.velocity = new Vector2(2, 5).normalized * jumperSpeed;
        jumperAnimator?.SetBool("rotate", true);
    }

    public void SetClimbingMode(GameObject stringGO)
    {
        isClimbing = true;
        animator.SetBool("climbing", true);
        rb.isKinematic = true;
        jumpAnima.SetActive(false);
        rb.velocity = Vector2.zero;

        float targetX = stringGO.transform.position.x + distBetwPlayerAndString;
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    private void PrepareForSpecialState(Transform targetPos)
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        sittingInJumper = true;

        animator.SetBool("walk", false);
        animator.SetBool("jump", false);
        jumpAnima.SetActive(false);

        if (targetPos != null)
            transform.position = targetPos.position;
    }

    private void ExitSpecialState(Transform targetPos)
    {
        sittingInJumper = false;
        rb.isKinematic = false;

        if (targetPos != null)
            transform.position = targetPos.position;
    }

    public void SetExtraVelocity(Vector2 velocity)
    {
        extraVelocity = velocity;
    }

    public void ResetExtraVelocity()
    {
        extraVelocity = Vector2.zero;
    }
}
