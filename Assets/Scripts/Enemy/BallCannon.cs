using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCannon : MonoBehaviour
{
    private PlayerController _player;
    private float _sec = 10f;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        transform.Translate(Vector2.left * 8f * Time.deltaTime);

        _sec -= Time.deltaTime;

        if (_sec < 0)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);

            if (gameObject.transform.position.x < collision.transform.position.x)
                _player.Damage("Right", "CannonBall");
            else if (gameObject.transform.position.x > collision.transform.position.x)
                _player.Damage("Left", "CannonBall");
        }
    }
}
