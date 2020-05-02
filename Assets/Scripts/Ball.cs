using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    // Configuration
    [SerializeField] Paddle paddle;
    [SerializeField] float xPush = 10f;
    [SerializeField] float yPush = 5f;
    [SerializeField] AudioClip[] ballSounds;
    [SerializeField] float randomFactor = 0.2f;

    // States
    Vector2 distanceBetweenPaddleAndBall;
    bool hasStarted;

    // Cache
    AudioSource myAudioSource;
    Rigidbody2D myRigidBody; 

    // Start is called before the first frame update
    void Start()
    {
        hasStarted = false;
        distanceBetweenPaddleAndBall = transform.position - paddle.transform.position;
        myAudioSource = GetComponent<AudioSource>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            LockBallToPaddle();
            LaunchBallFromPaddle();
        }
        
    }

    private void LockBallToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle.transform.position.x, paddle.transform.position.y);
        transform.position = paddlePos + distanceBetweenPaddleAndBall;
    }

    private void LaunchBallFromPaddle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hasStarted = true;
            myRigidBody.velocity = new Vector2(xPush, yPush);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocityTweak = new Vector2(
            UnityEngine.Random.Range(0, randomFactor), 
            UnityEngine.Random.Range(0, randomFactor));

        if (hasStarted)
        {
            AudioClip clip = ballSounds[UnityEngine.Random.Range(0, ballSounds.Length)];
            myAudioSource.PlayOneShot(clip);
            myRigidBody.velocity += velocityTweak;
        }
    }
}
