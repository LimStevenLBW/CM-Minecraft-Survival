using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyController : NetworkBehaviour, Enemy
{
    public hpText hptext;
    public int maxHp;
    private int currentHp;
    public float jumpSpeed;
    public AudioClip jumpClip;
    public float speed;
    public bool canCastFireball = false;
    public projectile fireballPrefab;
    public SkinnedMeshRenderer mesh;
    public AudioSource audioSource;
    public AudioClip attackClip;

    public Material hitMat;
    private Material atlas;

    private Player player;
    private Rigidbody body;
    private Vector3 direction;

    private float distance;
    private bool shouldJump;
    private bool isStunned = false;
    private Coroutine StunCoroutine;
    // Start is called before the first frame update

    public override void OnNetworkSpawn()
    {
        body = GetComponent<Rigidbody>();
        //player = GameObject.FindGameObjectsWithTag("Player")[0];

        if (IsServer)
        {
            currentHp = maxHp;
            hptext.updateText(maxHp, currentHp);
            StartCoroutine(UpdateTarget());
            if (canCastFireball) StartCoroutine(ShootAtTarget());
        }

    }

    void Start()
    {
        atlas = mesh.material;
    }
    IEnumerator UpdateTarget()
    {
        while (true)
        {
            FindTarget();
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator ShootAtTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            if (shouldJump)
            {
                shouldJump = false;
                body.velocity += new Vector3(0, jumpSpeed, 0);
                audioSource.PlayOneShot(jumpClip);

                yield return new WaitForSeconds(1);
                CastFireball();
            }


        }
    }
    void FindTarget()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        List<Player> playerList = GameManager.Instance.playerList;

        float minimum = float.MaxValue;
        for (int i = 0; i < playerList.Count; i++)
        {
            Vector3 playerPosition = playerList[i].transform.position;
            distance = Vector3.Distance(playerPosition, transform.position);
            if (distance < minimum)
            {
                minimum = distance;
                if (distance < 25) shouldJump = true;
                player = playerList[i];
            }
        }

        if (player != null) direction = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            FindTarget();
            if(isStunned == false) Move();
            RotateTowardsPlayer();
        }

    }

    void RotateTowardsPlayer(){
        if (player == null) return;
        Vector3 rotation = Quaternion.LookRotation(player.transform.position - transform.position).eulerAngles;
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = Quaternion.Euler(rotation);
    }

    void Move(){   
        transform.position += direction.normalized * speed * Time.deltaTime;
    }

    void CastFireball()
    {

        audioSource.PlayOneShot(attackClip);
        Vector3 castPosition = transform.position + transform.forward;
        projectile fireball = Instantiate(fireballPrefab, castPosition, Quaternion.identity);
        Rigidbody fireballBody = fireball.GetComponent<Rigidbody>();
        Vector3 direction = player.transform.position - transform.position;
        fireballBody.AddForce(direction * 175);
    }

    public void TakeDamage(int damage, Vector3 direction)
    {
        //if (StunCoroutine != null) { StopCoroutine(StunCoroutine); }
        StartCoroutine(HitStun());

        currentHp -= damage;
        hptext.updateText(maxHp, currentHp);
        direction.y += 0.3f;
        body.AddForce(direction.normalized * 35f);
        
        if(currentHp <= 0)StartCoroutine(Die());

        audioSource.PlayOneShot(attackClip);
    }

    IEnumerator Die() {
        yield return new WaitForSeconds(0.5f);
        
        Score.Instance.AddScore(1 + (int)(maxHp * 0.1));
        Destroy(gameObject);
    }

    IEnumerator HitStun()
    {
        isStunned = true;
        mesh.material = hitMat;
        yield return new WaitForSeconds(1f);
        mesh.material = atlas;
        isStunned = false;

    }
}
