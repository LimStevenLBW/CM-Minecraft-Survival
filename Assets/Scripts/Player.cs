using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

//Placing blocks
public class Player : NetworkBehaviour
{
    public projectile problastPrefab;
    public Ultimate ultimatePrefab;
    public Animator weaponAnimator;
    public ATACK atack;
    public Camera mainCamera;
    public Rigidbody body;

    public float speed;
    public float jumpSpeed;
    public float gravityBoost;
    public int jumpAmount;
    private Vector3 direction;
    private bool isGrounded;
    private int currentJumpCount;

    public List<GameObject> blockInventory;
    private GameObject selectedBlock;

    //Camera Variables
    private float yaw;
    private float pitch;
    public float mouseSensitivity;
    public Camera playerCamera;

    //Player Status Variables
    public float maxHp;
    private float currentHp;
    public float maxMana;
    private float currentMana;
    private Status playerStatus;
    private bool isInvincible = false;
    public AudioSource audioSource;
    public AudioClip attackClip;
    private bool isAttacking;
    public AudioClip jumpClip;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }

            playerCamera.enabled = false;
            playerCamera.gameObject.GetComponent<AudioListener>().enabled = false;
        }
        transform.position = new Vector3(0, 5, 5);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
        currentMana = maxMana;
        selectedBlock = blockInventory[0];
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(UpdateStatusCoroutine());
    }

    public void Respawn()
    {
        currentHp = maxHp;
        currentMana = maxMana;
        transform.position = new Vector3(10, 0, 10.14f);
        Score.Instance.ResetScore();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHp <= 0)
        {
            Respawn();
        }

        if (IsOwner)
        {
            CameraControl();
            PlaceBlock();
            Move();
            Jump();
            Attack();


            transform.position += direction.normalized * speed * Time.deltaTime;
        }

    }

    void FixedUpdate(){
        body.AddForce(Vector3.down * gravityBoost * body.mass);
    }

    void Move(){
       direction = new Vector3(0, 0, 0);

      if(Input.GetKey("w")){
            //direction.z = 1 ;
            direction += transform.forward;
      }

      if(Input.GetKey("s")){
            direction += transform.forward * -1;
      }


      if(Input.GetKey("a")){
            direction += transform.right * -1;
        }

      if(Input.GetKey("d")){
            direction += transform.right;
      }

      //BLOCK SWITCHING
      if (Input.GetKeyDown("1")) selectedBlock = blockInventory[0];
      if (Input.GetKeyDown("2")) selectedBlock = blockInventory[1];
      if (Input.GetKeyDown("3")) selectedBlock = blockInventory[2];
      if (Input.GetKeyDown("4")) selectedBlock = blockInventory[3];
      if (Input.GetKeyDown("5")) selectedBlock = blockInventory[4];
      if (Input.GetKeyDown("6")) selectedBlock = blockInventory[5];
      if (Input.GetKeyDown("7")) selectedBlock = blockInventory[6];
      if (Input.GetKeyDown("8")) selectedBlock = blockInventory[7];
      if (Input.GetKeyDown("9")) selectedBlock = blockInventory[8];


    }

    void Jump(){
      if(Input.GetKeyDown(KeyCode.Space)){

        if(isGrounded){
          currentJumpCount++;
          isGrounded = false;
          body.velocity += new Vector3(0, jumpSpeed, 0);
          audioSource.PlayOneShot(jumpClip);
            }
        else if(currentJumpCount < jumpAmount){
          currentJumpCount++;
          body.velocity += new Vector3(0, jumpSpeed, 0);
          audioSource.PlayOneShot(jumpClip);
            }

      }
    }

    public void HitTheGround()
    {
        isGrounded = true;
    }

    void PlaceBlock(){
      Vector3 placePosition = playerCamera.transform.forward * 2 + transform.position;

      placePosition = new Vector3(
          Mathf.Round(placePosition.x), 
          Mathf.Round(placePosition.y), 
          Mathf.Round(placePosition.z)
     );

        placePosition.y += 1;
    
        if(mainCamera.transform.rotation.eulerAngles.x >= 20  && mainCamera.transform.rotation.eulerAngles.x < 180)
        {

        }
        else if (mainCamera.transform.rotation.eulerAngles.x > 180 && mainCamera.transform.rotation.eulerAngles.x < 360 - 20)
        {
            //placePosition.y += 1f;
        }
      //placePosition.z += 4;
      //placePosition.y -= 1f;

    //PLACE BLOCK IF BLOCK EXISTS
      if (Input.GetMouseButtonDown(1) && selectedBlock){
        Instantiate(selectedBlock, placePosition, Quaternion.identity);
      }
    }

    void CameraControl(){
        yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");

        // Clamp pitch between lookAngle
        pitch = Mathf.Clamp(pitch, -80, 80);

        transform.localEulerAngles = new Vector3(0, yaw, 0);
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);

    }
    void Attack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (isAttacking == false)
            {
                weaponAnimator.SetTrigger("triggerAttack");
                audioSource.PlayOneShot(attackClip);
                StartCoroutine(AttackCoroutine());
            }

        }

        if (Input.GetKeyDown("q"))
        {
            if(Score.Instance.GetScore() >= 30)
            {
                currentMana -= 80;
                Score.Instance.AddScore(-30);
                audioSource.PlayOneShot(attackClip);
                Vector3 fireballers = mainCamera.transform.forward + mainCamera.transform.position;
                Ultimate ultimate = Instantiate(ultimatePrefab, fireballers, transform.rotation);
                ultimate.Track(mainCamera.transform);
            }
            

        }

        if (Input.GetKeyDown("f")){
            audioSource.PlayOneShot(attackClip);
            currentMana -= 10;
            Vector3 fireballers = mainCamera.transform.forward + mainCamera.transform.position;

            projectile blast = Instantiate(problastPrefab, fireballers, Quaternion.identity);
            Rigidbody blastBody = blast.GetComponent<Rigidbody>();
            blastBody.AddForce(mainCamera.transform.forward * 4000);

        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        atack.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        atack.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        isAttacking = false;
    }

    IEnumerator UpdateStatusCoroutine()
    {
        playerStatus = GameObject.FindWithTag("status").GetComponent<Status>();


        while (true)
        {
            if(currentMana <= 0) currentHp -= 3;

            if (currentMana < -10) currentMana = -10;

            if (currentHp <= maxHp) currentHp += 1;

            if (currentMana <= maxMana) currentMana += 1;
        
            playerStatus.UpdateHealth(currentHp / maxHp);
            playerStatus.UpdateMana(currentMana / maxMana);
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            isGrounded = true;
            currentJumpCount = 0;

        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            Vector3 direction = transform.position - collision.gameObject.transform.position;
            TakeDamage(5, direction);
        }
    }

    public void TakeDamage(int damage, Vector3 direction)
    {
        if (isInvincible == true) return;

        currentHp -= 10;
        direction.y += 0.3f;
        body.AddForce(direction.normalized * 2.5f, ForceMode.Impulse);
        isInvincible = true;
        StartCoroutine(EndInvincibility());
    }

    IEnumerator EndInvincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(.5f);
        isInvincible = false;
    }
}
