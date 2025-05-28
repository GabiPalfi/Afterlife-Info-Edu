using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Movement")]
    public bool canMove=true;
    public float speed;
    public float scale;
    public float direction;
    public float c_y;
    public float c_x;
    public float x;
    public float stunDuration;

    [Header("Jumping")]
    public float jumpForce;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float rememberGroundedFor;
    float lastTimeGrounded;
    public LayerMask groundLayer;
    public int defaultAdditionalJumps = 1;
    public int additionalJumps;
    public bool canPressJump;

    [Header("Cheker")]
    public Transform isGroundedChecker;
    public bool isGrounded = false;
    public bool isDashReady = true;
    public bool isWalking;
    public float checkGroundRadius;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        additionalJumps = defaultAdditionalJumps;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        BetterJump();
        CheckIfGrounded();
        //Ciupic();
        WalkingChecker();
    }
    void Move() {
        if(canMove){
            float x = Input.GetAxisRaw("Horizontal");
            // float x = joyStick.Horizontal;
            // if(joyStick.Horizontal>=0.2f){
            //     x = 1;
            // }else if(joyStick.Horizontal <= -0.2f){
            //     x=-1;
            // }else{
            //     x=0;
            // }
            float moveBy = x * speed;
            rb.velocity = new Vector2(moveBy, rb.velocity.y);
            
            if(x > 0.01f){
                direction = c_x;
                transform.localScale = new Vector3(direction,c_y,1);
            }else{
                if(x < -0.01f){
                    direction = -c_x;
                    transform.localScale = new Vector3(direction,c_y,1);
                }
            }
        }
    }
    void Jump() {
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor || additionalJumps > 0)) {
            //Instantiate(jumpParticle,new Vector3(transform.position.x,transform.position.y,transform.position.z),Quaternion.identity);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            additionalJumps--;
            // anim.SetTrigger("TakeOf");
            // jump.Play();
        }
    }
    void BetterJump() {
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) {
            rb.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }   
    }
    void CheckIfGrounded() {
        Collider2D colliders = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);

        if (colliders != null) {
            isGrounded = true;
            additionalJumps = defaultAdditionalJumps;
            //anim.SetBool("IsJumping", false);
            //jumpPresed = false;
        } else {
            if (isGrounded) {
                lastTimeGrounded = Time.time;
            }
            isGrounded = false;
            //anim.SetBool("IsJumping", true);
        }
    }
    void WalkingChecker(){
        if(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D)){
            isWalking = true;
        }
    }
}
