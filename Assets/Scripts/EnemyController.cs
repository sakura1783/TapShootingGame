using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class EnemyController : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        transform.Translate(0, -0.01f, 0);

        if (transform.localPosition.y < -1300)
        {
            Destroy(gameObject);
        }
    }
}
