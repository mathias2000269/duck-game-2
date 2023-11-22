using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPollo : MonoBehaviour
{
    public float speed;
    public float vidaSecondsCounter = 0;
    private Rigidbody2D rb2d;


    // Start is called before the first frame update
    void Start()
    {
        speed = 0.5f;
        rb2d = GetComponent<Rigidbody2D>();


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckCollisions(); 

        FlipLlamada();

        vidaSecondsCounter += Time.deltaTime;

        if (vidaSecondsCounter > 30)
        {
            FinPollo();
        }


    }



    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player"){
            Destroy(gameObject);
            //stunplayer
        }
    }

    private void FinPollo()
    {
        Destroy(gameObject);
    }



    #region FLIP
    public Vector2 velocidad;

    public bool flip, flipBool;
    [SerializeField] float segundosParaActivarFlip;

    [Header("Raycasts")]
    [SerializeField] Vector3 wallRaycastOffset;
    [SerializeField] float wallRaycastLength;
    public bool checkWall;

    [Header("LayerMasks")]
    [SerializeField] LayerMask groundLayer;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (checkWall)
        {
            Gizmos.color = Color.green;
        }

        //Wall Check
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * wallRaycastLength);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * wallRaycastLength);


    }

    private void CheckCollisions()
    {
        //Wall Collisions
        if ((Physics2D.Raycast(transform.position, Vector2.right, wallRaycastLength, groundLayer) || 
            Physics2D.Raycast(transform.position, Vector2.left, wallRaycastLength, groundLayer))
            && flipBool)

        {
            Flip();
            flipBool = false;
            StartCoroutine(FlipCount(segundosParaActivarFlip));
        }


        checkWall = Physics2D.Raycast(transform.position, Vector2.right, wallRaycastLength, groundLayer) ||
                    Physics2D.Raycast(transform.position, Vector2.left, wallRaycastLength, groundLayer);

    }

    IEnumerator FlipCount(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        flipBool = true;
    }


    void Flip()
    {
        flip = !flip;
    }

    private void FlipLlamada()
    {
        if (flip)
        {
            rb2d.MovePosition(rb2d.position - (velocidad * Time.fixedDeltaTime));
        }
        else
        {
            rb2d.MovePosition(rb2d.position + (velocidad * Time.fixedDeltaTime));
        }
    }
    #endregion

}
