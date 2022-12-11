using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatEscape
{
    public class ArrowGenerator : MonoBehaviour
    {
        // constants
        private const int LeftBoundary = -9;
        private const int RightBoundary = 10;
        
        // members
        public GameObject arrowPrefab;
        public float span = 0.5f; // 矢が生成される間隔（単位：秒）
        public int numArrow = 3; // 同時に生成される矢の数
        private float delta = 0;
        
        void Update()
        {
            this.delta += Time.deltaTime;
            if (this.delta > this.span)
            {
                this.delta = 0;
                this.Generate();
            }
        }

        void Generate()
        {
            for (int i = 0; i < this.numArrow; i++)
            {
                GameObject arrow = Instantiate(arrowPrefab);
                int px = Random.Range(LeftBoundary, RightBoundary);
                arrow.transform.position = new Vector3(px, 7, 0);
            }
        }

        public void SetDifficulty(float span, int numArrow)
        {
            this.span = span;
            this.numArrow = numArrow;
        }
    }
}
