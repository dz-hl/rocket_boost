using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float trustForce = 100f;
    [SerializeField] float rotationForce = 100f;
    [SerializeField] AudioClip soundThrust;
    [SerializeField] AudioClip soundDeath;
    [SerializeField] AudioClip soundFinish;
    //Particles
    [SerializeField] ParticleSystem particleThrust;
    [SerializeField] ParticleSystem particleDeath;
    [SerializeField] ParticleSystem particleFinish;
    
    Rigidbody rigidBody;
    AudioSource audioSource;
   
    enum State { Alive, Transcending, Dying }
    State state = State.Alive;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            ApplyThrusting();
            Rotate();
        }
    }

    private void ApplyThrusting()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            particleThrust.Play();
            rigidBody.AddRelativeForce(Vector3.up * trustForce);
            
            if (audioSource.isPlaying == false)
            {
                audioSource.PlayOneShot(soundThrust);
            }
        } else
        {
            audioSource.Stop();
            particleThrust.Stop();
        }
        
           
        
       
        
    }

    private void Rotate()
    {
        float rRotateForce = rotationForce * Time.deltaTime;
        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rRotateForce);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-(Vector3.forward * rRotateForce));
        }
        rigidBody.freezeRotation = false;


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;
            case "Finish":
                state = State.Transcending;
                LevelFinish();
                break;
            default:
                state = State.Dying;
                Die();
                break;
        }
    }

    private void LevelFinish()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(soundFinish);
        particleFinish.Play();
        Invoke("LoadNextScene", 1f);
        print("You've reached the end");
    }

    private void Die()
    {
        audioSource.Stop();
        particleThrust.Stop();
        audioSource.PlayOneShot(soundDeath);
        particleDeath.Play();
        Invoke("Restart", 2f);
        print("Dead");
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex > SceneManager.sceneCount)
            Restart();
        SceneManager.LoadScene(nextSceneIndex);
    }
    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

}
