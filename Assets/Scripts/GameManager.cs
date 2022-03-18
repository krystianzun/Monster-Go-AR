using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float velocityFactor = 0.001f;
    public float angleCorrection = 30;

    public Rigidbody ballPrefab;
    public Transform cameraTransform;
    public AudioSource throwAudioSource;

    private Vector2 startSwipePosition;
    private float startSwipeTime;
    private bool ballThrowingDisable;

    public RectTransform capturedMonsterHolder;
    public CapturedMonsterUI capturedMonsterUIPreFab;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.touchCount != 1 || ballThrowingDisable)
        {
            return;
        }

        var touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            startSwipePosition = touch.position;
            startSwipeTime = Time.time;
        }
        else if(touch.phase == TouchPhase.Ended)
        {
            // get velocity of the swipe
            var distance = (touch.position - startSwipePosition).magnitude;
            var deltaTime = Time.time - startSwipeTime;
            var velocity = distance / deltaTime;

            if(velocity < 2500)
            {
                return;
            }

            ThrowBall(Mathf.Min(velocity, 10000));
        }
    }

    private void ThrowBall(float velocity)
    {
        var position = cameraTransform.position + (cameraTransform.forward * 0.5f);
        var ballRigidBody = Instantiate(ballPrefab, position, cameraTransform.rotation);

        ballRigidBody.angularVelocity = Random.insideUnitSphere * Random.Range(0.5f, 2);

        var direction = Vector3.RotateTowards(cameraTransform.forward, Vector3.up, Mathf.Deg2Rad * angleCorrection, 0);

        ballRigidBody.velocity = direction * velocity * velocityFactor;

        ballThrowingDisable = true;
        throwAudioSource.Play();
    }

    public void BallDestroyed()
    {
        ballThrowingDisable = false;
    }

    public void MonsterCaptured(Monster monster)
    {
        MonsterSpawner.MonsterCapture();
        var newUIEntry = Instantiate(capturedMonsterUIPreFab, capturedMonsterHolder);
        newUIEntry.SetUp(monster.portraitSprite);
    }

    private void Awake()
    {
        foreach (Transform child in capturedMonsterHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
