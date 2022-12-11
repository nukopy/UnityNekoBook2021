using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CatEscape
{
    public class Arrow
    {
        // constants
        private const int InitDamage = 20;

        // members
        public int Damage { get; set; }

        public Arrow(int damage = InitDamage)
        {
            this.Damage = damage;
        }
    }
    
    public class ArrowController : MonoBehaviour, ICollidable
    {
        // constants
        private const float SpeedArrowDown = -0.2f;
        private const float ThresholdYaxisReachedBottom = -5.0f;

        // members
        private Arrow arrow; 
        private GameObject player;
        private float Radius = 0.5f;

        void Start()
        {
            this.arrow = new Arrow();
            this.player = GameObject.Find("Player");
        }
        
        void Update()
        {
            MoveDown();

            // 矢がゲーム画面外に出た判定
            if (IsReachedBottom()) this.OnReachedBottom();

            // 矢が Player に当たった判定
            if (IsCollisionWith(this.player))
            {
                CollisionEvent e = this.GenerateArrowDamageEvent();
                this.OnCollision(e);
            }
        }

        private void MoveDown()
        {
            this.transform.Translate(0, SpeedArrowDown, 0);
        }

        private bool IsReachedBottom()
        {
            return this.transform.position.y < ThresholdYaxisReachedBottom;
        }

        private void OnReachedBottom()
        {
            // 矢のゲームオブジェクトを破棄する
            Destroy(gameObject);
        }

        public float GetRadius()
        {
            return this.Radius;
        }

        /// <summary>
        /// 矢の当たり判定を行うメソッド
        /// 2 つのゲームオブジェクトの形状を円形に近似し、円の中心間の距離が 2 つの円の半径以下の場合、当たり判定とする
        /// </summary>
        /// <returns></returns>
        public bool IsCollisionWith(GameObject go)
        {
            // ICollidable を実装していなかったらエラー
            ICollidable collidableGo = go.gameObject.GetComponent<ICollidable>();
            if (collidableGo == null) throw new Exception("Player must implement ICollidable");

            // 衝突計算
            Vector2 pGo = go.transform.position;
            Vector2 pArrow = this.transform.position;
            Vector2 vec = pGo - pArrow;
            float distance = vec.magnitude;

            return distance < this.GetRadius() + collidableGo.GetRadius();
        }

        /// <summary>
        /// プレイヤーとの当たり判定時に実行するメソッド
        /// </summary>
        /// <returns></returns>
        public void OnCollision(CollisionEvent e)
        {
            // ICollidable を実装していなかったらエラー
            ICollidable collidablePlayer = this.player.gameObject.GetComponent<ICollidable>();
            if (collidablePlayer == null) throw new Exception("Player must implement ICollidable");

            // 当たった相手の OnCollision を invoke
            // FIXME: 本当は CollisionDetector を置いて、そこから collidable なオブジェクトにイベントを送信すべき。現段階だとうまい実装方法がわからなかったのでこうやってる。
            collidablePlayer.OnCollision(this.GeneratePlayerDamageEvent());
            
            // 矢のゲームオブジェクトは当たり判定時に破棄する
            Destroy(gameObject);
        }

        public CollisionEvent GenerateArrowDamageEvent()
        {
            return new("player", damage: 0);
        }
        
        public CollisionEvent GeneratePlayerDamageEvent()
        {
            return new("arrow", this.arrow.Damage);
        }
    }
}
