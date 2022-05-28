using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rd;
    public AudioSource hitAudioSource;
    private float ballLifetime = 1.5f;


    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Monster"))
        {
            return;
        }
       
        animator.SetTrigger("Capturing");

        hitAudioSource.Play();

        enabled = false;

        rd.useGravity = false;
        rd.velocity = Vector3.zero;
        rd.angularVelocity = Vector3.zero;

        transform.LookAt(collision.transform);
    }

    private void DestroyBall()
    {
        FindObjectOfType<GameManager>().BallDestroyed();
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        ballLifetime -= Time.deltaTime;
        if(ballLifetime <= 0)
        {
            DestroyBall();
        }
    }

    public void OnCapturedAnimationFinished()
    {
        DestroyBall();
    }
}
