using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    public ObjectManager objectManager;
    public void PlayerPowerOne(GameObject bulletObj_A, Vector3 playerPos)
    {
        GameObject bullet = objectManager.MakeObj("BulletPlayerA");
        bullet.transform.position = playerPos;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
    }

    public void PlayerPowerTwo(GameObject bulletObj_A, Vector3 playerPos)
    {
        GameObject bulletR = objectManager.MakeObj("BulletPlayerA");
        bulletR.transform.position = playerPos + Vector3.right * 0.1f;

        GameObject bulletL = objectManager.MakeObj("BulletPlayerA");
        bulletL.transform.position = playerPos + Vector3.left * 0.1f;

        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

        rigidR.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        rigidL.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
    }

    public void PlayerPowerThree(GameObject bulletObj_A, GameObject bulletObj_B, Vector3 playerPos)
    {
        GameObject bulletRR = objectManager.MakeObj("BulletPlayerA");
        bulletRR.transform.position = playerPos + Vector3.right * 0.35f;

        GameObject bulletCC = objectManager.MakeObj("BulletPlayerB");
        bulletCC.transform.position = playerPos;

        GameObject bulletLL = objectManager.MakeObj("BulletPlayerA");
        bulletLL.transform.position = playerPos + Vector3.left * 0.35f;

        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

        rigidRR.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        rigidCC.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
    }
}
