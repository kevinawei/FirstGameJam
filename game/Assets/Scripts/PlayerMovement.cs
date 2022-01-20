using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 800;
    public float jumpSpeed = 18;
    private bool isGrounded = true;
    public bool isSwinging;

    float h;
    float v;

    void Awake(){

    }
    // Start is called before the first frame update
    void Start()
    {

    }
    void OnCollisionEnter2D (Collision2D col){
        isGrounded = true;
    }
    // Update is called once per frame
    void Update()
    {   
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Jump");

        move(h,v);
    }

    private void move(float h, float v){
        
        if (isGrounded && v == 1){
            jump();
            isGrounded = false;
        }
        else{
            rb.velocity = new Vector2(h * moveSpeed * Time.deltaTime, rb.velocity.y);
        }
   
    }
    
    private void jump(){
        rb.AddForce(new Vector2(0f, jumpSpeed), ForceMode2D.Impulse);
    }
}


