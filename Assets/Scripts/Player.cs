using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public enum MoveTo
{
    Forward,
    Left,
    Right
}

public class Player : MonoBehaviour
{
    private bool isJumped = true;

    Animator animator;
    [SerializeField] private AnimationClip diedAnimation;

    [SerializeField] private float jumpForce;
    [SerializeField] private float platformSpeed;

    [SerializeField]
    private FragmentsGenerator fragmentsGenerator;

    [SerializeField] private Text scoreUI;
    [SerializeField] private Text maxScoreUI;
    [SerializeField] private Text moneysUI;

    int score = 0;
    int maxScore = 0;
    int moneys = 0;

    Rigidbody rb;

    [SerializeField] private float rotationSpeed = 10.0f;
    Quaternion targetRotation = Quaternion.identity;

    private void Start()
    {
        SwipeController.SwipeEvent += Jump;

        animator = GetComponent<Animator>();

        GameSaver.LoadPoints();

        moneys = GameSaver.pointsSaves.moneys;
        maxScore = GameSaver.pointsSaves.maxScore;

        maxScoreUI.text = maxScore.ToString();
        moneysUI.text = moneys.ToString();

        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void Jump(MoveTo moveTo)
    {
        if (isJumped)
        {
            switch (moveTo)
            {
                case MoveTo.Forward:
                    targetRotation = Quaternion.Euler(0, 0, 0);
                    MovePlayer(MoveTo.Forward);
                    break;

                case MoveTo.Right:
                    if (transform.position.x + 3 >= 4) return;

                    targetRotation = Quaternion.Euler(0, 45, 0);
                    MovePlayer(MoveTo.Right);
                    break;

                case MoveTo.Left:
                    if (transform.position.x - 3 <= -4) return;

                    targetRotation = Quaternion.Euler(0, -45, 0);
                    MovePlayer(MoveTo.Left);
                    break;
            }

            isJumped = false;
        }
    }

    void MovePlayer(MoveTo moveTo)
    {
        Vector3 forceVect = Vector3.zero;

        switch (moveTo)
        {
            case MoveTo.Forward:
                forceVect = new Vector3(0, jumpForce, jumpForce);
                break;
            case MoveTo.Right:
                forceVect = new Vector3(jumpForce, jumpForce, jumpForce);
                break;
            case MoveTo.Left:
                forceVect = new Vector3(-jumpForce, jumpForce, jumpForce);
                break;
        }

        fragmentsGenerator.Spawn();

        score += 1;
        scoreUI.text = score.ToString();

        rb.velocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Round(transform.position.z));

        rb.AddForce(forceVect);
    }

    public void PlayerDied()
    {
        if (score > maxScore)
            GameSaver.pointsSaves.maxScore = score;

        SwipeController.SwipeEvent -= Jump;

        GameSaver.pointsSaves.moneys = moneys;

        GameSaver.SavePoints();

        SceneManager.LoadScene("SampleScene");
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        isJumped = true;

        if (tag == "platform")
        {
            StartCoroutine(MovePlatform(collision.gameObject));

            NormalizationPos();
        }
        else if (tag == "MovePlatform")
        {
            transform.parent = collision.transform;

            StartCoroutine(MovePlatform(collision.gameObject));

            NormalizationPos();
        }
        if (tag == "MixController")
        {
            SwipeController.Left = MoveTo.Right;
            SwipeController.Right = MoveTo.Left;

            StartCoroutine(MovePlatform(collision.gameObject));
            
            NormalizationPos();
        }
        else if (tag == "Lava")
        {
            animator.SetTrigger(diedAnimation.name);

            isJumped = false;
        }
        if (tag == "Trap")
            PlayerDied();
    }

    void NormalizationPos()
    {
        float z = Mathf.Round(transform.position.z * 2) / 2;

        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Money")
        {
            moneys += 1;
            moneysUI.text = moneys.ToString();

            Destroy(col.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "MovePlatform")
            transform.parent = null;
        else if (tag == "MixController")
        {
            SwipeController.Left = MoveTo.Left;
            SwipeController.Right = MoveTo.Right;
        }
    }

    IEnumerator MovePlatform(GameObject platform)
    {
        while (true)
        {
            if (platform)
                if (platform.transform.position.y > -4f)
                {
                    platform.transform.Translate(Vector3.down * Time.deltaTime * platformSpeed);
                    yield return null;
                }
                else break;
            else break;
        }
    }
}
