using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroControl : MonoBehaviour {

    public Rigidbody2D rb;
    public float jumpLow = 6f, jumpHigh = 5f;
    public Animator anim;

    public bool onGround;
    public Transform groundCheck;
    public float groundCheckRange = 0.02f;
    public LayerMask whatsGround;

    [SerializeField] private GameObject bala;
    [SerializeField] private GameObject canoArma;

    public bool face = true;
    public float move;
    public float maxSpeed = 5f;
    public float jumpForce = 5f;
    [SerializeField] private JoyControl joyC;

    // Use this for initialization
	void Awake () {


        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canoArma = GameObject.FindGameObjectWithTag("Arma");
        //groundCheck = transform.Find("ChaoCH");
    }
	
	// Update is called once per frame
	void Update () {

        if(onGround && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject balaInst = Instantiate(bala, canoArma.transform.position, Quaternion.identity) as GameObject;
            balaInst.GetComponent<MoveBullet>().Vel *= transform.localScale.x;
        }

        if (onGround)
        {
           
            anim.SetFloat("X", Mathf.Abs(move));
        }

        move = joyC.Hori();
        anim.SetFloat("Y", rb.velocity.y);
        anim.SetBool("Chao", onGround);
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(groundCheck.position, groundCheckRange);
    }

    private void FixedUpdate()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRange, whatsGround);
        rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);

        if (rb.velocity.y<0)
        {
            rb.gravityScale = jumpLow;
        }

        else if (rb.velocity.y > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            rb.gravityScale = jumpHigh;
        }

        else
        {
            rb.gravityScale = 1;
        }

        if(move > 0 && !face)
        {
            Flip();
        }

        else if(move < 0 && face)
        {
            Flip();
        }
    }

    void Flip()
    {
        face = !face;
        Vector3 tempScale = transform.localScale;
        tempScale.x *= -1;
        transform.localScale = tempScale;
    }

}
