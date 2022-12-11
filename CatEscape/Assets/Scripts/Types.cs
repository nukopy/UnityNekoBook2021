using System;
using UnityEngine;

namespace CatEscape
{
    public struct CollisionEvent
    {
        public string collidedBy;
        public int damage;

        public CollisionEvent(string collidedBy, int damage)
        {
            this.collidedBy = collidedBy;
            this.damage = damage;
        }
    }

    public interface ICollidable
    {
        public float GetRadius();
        public bool IsCollisionWith(GameObject go);
        public void OnCollision(CollisionEvent e) {}
    }
}
