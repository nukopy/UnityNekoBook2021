using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RouletteController : MonoBehaviour
{
    public Status status = Status.NotReady;
    private float rotSpeed;
    private float attenuationCoefficient;
    private const float ROTATION_SPEED = 10.0f;
    private const float STOP_THRESHOLD = 0.01f;
    
    void Start()
    {
        this.Ready(); 
    }
    
    void Update()
    {
        // クリックしたときの挙動: ルーレットが静止している場合はルーレットを回し、ルーレットが回っている場合はルーレットを停止する
        if (Input.GetMouseButtonDown(0))
        {
            switch (this.status)
            {
                case Status.Ready:
                    this.StartRotating();
                    break;
                case Status.Rotating:
                    this.StopRotating();
                    break;
                case Status.Stopping:
                    Debug.Log("Now \"Stopping\"...");
                    break;
                case Status.Finished:
                    Debug.Log("Now \"Finished\".");
                    this.Ready();
                    break;
                default:
                    throw new Exception("ここにはこないよーーー！");
            }
        }
        
        this.Rotate();
        this.CheckFinished();
    }

    private void Ready()
    {
        Debug.Log("Ready to start roulette game!!");
        this.status = Status.Ready;
        this.rotSpeed = 0f;
        this.attenuationCoefficient = 0f;
    }

    private void StartRotating()
    {
        Debug.Log("Start rotating...");
        this.status = Status.Rotating;
        this.rotSpeed = ROTATION_SPEED;
        this.attenuationCoefficient = 1.0f;
    }

    private void StopRotating()
    {
        Debug.Log("Stop rotating...");
        this.status = Status.Stopping;
        this.attenuationCoefficient = 0.94f + Random.Range(0.01f, 0.0599f);
    }

    private void CheckFinished()
    {
        if (this.rotSpeed < STOP_THRESHOLD && this.status == Status.Stopping)
        {
            Debug.Log("Game finished!!");
            this.status = Status.Finished;
            this.rotSpeed = 0f;
            this.attenuationCoefficient = 0f;
        }
    }

    private void Rotate()
    {
        transform.Rotate(0, 0, this.rotSpeed);
        this.rotSpeed *= this.attenuationCoefficient;
    }
}
