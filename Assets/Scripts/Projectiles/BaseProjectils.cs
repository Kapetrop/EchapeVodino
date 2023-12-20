using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Classe de base pour les projectiles
/// Code et commentaires fait par Jean-Samuel
/// #synthese Jean-Samuel
/// </summary>
public class BaseProjectils : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    float _angleSupplementaire = 90; // L'angle initial du projectile
    public float angleSupplementaire => _angleSupplementaire;
    private Rigidbody2D _rb;

    private float _vitesse = 10f; // La vitesse à laquelle le projectile se déplace

    private Vector2 _target; // La position de la cible
    public Vector2 target
    {
        get => _target;
        set
        {
            _target = value;
        }
    }

    void Start()
    {
        Debug.Log(_target);
        _rb = GetComponent<Rigidbody2D>();
     


        // Calculer l'angle en utilisant la méthode TrouverAngle
        float angle = TrouverAngle(transform.position, _target) * Mathf.Rad2Deg;
        // Ajouter ou soustraire 90 degrés à l'angle initial
        Debug.Log(angle);
        angle -= _angleSupplementaire;
        Debug.Log(_angleSupplementaire);

        // Appliquer la rotation
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Update()
    {
        // Calculer l'angle à chaque mise à jour en utilisant la méthode TrouverAngle
        float angle = Mathf.Atan2(_target.y, _target.x) * Mathf.Rad2Deg;
        angle -= _angleSupplementaire;
        // Appliquer la rotation
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Calculer le vecteur de destination
        Vector2 destination = (Vector2)transform.position + _target.normalized * 10f;

        // Déplacer le projectile vers sa destination à une vitesse donnée
        transform.position = Vector2.MoveTowards(transform.position, destination, _vitesse * Time.deltaTime);

        // Si le projectile est proche de sa cible, détruire l'objet
        if (Vector2.Distance(transform.position, _target) < 0.1f) DetruireProjectile();
    
    }
    // Détruire l'objet
    public void DetruireProjectile()
    {
        Destroy(gameObject);
    }
    // Trouver l'angle entre deux vecteurs
    public float TrouverAngle(Vector2 v1, Vector2 v2)
    {
        Vector2 v = v2 - v1;
        return Mathf.Atan2(v.y, v.x);
    }
    
    virtual protected void OnCollisionEnter2D(Collision2D collision)
    {
        // Si le projectile touche un objet avec un layer inclus dans le layermask _layerMask, détruire le projectile
        if (_layerMask == (_layerMask | (1 << collision.gameObject.layer)))
        {
            Debug.Log("Murs touché");
            DetruireProjectile();
        }
    }
}