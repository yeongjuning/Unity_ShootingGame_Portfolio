using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool isRotate;   // 총알이 스스로 회전하는지에 대한 플래그 변수

    void Update()
    {
        if (isRotate)
            transform.Rotate(Vector3.forward * 10f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
            gameObject.SetActive(false);
    }
}
