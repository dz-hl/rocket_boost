using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    // todo remove from inspector later
    [Range(0,1)][SerializeField] float movementFactor;

    private Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;
    }


    void Update()
    {
        // set movement factor automatically
        if (period != 0)
        {
            float cycles = Time.time / period;

            const float tau = Mathf.PI * 2; // about 6.28
            float rawSinWave = Mathf.Sin(cycles * tau);
            movementFactor = rawSinWave / 2f + 0.5f;

            Vector3 offset = movementVector * movementFactor;
            transform.position = startingPos + offset;
        } 
    }
}
