using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouger : MonoBehaviour
{

    [SerializeField]
    private float _acceleration = 0.005f;
    [SerializeField]
    private float _maxSpeed = 0.1f;
    [SerializeField]
    private float _friction = 0.001f;

    private float _vx = 0f;
    private float _vy = 0f;
    private bool _gauche = false;
    private bool _droite = false;
    private bool _haut = false;
    private bool _bas = false;
    private SpriteRenderer _sp;

    void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Touches du clavier
        _gauche = Input.GetKey("left") || Input.GetKey("a");
        _droite = Input.GetKey("right") || Input.GetKey("d");
        _haut = Input.GetKey("up") || Input.GetKey("w");
        _bas = Input.GetKey("down") || Input.GetKey("s");

        _sp.flipX = _vx < 0f;
    }

    private void FixedUpdate()
    {
        // Directions (touches du clavier)
        if (_gauche) _vx -= _acceleration;
        if (_droite) _vx += _acceleration;
        if (_haut) _vy += _acceleration;
        if (_bas) _vy -= _acceleration;

        // Friction
        if (_vx > _friction) _vx -= _friction;
        else if (_vx < -_friction) _vx += _friction;
        else _vx = 0f;

        if (_vy > _friction) _vy -= _friction;
        else if (_vy < -_friction) _vy += _friction;
        else _vy = 0f;

        // Limite de vitesse
        if (_vx > _maxSpeed) _vx = _maxSpeed;
        if (_vx < -_maxSpeed) _vx = -_maxSpeed;
        if (_vy > _maxSpeed) _vy = _maxSpeed;
        if (_vy < -_maxSpeed) _vy = -_maxSpeed;

        // Déplacement
        transform.position += new Vector3(_vx, _vy, 0f);
    }
}
