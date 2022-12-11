using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CatEscape
{
    public enum GameStatus
    {
        Ready,
        OnGoing,
        Finished,
    }
    
    public class GameManager : MonoBehaviour
    {
        // constants
        private const string TextReady = "Tap to Start!";
        private const string TextOnGoing = ""; // ゲーム中はステータステキストは表示しない
        private const int SpanUpDifficulty = 20; // 難易度を上げるまでの時間（単位：秒）
        private const float AdditionDifficultySpan = 0.2f;
        private const int AdditionDifficultyArrow = 1;

        // members
        private GameObject player;
        private GameObject arrowGenerator;
        private GameObject gameStatusText;
        private GameObject leftButton;
        private GameObject rightButton;
        public AudioClip audioClip;
        private GameStatus status = GameStatus.Ready;
        private float elapsedTime = 0;

        void Start()
        {
            Debug.Log("Ready to start!");
            
            this.player = GameObject.Find("Player");
            this.arrowGenerator = GameObject.Find("ArrowGenerator");
            this.gameStatusText = GameObject.Find("GameStatusText");
            this.leftButton = GameObject.Find("LButton");
            this.rightButton = GameObject.Find("RButton");
            
            // 静止状態を作る
            Time.timeScale = 0;
            
            // スタート画面ではボタンを非表示にする
            this.leftButton.SetActive(false);
            this.rightButton.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            this.UpdateStatus();
            this.UpdateGameStatusText();
        }

        void StartGame()
        {
            Time.timeScale = 1;
            this.leftButton.SetActive(true);
            this.rightButton.SetActive(true);
        }

        void StopGame()
        {
            // Time.timeScale = 0;
            // ボタンを画面から除去する
            Destroy(leftButton);
            Destroy(rightButton);
        }

        void UpdateStatus()
        {
            switch (this.status)
            {
                case GameStatus.Ready:
                    // タップしてゲームをスタートする
                    if (Input.GetMouseButtonUp(0))
                    {
                        Debug.Log("Start game!");
                        this.status = GameStatus.OnGoing;
                        this.StartGame();
                        this.PlayAudio();
                    }
                    break;
                case GameStatus.OnGoing:
                    // Player が死んだらゲーム終了
                    if (this.player.GetComponent<PlayerController>().GetPlayerStatus() == PlayerStatus.Dead)
                    {
                        Debug.Log("===== GAME OVER! =====");
                        this.status = GameStatus.Finished;
                        this.StopGame();
                    }
                    
                    // 30 秒過ぎたら難易度を上げる
                    // int elapsedTimeInt = (int) this.elapsedTime;
                    // if (elapsedTimeInt != 0 && elapsedTimeInt % SpanUpDifficulty == 0)
                    // {
                    //     // level up
                    //     this.GoToNextDifficulty();
                    // }
                    
                    // 生きていたらスコアを加算
                    this.Score();
                    
                    break;
                case GameStatus.Finished:
                    if (Input.GetMouseButtonUp(0))
                    {
                        Debug.Log("Restart game!");
                        this.PlayAudio();
                        StartCoroutine(this.DelaySceneLoad());
                    }
                    break;
                default:
                    break;
            }
        }

        private void UpdateGameStatusText()
        {
            var tm = this.gameStatusText.GetComponent<TextMeshProUGUI>();
            
            switch (this.status)
            {
                case GameStatus.Ready:
                    tm.text = TextReady;
                    break;
                case GameStatus.OnGoing:
                    tm.text = TextOnGoing;
                    break;
                case GameStatus.Finished:
                    // スコア算出
                    string score = ((int)this.elapsedTime * 10).ToString();
            
                    // テキスト更新 
                    string text = $"GAME OVER\nSCORE: {score}\nTap to Restart!";
                    tm.text = text;
                    break;
                default:
                    break;
            }
        }
        
        IEnumerator DelayResetGameStatusText(float delay)
        {
            var tm = this.gameStatusText.GetComponent<TextMeshProUGUI>();
            tm.text = "LEVEL UP!!";
            yield return new WaitForSeconds(delay);
            tm.text = "";
        }

        private void UpdateGameStatusTextTemporarily()
        {
            StartCoroutine(DelayResetGameStatusText(1f));
        }

        private void GoToNextDifficulty()
        {
            Debug.Log("Let's go to next level!");
            
            // ArrowGenerator の難易度に関わるパラメータを更新
            var ag = this.arrowGenerator.GetComponent<ArrowGenerator>();
            ag.SetDifficulty(ag.span + AdditionDifficultySpan, ag.numArrow + AdditionDifficultyArrow);
            
            // テキストを一時的に表示する
            this.UpdateGameStatusTextTemporarily();
        }

        private void PlayAudio()
        {
            GetComponent<AudioSource>().PlayOneShot(this.audioClip);
        }
        
        IEnumerator DelaySceneLoad()
        {
            yield return new WaitForSeconds(0.3f);
            Debug.Log("Reload scene...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void Score()
        {
            elapsedTime += Time.deltaTime;
        }
    }
}
