using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary> 
/// Ennemi AC  #synthese Antoine
/// </summary>
public class EnnemiAC : BaseEnnemi
{
    [Header("Paramètres esthétique")]
    [SerializeField] private AudioClip _effetDetection;
    [SerializeField] private AudioClip _effetBlesse;
    [SerializeField] private AudioClip _effetAttaque;
    [SerializeField] private AudioClip _effetMort;
    private const int _GROSSEUR_MAX = 3; // grosseur maximal ABSOLUT #synthèse Antoine

    private const int _GROSSEUR_MIN = 2; // grosseur minimal ABSOLUT #synthèse Antoine


    [Header("Paramètres techniques")]
    [SerializeField] private float _rayonDetectionAC; // rayon de détection #synthese Antoine
    [SerializeField] private int _nbVie; // nombre de vie de l'ennemi #synthèse Antoine
    private Coroutine coroutGrossir; // coroutine qui fait grossir et rétrécir l'ennemi #synthèse Antoine
    private float _grosseurMinimalJeu = 1f; // grosseur minimal de l'ennemi #synthèse Antoine
    private float _grosseurMaximalJeu = 2f; // grosseur maximal de l'ennemi #synthèse Antoine
    private Transform _transformBouclier; // transform du bouclier #synthèse Antoine
    private bool _estActif = false; // Si le joueur est en vue, alors l'énnemis sera actif #synthèse Antoine
    public bool estActif
    {
        get { return estActif; }
        set
        {
            // return si la variable ne change pas : permet de savoir quand la variable flip #synthèse Antoine
            if (value == _estActif) return;
            _estActif = value;
            if (_estActif)
            {
                Debug.Log("je rentre dans le rayon de vu");
                base.LancerCoroutine(false);
                // SoundManager.instance.JouerEffetSonore(_effetDetection, 1, true);
                gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                coroutGrossir = StartCoroutine(CoroutineGrossi(true));
            }
            else
            {
                Debug.Log("je sors du rayon de vu");
                base.LancerCoroutine(true);
                StopCoroutine(coroutGrossir);
                coroutGrossir = StartCoroutine(CoroutineGrossi(false));
                gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
    protected override void Start()
    {
        base.Start();
        _transformBouclier = transform.GetChild(0);
    }

    /// <summary> #synthese Antoine
    /// systeme de patrouille avec addforce et delai entre chaque depart
    /// </summary>
    IEnumerator CoroutineGrossi(bool doitGrossir)
    {
        // augmente la grosseur maximal de l'ennemi ainsi que son rayon de dection à chaque départ #synthèse Antoine
        if (_grosseurMaximalJeu < _GROSSEUR_MAX) _grosseurMaximalJeu += 0.1f; //safety pour pas qu'il grossisse a l'infinie #synthèse Antoine
        if (_grosseurMinimalJeu < _GROSSEUR_MIN) _grosseurMinimalJeu += 0.02f;
        _rayonDetectionAC += 0.1f; // augmente son rayon de détection #synthèse Antoine
        base.toleranceDest += 0.1f; // augmente sa tolérance de destination #synthèse Antoine

        if (doitGrossir)
        {
            while (_transformBouclier.localScale.x < _grosseurMaximalJeu)  // si l'ennemi est plus grand que 2 fois ca taille arrete de grossir #synthese Antoine
            {
                _transformBouclier.localScale += Vector3.one / 100;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            while (_transformBouclier.localScale.x > _grosseurMinimalJeu)  // tant que ca taille n'est pas rétablie la normal rétrécis #synthèse Antoine
            {
                _transformBouclier.localScale -= Vector3.one / 110;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    /// <summary> #synthèse Antoine
    ///  Détecte si le joueur est dans la zone de détection
    /// </summary>
    private void FixedUpdate()
    {
        base.VerifierPerso(_rayonDetectionAC);
        base.FaireAnimation();
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        // si l'ennemi touche le joueur il lui fait perdre une vie #synthèse Antoine
        Perso perso = other.gameObject.GetComponent<Perso>();
        if (perso != null)
        {
            if (perso.transform.position.y > _transformBouclier.position.y) // si le perso est plus haut que l'ennemi il le heurte #synthèse Antoine
            {
                Heurter();
                base.anim.SetTrigger("heurte");
            }
            else
            {
                perso.PerdreVie();
            }
        }
    }

    /// <summary> # synthèse Antoine
    /// Permet de blesser
    /// </summary>
    private void Heurter()
    {
        _nbVie--;
        if (_nbVie <= 0)
        {
            base.Meurt();
        }
    }
}
