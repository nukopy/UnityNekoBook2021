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

        // members
        private GameObject player;
        private GameObject gameStatusText;
        private GameObject leftButton;
        private GameObject rightButton;
        public AudioClip audioClip;
        private GameStatus status = GameStatus.Ready;
        private float elapsedTime = 0;

        void Start()
        {
            Debug.Log("Ready to start!");
            Time.timeScale = 0;
            
            this.player = GameObject.Find("Player");
            this.gameStatusText = GameObject.Find("GameStatusText");
            this.leftButton = GameObject.Find("LButton");
            this.rightButton = GameObject.Find("RButton");
        }

        // Update is called once per frame
        void Update()
        {
            this.UpdateStatus();
            this.UpdateGameStatusText();
            elapsedTime += Time.deltaTime;
        }

        void StartGame()
        {
            Time.timeScale = 1;
        }

        void StopGame()
        {
            Time.timeScale = 0;
            
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
                    break;
                case GameStatus.Finished:
                    if (Input.GetMouseButtonUp(0))
                    {
                        Debug.Log("Restart game!");
                        // this.PlayAudio();
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                    break;
                default:
                    break;
            }
        }

        private void UpdateGameStatusText()
        {
            switch (this.status)
            {
                case GameStatus.Ready:
                    this.gameStatusText.GetComponent<TextMeshProUGUI>().text = TextReady;
                    break;
                case GameStatus.OnGoing:
                    this.gameStatusText.GetComponent<TextMeshProUGUI>().text = TextOnGoing;
                    break;
                case GameStatus.Finished:
                    string score = ((int)this.elapsedTime * 10).ToString();
                    string text = $"GAME OVER\nSCORE: {score}\nTap to Restart!"; 
                    this.gameStatusText.GetComponent<TextMeshProUGUI>().text = text;
                    break;
                default:
                    break;
            }
        }

        private void PlayAudio()
        {
            GetComponent<AudioSource>().PlayOneShot(this.audioClip);
        }
    }
}
