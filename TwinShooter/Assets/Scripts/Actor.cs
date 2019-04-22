using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Actor : MonoBehaviour
{
    public enum ActorType
    {
        Player,
        Enemy
    }
    [Header("Actor Para")]
    public bool bDead;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float currentHp;
    [SerializeField] protected ActorType myType;

    //public Actor(int _maxHp, ActorType _type) 
    //{
    //    maxHp = _maxHp;
    //    currentHp = _maxHp;
    //    myType = _type;
    //}

    public virtual void GetDamage(float _dmg)
    {
        if (!bDead)
        {
            currentHp -= _dmg;
            if (currentHp <= 0)
            {
                currentHp = 0;
                Die();
            }
        }

    }

    public virtual void Die()
    {
        bDead = true;
    }
}
