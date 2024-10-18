using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{

    public Vector3 targetPosition;
    public float speed = 2f;
    private bool isMoving = false, hasDocked = false;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip carStartClip, crashClip, winClip;

    public float forceSpeed = 0.5f;
    private Rigidbody rb;
    public Vector3 initialPosition;
    public Quaternion initialRotation;

    [SerializeField]
    private ParticleSystem sparks;

    public delegate void OnDockedSuccessfully(string carColor);
    public static event OnDockedSuccessfully onDockedSuccessfully;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isMoving)
        {
            rb.MovePosition(transform.position + transform.forward * forceSpeed * Time.deltaTime);
            // audioSource.clip = carStartClip;
            // audioSource.Play();
        }
    }

    void OnMouseDown()
    {
        isMoving = true;
        PlayClip(carStartClip);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("TurnBlock"))
        {
            rb.rotation = collision.gameObject.transform.rotation;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            return;
        };

        if (CollisionDetectionByColor(collision) && Quaternion.Angle(collision.transform.rotation, transform.rotation.normalized) == 0)
        {
            Destroy(gameObject);
            if (!hasDocked)
            {
                hasDocked = true;
                onDockedSuccessfully(collision.gameObject.tag);
            }
            Destroy(collision.gameObject);

            // PlayClip(winClip);
        }

        else if (collision.gameObject.CompareTag("RedContainer") || collision.gameObject.CompareTag("RedCar") || collision.gameObject.CompareTag("BlueContainer") || collision.gameObject.CompareTag("BlueCar"))
        {
            sparks.gameObject.transform.position = collision.contacts[0].point;
            sparks.gameObject.SetActive(true);
            sparks.Play();
            PlayClip(crashClip);
            StartCoroutine(ResetCars(collision));
        }
    }

    private bool CollisionDetectionByColor(Collision collision)
    {
        if (gameObject.tag == "BlueCar" && collision.gameObject.CompareTag("BlueContainer"))
        {
            return true;
        }
        else if (gameObject.tag == "RedCar" && collision.gameObject.CompareTag("RedContainer"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator ResetCars(Collision collision)
    {
        yield return new WaitForSeconds(0.25f);
        isMoving = false;
        yield return new WaitForSeconds(0.75f);

        sparks.Stop();
        sparks.gameObject.SetActive(false);

        rb.position = initialPosition;
        rb.rotation = initialRotation;
        rb.velocity = Vector3.zero;

        collision.gameObject.transform.position = collision.gameObject.GetComponent<CarScript>().initialPosition;
        collision.gameObject.transform.rotation = collision.gameObject.GetComponent<CarScript>().initialRotation;
        collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void PlayClip(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
