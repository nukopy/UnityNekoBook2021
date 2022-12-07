using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum CarStatus
{
    Ready, // 移動準備 OK
    Running, // 移動
    Stopped, // 停車
}

public class CarController : MonoBehaviour
{
    public CarStatus status;
    private Vector3 posSwipeStart;
    private Vector3 posSwipeEnd;
    private float speed;
    private float deceleration; // 減速率

    private const float INIT_SPEED = 0.0f;
    private const float INIT_DECELERATION = 1.0f;
    private const float DEFAULT_DECELERATION = 0.98f;
    private const float SPEED_STANDSTILL_THRESHOLD = 0.001f;
    private const float PARAM_CALC_V0 = 0.0002f;
    
    void Start()
    {
        this.Init();
    }
    
    void Update()
    {
        if (this.status == CarStatus.Stopped) return;
        
        // スワイプの長さで初速を変える
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Swipe started...");
            this.posSwipeStart = Input.mousePosition;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Swipe done!");
            this.posSwipeEnd = Input.mousePosition;
            float v0 = this.CalcV0(this.posSwipeStart.x, this.posSwipeEnd.x);
            
            switch (this.status)
            {
                case CarStatus.Ready:
                    this.StartRunning(v0);
                    break;
                case CarStatus.Running:
                    break;
                default:
                    // ここには来ないはず
                    break;
            }
            this.PlayAudio();
        }

        this.Run();
        this.CheckStopped();
    }

    public void Init()
    {
        Debug.Log("Init GameObject \"Car\"!");
        this.status = CarStatus.Ready;
        this.speed = INIT_SPEED;
        this.deceleration = INIT_DECELERATION;
    }

    /// <summary>
    /// スワイプの開始、終了位置の x 座標から初速度を計算する
    /// </summary>
    private float CalcV0(float startX, float endX)
    {
        float v0 = (endX - startX) * PARAM_CALC_V0;
        Debug.Log($"v0 from swipe lenght: {v0}");
        
        return v0;
    }
    
    private void StartRunning(float v0)
    {
        Debug.Log("Start running...");
        this.status = CarStatus.Running;
        this.speed = v0;
        this.deceleration = DEFAULT_DECELERATION;
    }

    private void Run()
    {
        this.transform.Translate(this.speed, 0, 0); // 移動
        this.speed *= this.deceleration; // 減速
    }

    private void CheckStopped()
    {
        if (this.status == CarStatus.Running && Mathf.Abs(this.speed) < SPEED_STANDSTILL_THRESHOLD)
        {
            Debug.Log("Car stopped.");
            this.status = CarStatus.Stopped;
            this.speed = INIT_SPEED;
            this.deceleration = INIT_DECELERATION;
        }
    }

    private void PlayAudio()
    {
        this.GetComponent<AudioSource>().Play();
    }
}
