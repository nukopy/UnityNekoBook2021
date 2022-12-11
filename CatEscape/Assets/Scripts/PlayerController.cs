using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatEscape
{
    public enum PlayerStatus
    {
        Alive,
        Dead
    }

    public class Player
    {
        // constants
        private const int MaxHp = 100;
        private const int InitHp = 100;
        private const PlayerStatus InitPlayerStatus = PlayerStatus.Alive;

        // members
        public int Hp { get; set; }
        public PlayerStatus Status { get; set; }
        
        public Player(int hp = InitHp, PlayerStatus status = InitPlayerStatus)
        {
            this.Hp = hp;
            this.Status = status;
        }
        
        /// <summary>
        /// プレイヤーがダメージを受けるメソッド
        /// 他のゲームオブジェクトからダメージを受けるときに呼ばれる
        /// </summary>
        /// <param name="damage"></param>
        public void Damaged(int damage)
        {
            int prev = this.Hp;
            this.Hp -= damage;

            Debug.Log($"Player damaged: prev {prev} -> new {this.Hp}");
        }

        public void UpdateStatus()
        {
            if (this.Hp <= 0)
            {
                this.Status = PlayerStatus.Dead;
            }
        }

        public float GetHpRatio()
        {
            return (float)this.Hp / (float)MaxHp;
        }
    }
    
    public enum Orientation
    {
        Left,
        Right,
    }

    public class PlayerController : MonoBehaviour, ICollidable
    {
        // constants
        private const float LeftBoundary = -9.00f;
        private const float RightBoundary = 9.00f; 
        private const int ThresholdHpAudioDamaged = 20;
        
        // members
        private Player player;
        private float MoveLength = 1.0f;
        private float Radius = 1.0f;
        
        // members related to effect
        public Sprite ExplosionSprite;
        public AudioSource audioSourceMove;
        public AudioSource audioSourceDamaged;
        public AudioSource audioSourceDead;
        public AudioClip audioClipDamaged;
        public AudioClip audioClipDead;
        public AudioClip audioClipMove;

        void Start()
        {
            this.player = new Player();
        }
        
        void Update()
        {
            // Player が死んでいる場合、操作を受け付けない
            if (this.player.Status == PlayerStatus.Dead) return;
            
            // 左矢印
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                this.MoveLeft();
            }
        
            // 右矢印
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                this.MoveRight();
            }
            
            // 生死の確認
            this.player.UpdateStatus();
            if (this.player.Status == PlayerStatus.Dead)
            {
                // 死んだことを検知したらスプライトを更新する
                this.UpdatePlayerSprite();
            }
        }

        private void Move(Orientation orientation)
        {
            Vector3 pos = this.gameObject.transform.position;
            
            switch (orientation)
            {
                case Orientation.Left:
                    // 境界チェック
                    if (pos.x - MoveLength < LeftBoundary)
                    {
                        Debug.Log($"Player cannot move anymore on the left boundary: {pos}");
                        return;
                    }
                    
                    // move
                    this.transform.Translate(-MoveLength, 0, 0);
                    Debug.Log($"Moved Left: {pos}");
                    break;
                case Orientation.Right:
                    // 境界チェック
                    if (pos.x + MoveLength > RightBoundary)
                    {
                        Debug.Log($"Player cannot move anymore on the right boundary: {pos}");
                        return;
                    }
                    
                    // move
                    this.transform.Translate(+MoveLength, 0, 0);
                    Debug.Log($"Move Right: {pos}");
                    break;
                default:
                    throw new Exception("Default case should not occur");
            }
        }

        public void MoveLeft()
        {
            this.Move(Orientation.Left);
            this.PlayAudioOnMove();
        }

        public void MoveRight()
        {
            this.Move(Orientation.Right);
            this.PlayAudioOnMove();
        }
        
        public float GetRadius()
        {
            return this.Radius;
        }

        public bool IsCollisionWith(GameObject go)
        {
            // FIXME: 良くないけど、今回の実装では ArrowController で Arrow と Player の衝突判定を行っている。
            // 良さげな実装としては、CollisionDetector を置いて Detect(ICollidable Arrow, ICollidable Player) みたいな感じかな。わからん。
            throw new NotImplementedException();
        }

        public void OnCollision(CollisionEvent e)
        {
            Debug.Log($"Collision of \"Player\" with \"{e.collidedBy}\" occurred.");
            this.player.Damaged(e.damage);
            this.PlayAudioOnCollision();
        }

        public void PlayAudioOnCollision()
        {
            if (ThresholdHpAudioDamaged <= this.player.Hp)
            {
                // HP が 20 以上のときはダメージ音
                this.audioSourceDamaged.clip = audioClipDamaged;
                this.audioSourceDamaged.Play();
            } else if (this.player.Hp == 0)
            {
                // HP が 0 になったときは爆発音
                this.audioSourceDead.clip = audioClipDead;
                this.audioSourceDead.Play();
            }
        }

        private void PlayAudioOnMove()
        {
            // 移動音
            this.audioSourceMove.clip = audioClipMove;
            this.audioSourceMove.Play();
        }

        public float GetPlayerHpRatio()
        {
            return this.player.GetHpRatio();
        }

        public PlayerStatus GetPlayerStatus()
        {
            return this.player.Status;
        }
        
        void UpdatePlayerSprite()
        {
            SpriteRenderer playerSr = gameObject.GetComponent<SpriteRenderer>();
            playerSr.sprite = this.ExplosionSprite;
        }
    }
}
