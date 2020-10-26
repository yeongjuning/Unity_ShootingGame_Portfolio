using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        // 폭발 오브젝트 스스로 활성화
        Invoke("Disable", 2f);
    }

    void Disable()
    {
        // 폭발 오브젝트 스스로 비활성화
        gameObject.SetActive(false);
    }

    public void StartExplosion(string target)
    {
        anim.SetTrigger("OnExplosion");

        switch (target)
        {
            case "S":       // 작은 적 비행기
                transform.localScale = Vector3.one * 0.7f;
                break;
            case "M":       // 중간 적 비행기
            case "P":       // 플레이어 비행기
                transform.localScale = Vector3.one * 1f;
                break;
            case "L":       // 큰 적 비행기
                transform.localScale = Vector3.one * 2f;
                break;
            case "B":       // 보스 비행기
                transform.localScale = Vector3.one * 3f;
                break;
        }
    }
}
