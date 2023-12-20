using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary> #tp4 Antoine
/// Classe de base pour les ennemis
/// Code fait et commenté par Jean-Samuel et Antoine
/// </summary>
public class BaseEnnemi : MonoBehaviour
{
    [Header("Propriétés pour Patrouille")]
    [SerializeField] float _delaiPremierDepart = 0.5f;
    [SerializeField] float _delaiDepartsSuivants = 1f;
    float _delaiDepartsSuivantsSauvegarde; // sauvegarde du délai pour le rétablir après modification selon horloge #synthese Antoine
    private int _iPointPatrouille = 0; // index du point de patrouille actuel #synthese Antoine
    private Transform[] _tPointsPatrouille; // liste des points de patrouille #synthese Antoine
    [SerializeField] int _pointageEnnemi = 1; // pointage de l'ennemi #synthese Jean-Samuel

    [Header("Propriétés pour AddForce")]
    [SerializeField] float _vitesseMax;
    private float _vitesseMaxSauvegarde; // vitesse max sauvegardée #synthese Antoine
    [SerializeField] float _force; //force appliqué
    private float _forceSauvegarde; //force du moteur sauvegardé #synthèse Antoine
    [SerializeField][Range(0.1f, 3f)] float _distFreinage; //distance à partir de laquelle on commence à freiner (2)
    [SerializeField] float _toleranceDest = 0.1f;//distance à partir de laquelle on considère que l'ennemi est arrivé à destination (0.1)
    public float toleranceDest { get { return _toleranceDest; } set { _toleranceDest = value; } }   //setter getter #synthese Antoine

    [Header("Propriétés génériques")]
    private Rigidbody2D _rb; // rigidbody de l'ennemi #synthese Antoine
    private Salle _salle; // salle parente #synthese Antoine
    private float _rayonDectection; // rayon de détection #synthese Antoine
    [SerializeField] private LayerMask _playerLayer; // layer du joueur pour détection #synthese Antoine
    private Animator _anim;
    public Animator anim => _anim; // getter du animator #synthese Antoine
    private SpriteRenderer _sr;
    [SerializeField] private AudioClip _sonMort;

    [Header("Drops des ennemis")]
    [SerializeField] private GameObject _butinDeBase;
    [SerializeField, Range(0, 100)] float _chanceButinBase;
    [SerializeField] private GameObject _butinRare;
    [SerializeField, Range(0, 100)] float _chanceButinRare;
    private ButinEnnemi _ButinLancer;
    [SerializeField] private GameObject _bouclier;
    private CapsuleCollider2D[] _tCollidersBouclier; // tableau des colliders du bouclier pour flip x/y #synthèse Antoine
    private int _numEnnemi; // numéro ( 0 ou 1 ) de l'ennemi dans la salle #synthese Antoine
    public int numEnnemi { set { _numEnnemi = value; } } //setter #synthese Antoine


    void Awake()
    {
        _ButinLancer = GetComponent<ButinEnnemi>();
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _salle = GetComponentInParent<Salle>(); // trouve la salle parente #tp4 Antoine
        _rb = GetComponent<Rigidbody2D>();
        _tCollidersBouclier = _bouclier.GetComponents<CapsuleCollider2D>(); // trouve les colliders du bouclier #synthèse Antoine
        
        // s'abonne à l'évenement de l'horloge et sauvegarde les infos de la patrouille #synthèse Antoine
        Niveau.instance.activeHorloge.AddListener(AltererTemps); 
        _delaiDepartsSuivantsSauvegarde = _delaiDepartsSuivants;
        _forceSauvegarde = _force;
        _vitesseMaxSauvegarde = _vitesseMax;
    }
    virtual protected void Start()
    {
        switch (_numEnnemi) // switch pour choisir les points de patrouille #synthèse Antoine
        {
            case 0:
                _tPointsPatrouille = _salle.tPointsPatrouille; // récupère les points de patrouille de la salle parente #synthese Antoine
                break;
            case 1:
                _tPointsPatrouille = _salle.tPointsPatrouille2; // récupère les points de patrouille de la salle parente #synthese Antoine
                break;
        }
        this.transform.position = _tPointsPatrouille[0].position; // place l'ennemi au premier point de patrouille #synthese Antoine
        LancerCoroutine(true);
    }
    /// <summary> #synthese Antoine
    /// Quand le joueur attrape une horloge tous les ennemis sont affecter par le modificateur de temps
    /// Il affecte la vitesse de l'ennemi, le délai entre chaque départ et la force du moteur
    /// </summary>
    private void AltererTemps()
    {
        if (Niveau.instance.modificateurTemps != 1)
        {
            _anim.speed = Niveau.instance.modificateurTemps;
            _delaiDepartsSuivants /= Niveau.instance.modificateurTemps;
            _force *= Mathf.Clamp(Niveau.instance.modificateurTemps, 0.8f, 1.5f); ;
            _vitesseMax *= Mathf.Clamp(Niveau.instance.modificateurTemps, 0.3f, 1.5f); ;
        }
        else
        {
            Debug.Log("retablir temps normal");
            _anim.speed = 1;
            _delaiDepartsSuivants = _delaiDepartsSuivantsSauvegarde;
            _force = _forceSauvegarde;
            _vitesseMax = _vitesseMaxSauvegarde;
        }
    }
    /// <summary> #synthèse Antoine
    /// Permet de démarrer ou d'arreter la coroutine de patrouille
    /// </summary>
    public void LancerCoroutine(bool play)
    {
        if (play)
        {
            StartCoroutine(CoroutinePatrouille());
        }
        else
        {
            StopAllCoroutines();
        }
    }
    /// <summary> #synthese Antoine
    /// systeme de patrouille avec addforce et delai entre chaque depart
    /// </summary>
    IEnumerator CoroutinePatrouille()
    {
        yield return new WaitForSeconds(_delaiPremierDepart);
        while (true)
        {
            Vector2 posDest = ObtenirPosProchaineDestination();
            while (Vector2.Distance(transform.position, posDest) > _toleranceDest)
            {
                yield return new WaitForFixedUpdate();  // attend la prochaine frame physique #synthese Antoine
                AjouteForceVersDestination(posDest);
            }
            yield return new WaitForSeconds(_delaiDepartsSuivants);
        }
    }

    /// <summary> #synthese Antoine
    /// Ajoute une force vers la destination en clamant la velocité pour ne pas dépasser la vitesse max
    /// </summary>
    /// <param name="posDest">repère de destination</param>
    private void AjouteForceVersDestination(Vector2 posDest)
    {
        Vector2 direction = (posDest - (Vector2)transform.position).normalized; // direction vers la destination reduit à 1 #synthese Antoine
        float distance = Vector2.Distance(transform.position, posDest); // distance entre l'ennemi et la destination #synthese Antoine

        float ratioFreinage = (distance > _distFreinage) ? 1 : distance / _distFreinage; // va réduire le ratio de freinage si on est proche de la destination pour clamper la velocité #synthese Antoine

        _rb.AddForce(direction * _force); // ajoute la force vers la destination #synthese Antoine
        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, _vitesseMax * ratioFreinage); // clamp la velocité pour ne pas dépasser la vitesse max #synthese Antoine
    }
    /// <summary> #synthese Antoine
    /// retourne la position du prochain point de patrouille en parcourant le tableau de points de patrouille en ajoutant 1 à l'index
    /// </summary>
    /// <returns>prochaine position</returns>
    private Vector2 ObtenirPosProchaineDestination()
    {
        _iPointPatrouille++;
        if (_iPointPatrouille >= _tPointsPatrouille.Length) _iPointPatrouille = 0;
        Vector2 pos = _tPointsPatrouille[_iPointPatrouille].position;
        return pos;
    }


    /// <summary> #synthese Antoine
    /// Verifie si le perso est dans la zone de détection
    /// <param name="rayonDectection">rayon de dectection de l'ennemi</param>
    /// <param name="decalage">change le centre du cercle de détection</param>
    /// </summary>
    public bool VerifierPerso(float rayonDectection, Vector3 decalage)
    {
        _rayonDectection = rayonDectection;
        // le rayon de détection est un cercle autour de l'ennemi #synthese Antoine 
        Collider2D col = Physics2D.OverlapCircle(transform.position + decalage, rayonDectection, _playerLayer);
        if (col != null) // si le collider n'est pas null, c'est qu'il y a un perso dans la zone de détection #synthese Antoine
        {
            // Debug.Log("Joueur en vu");
            if (GetComponentInParent<EnnemiAC>() != null) GetComponentInParent<EnnemiAC>().estActif = true;
            return true;
        }
        else
        {
            if (GetComponentInParent<EnnemiAC>() != null) GetComponentInParent<EnnemiAC>().estActif = false;
            return false;
        }
    }
    /// <summary>
    /// Verifie si le perso est dans la zone de détection
    /// met par défault le décalage à 0
    /// </summary>
    /// <param name="rayonDectection"></param>
    /// <returns></returns>
    public bool VerifierPerso(float rayonDectection)
    {
        return VerifierPerso(rayonDectection, Vector3.zero);
    }

    /// <summary> #synthèse Antoine
    /// Fait l'animation de l'ennemi en fonction de sa vitesse
    /// </summary>
    public void FaireAnimation()
    {
        _anim.SetFloat("VitesseX", Mathf.Abs(_rb.velocity.x));
        if (_rb.velocity.x > 0)
        {
            _sr.flipX = true;
            if (GetComponentInParent<EnnemiAC>() != null) // si tu trouve le script de Antoine alors applique le changement de bord du bouclier #synthèse Antoine
            {
                _bouclier.GetComponent<SpriteRenderer>().flipX = true;
                _tCollidersBouclier[1].enabled = true; // active le bouclier droit #synthèse Antoine
                _tCollidersBouclier[0].enabled = false; // désactive le bouclier gauche #synthèse Antoine
            }
        }
        else if (_rb.velocity.x < 0)
        {
            _sr.flipX = false;
            if (GetComponentInParent<EnnemiAC>() != null)
            {
                _bouclier.GetComponent<SpriteRenderer>().flipX = false;
                _tCollidersBouclier[0].enabled = true; // active le bouclier gauche #synthèse Antoine
                _tCollidersBouclier[1].enabled = false; // désactive le bouclier droit #synthèse Antoine
            }
        }
    }

    public void Meurt()
    {
        Salle salle = GetComponentInParent<Salle>(); // trouve la salle parente #tp4 Antoine
        // vérifie si la salle contient encore des ennemis pour changer la musique, le délais est pour laissé le temps a l'ennemi de s'autodéruitre #tp4 Antoine
        salle.VerifieSiContientEnnemiAvecDelais();
        // SoundManager.instance.JouerEffetSonore(_sonMort);
        Perso.instance.AugmenterPointageEnnemis(_pointageEnnemi);
        Debug.Log("je" + gameObject.name + " suis mort");
        LaisserUnButin();
        Destroy(gameObject);
    }
    private void LaisserUnButin()
    {
        float chanceDeBase = Random.Range(0, 100);
        if (chanceDeBase <= _chanceButinBase)
        {
            Debug.Log("butin de base");
            float chanceButinRare = Random.Range(0, 100);
            if (chanceButinRare <= _chanceButinRare)
            {
                _ButinLancer.LancerButin(_butinRare);
            }
            else
            {
                _ButinLancer.LancerButin(_butinDeBase);
            }
        }
    }
    /// <summary> #synthese Antoine
    /// Dessine la zone de détection autour des ennemis dans l'éditeur
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, _rayonDectection);
    }
}
