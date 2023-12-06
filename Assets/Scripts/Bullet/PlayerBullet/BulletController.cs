using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 direction = Vector3.zero;
    int damage;

    public enum Targets
    {
        ENEMY,
        PLAYER
    }

    public Targets targets;

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetDamage(int amount)
    {
        damage = amount;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (targets == Targets.PLAYER)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                col.GetComponent<ShipController>().TakeDamage(damage);

                Destroy(gameObject);
            }
        }

        if (targets == Targets.ENEMY)
        {
            if (col.gameObject.CompareTag("EnemyShip"))
            {
                col.GetComponent<EnemyMoving>().TakeDamage(damage);
            }
        }      

    }

}
