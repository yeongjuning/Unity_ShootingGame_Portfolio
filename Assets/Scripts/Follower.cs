using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public ObjectManager objectManager;

    public float maxShotDelay;
    public float currentShotDelay;

    public Vector3 followPos;
    public int followDelay;
    public Transform parent;
    public Queue<Vector3> parentPos;

    void Awake()
    {
        parentPos = new Queue<Vector3>();
    }

    void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();
    }

    void Watch()
    {
        // Input Position
        if (!parentPos.Contains(parent.position))
            parentPos.Enqueue(parent.position);

        // Output Position
        if (parentPos.Count > followDelay)
            followPos = parentPos.Dequeue();
        else if (parentPos.Count < followDelay)
            followPos = parent.position;
    }

    void Follow()
    {
        transform.position = followPos;
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1"))
            return;

        if (currentShotDelay < maxShotDelay)
            return;

        GameObject bullet = objectManager.MakeObj("BulletFollower");
        bullet.transform.position = transform.position;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        currentShotDelay = 0;
    }

    void Reload()
    {
        currentShotDelay += Time.deltaTime;
    }
}
