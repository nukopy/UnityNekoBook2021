using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RouletteTextController : MonoBehaviour
{
    private string rouletteObjName = "Roulette";
    private GameObject roulette;

    // Start is called before the first frame update
    void Start()
    {
        this.GetRoulette();
    }

    // Update is called once per frame
    void Update()
    {
        // update text
        string text;
        Vector3 rotation;
        switch (this.GetRouletteStatus())
        {
            case Status.Ready:
                // 初期値は "おみくじ"
                text = "おみくじ";
                this.UpdateText(text);
                break;
            case Status.Rotating:
                rotation = this.GetRouletteRotation();
                text = this.GetOmikujiText(rotation);
                this.UpdateText(text);
                break;
            case Status.Stopping:
                text = "判定中...";
                this.UpdateText(text);
                break;
            case Status.Finished:
                rotation = this.GetRouletteRotation();
                text = this.GetOmikujiText(rotation);
                this.UpdateText(text);
                break;
            default:
                throw new Exception("ここにはこないよーーー！");
        }
    }

    private void GetRoulette()
    {
        this.roulette = GameObject.Find(this.rouletteObjName);
        if (this.roulette)
        {
            Debug.Log($"Got GameObject \"{this.roulette.name}\"");
        }
        else
        {
            throw new NullReferenceException("GameObject \"Roulette\" must be found.");
        }
    }

    /// <summary>
    /// ルーレットが回っているかどうかの状態を取得するメソッド
    /// </summary>
    private Status GetRouletteStatus()
    {
        if (!this.roulette) return Status.NotReady;
        
        var targetScript = this.roulette.GetComponent<RouletteController>();
        return targetScript.status;
    }

    private Vector3 GetRouletteRotation()
    {
        if (!this.roulette) throw new Exception("GameObject \"Roulette\" not found.");

        Transform transform = this.roulette.GetComponent<Transform>();
        Quaternion quaternion = transform.rotation;
        Vector3 eulerAngles = quaternion.eulerAngles; 

        return eulerAngles;
    }

    private string GetOmikujiText(Vector3 rotation)
    {
        // return "Rotation: " + rotation.ToString();
        switch (rotation.z)
        {
            case (>= 330 and < 360) or (>= 0 and < 30):
                return "凶";
            case >= 30 and < 90:
                return "大吉";
            case >= 90 and < 150:
                return "大凶";
            case >= 150 and < 210:
                return "小吉";
            case >= 210 and < 270:
                return "末吉";
            case >= 270 and < 330:
                return "中吉";
            default:
                // ここには到達しないはず
                return "？？？";
        }
    }

    private void UpdateText(string text)
    {
        var textMesh = this.GetComponent<TextMeshProUGUI>();
        textMesh.text = text;
    }
}
