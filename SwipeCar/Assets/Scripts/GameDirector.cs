using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    private GameObject car;
    private GameObject flag;
    private float distance;
    private GameObject distanceText;
    
    // Start is called before the first frame update
    void Start()
    {
        this.Init();
    }

    // Update is called once per frame
    void Update()
    {
        this.distance = this.flag.transform.position.x - this.car.transform.position.x;
        
        // 車の状態に応じてテキストを出力
        string text = "";
        switch (this.GetCarStatus())
        {
            case CarStatus.Ready:
            case CarStatus.Running:
                // 車が停止するまでは、旗と車間の距離を表示
                text = $"ゴールまで {this.distance.ToString("F2")} m";
                break;
            case CarStatus.Stopped:
                // 車が停止したら、結果に応じたテキストを表示
                text = GenerateResultTextFromDistance(this.distance);
                break;
            default:
                break;
        }
        
        this.UpdateText(text);
    }

    private void Init()
    {
        Debug.Log("Init \"GameDirector\"!");
        this.car = GameObject.Find("Car");
        this.flag = GameObject.Find("Flag");
        this.distanceText = GameObject.Find("DistanceText");
        
        Debug.Log($"Initial position of car: {this.car.transform.position}");
    }
    
    private CarStatus GetCarStatus()
    {
        return car.GetComponent<CarController>().status;
    }

    private void UpdateText(string text)
    {
        // 距離をテキストに反映する
        this.distanceText.GetComponent<TextMeshProUGUI>().text = text;
    }
    
    private string GenerateResultTextFromDistance(float distance)
    {
        string prefix = $"ゴールとの距離 {distance.ToString("F2")} m";
        string result;

        if (distance < 0)
        {
            // 車の x 座標が flag の x 座標を超えてしまうとゲームオーバー
            result = "ゲームオーバー";
        }
        else if (distance < 1.5)
        {
            result = "すごい！";
        }
        else if (distance < 3.6125)
        {
            result = "まずまず";
        }
        else if (distance < 7.25)
        {
            result = "チキン";
        } else if (distance < 12)
        {
            result = "臆病者";
        }
        else
        {
            result = "船降りろ";
        }

        return $"{prefix}\n結果：{result}";
    }
}
