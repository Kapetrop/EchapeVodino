using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui controle les déplacements et les sauts du personnage
/// Auteurs du code: Antoine Chartier et Jean-Samuel David
/// Auteur des commentaires: Jean-Samuel David & Antoine #tp3
/// </summary>
public class Perso : BasePerso
{
    static private Perso _instance; // instance de la classe
    static public Perso instance => _instance; // getter
    [Header("Données du perso")]
    [SerializeField] SOPerso _donnees; // lien vers SO du perso #tp3 Antoine
    public SOPerso donnees => _donnees; // getter _donnes perso #tp4 Antoine
    [SerializeField] SONavigation _donneesNav; // lien vers SO navigation #tp4 Antoine
    public SONavigation donneesNav => _donneesNav; //getter donneesNav #synthese Antoine
    [SerializeField] AudioClip _sonSaut; // effet sonore du saut #tp4 Antoine
    [SerializeField] AudioClip _sonChute; // effet sonore de la chute #tp4 Antoine
    private Porte _porte; // Porte #tp4 Antoine
    public Porte LaPorte { get => _porte; set => _porte = value; } //getter/setter porte  #tp4 Antoine
    private bool _touchePorte = false; // si il est sur la porte = true #tp3 Antoine
    public bool touchePorte { set => _touchePorte = value; } //setter _touchePorte #synthèse Antoine
    

    [Header("Paramètres saut")]
    [SerializeField] private float _vitesse;
    [SerializeField] private float _forceSaut;

    [SerializeField] private int _nbFramesMax; // Nombres de frames disponible pour appliquer une force
    private int _nbFramesRestants = 0; // Nombre de frames restante pour appliquer une force
    private Rigidbody2D _rb;
    public Rigidbody2D rb { get => _rb; set => _rb = value; } // setter du rb pour l'arreter #synthèse Antoine
    private SpriteRenderer _sr;
    private float _axeX; // Direction que le personnage se dirige
    private bool _veutSauter = false;

    //#tp4 JEan-Samuel
    private Animator _anim; // lien vers l'animateur
    [SerializeField] GestParticules _gestParticules; // lien vers le script GestParticules

    [SerializeField] private GameObject _projectile; // #synthese Jean-Samuel
    private bool _veutTirer = false; // #synthese Jean-Samuel
    private bool _peutTirer = true; // #synthese Jean-Samuel
    private bool _coroutineAttaqueEnCour = false; // #synthese Jean-Samuel
    private bool _etaitDansLesAir; // #synthese Jean-Samuel
    private int _lesCiblesTouchees = 0; // #synthese Jean-Samuel

    public int lesCiblesTouchees
    {
        get => _lesCiblesTouchees;
        set => _lesCiblesTouchees = value;
    } // #synthese Jean-Samuel


    private void Awake()
    {
        if (_instance != null) { Destroy(gameObject); return; } // Singleton
        _instance = this; // Singleton
        _donnees.InitialiserPartiel(); // Initialise les données du perso necessaire
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); // Récupère le rigidbody
        _sr = GetComponent<SpriteRenderer>(); // Récupère le sprite renderer
        _anim = GetComponent<Animator>(); // Récupère l'animateur #tp4 Jean-Samuel
    }
    /// <summary>
    /// Ajoute un brocoli dans le SO des données Perso # tp3 Antoine
    /// est appellé par le script brocolis
    /// </summary>
    public void AjouterBroco(int valeurBrocoli)
    {
        _donnees.nbDeBrocolis += valeurBrocoli;
    }

    /// <summary>
    /// recueille l'input du joueur gauche/droite, fait fliper le perso en conséquence
    /// recueille l'input du joueur de la spaceBar
    /// </summary>
    void Update()
    {
        _axeX = Input.GetAxis("Horizontal");
        _anim.SetFloat("VitesseX", Mathf.Abs(_axeX));
        if (_axeX < 0) _sr.flipX = true;
        else if (_axeX > 0) _sr.flipX = false;

        _veutSauter = Input.GetButton("Jump"); // Le personnage saute lorsque l'usager appuis sur le bouton "jump"
        if (_veutSauter && _estAuSol && _touchePorte) // si le perso Jump & est au sol & touche la porte #synthese Antoine
        {
            _porte.EssaiPorte(); // appel de la méthode EssaiPorte() #synthese Antoine
        }
        _veutTirer = Input.GetButton("Fire1"); // #synthese Jean-Samuel
        if (_veutTirer && !_coroutineAttaqueEnCour)
        {
            StartCoroutine(CoroutineAttaque());
            _coroutineAttaqueEnCour = true;
        } // #synthese Jean-Samuel
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// Appel de la méthode de la classe mère
    /// et utilise sa propre méthode en même temps
    /// </summary>
    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        _anim.SetBool("EstAuSol", _estAuSol);//#tp4 Jean-Samuel
        if (_estAuSol)
        {

            if (_veutSauter)
            {
                if (!_etaitDansLesAir && !_estAuSol) SoundManager.instance.JouerEffetSonorePerso(_sonSaut); //#tp4 Antoine
                Sauter();
            }
            else
            {
                _nbFramesRestants = _nbFramesMax;
            }
        }
        else
        {
            bool peutSauterPlus = (_nbFramesRestants > 0);
            if (_veutSauter && peutSauterPlus)
            {
                Sauter();
            }
            else _nbFramesRestants = 0;
        }

        _rb.velocity = new Vector2(_axeX * _vitesse, _rb.velocity.y); // la vélocité c'est un changement de force constant, il est modifié par le la valeur de axeX pour faire déplacer le perso 
        if ((Mathf.Abs(_rb.velocity.x) > 0.1f) && _estAuSol) _gestParticules.AfficherParticulesMarche();
        _anim.SetFloat("VitesseY", _rb.velocity.y);

        if (_rb.velocity.y > 0)
        {
            _rb.gravityScale = 1.5f;
        }
        else if (_rb.velocity.y < 0)
        {
            _rb.gravityScale = 3.5f;
            // _anim.SetTrigger("VersLeSol");//#tp4 Jean-Samuel
            if (!_estAuSol) _gestParticules.AfficherParticulesChute();

        }
        if (_etaitDansLesAir && _estAuSol) _gestParticules.EnclancherParticuleAtterrissage(); //#synthese Jean-Samuel
        _etaitDansLesAir = _estAuSol;
        //Variable etaitDansLesAir + simplifier em particules
    }
    public void PerdreVie()
    {
        _donnees.nbDeVie -= 1;
    }
    /// <summary>
    /// Méthode pour faire sauter le personnage
    /// </summary>
    void Sauter()
    {
        float fractionDeForce = (float)_nbFramesRestants / _nbFramesMax;
        Vector2 vecteurDeForce = Vector2.up * _forceSaut * fractionDeForce;
        _rb.AddForce(vecteurDeForce);
        if (_nbFramesRestants > 0) _nbFramesRestants--;
        // _anim.SetTrigger("Sauter");
    }
    /// <summary> #tp3 Antoine
    /// augmente la vitesse de déplacement depuis le script Bonus
    /// </summary>
    public void MarchePlusVite(float modificateurVitesse, Color couleurParticule, float grosseurParticule)
    {
        Coroutine Corout = StartCoroutine(CoroutineSuperVitesse(modificateurVitesse, couleurParticule, grosseurParticule));
    }
    /// <summary> #tp3 Antoine
    /// augmente la force du saut depuis le script Bonus
    /// </summary>
    public void SautePlusHaut(float modificateurSaut, Color couleurParticule, float grosseurParticule)
    {
        Coroutine corout = StartCoroutine(CoroutineSuperSaut(modificateurSaut, couleurParticule, grosseurParticule));
    }
    void Attaquer()
    {
        if (_donnees.nbDeFleches > 0)
        {
            GameObject projectile = Instantiate(_projectile, transform.position, Quaternion.identity);
            _donnees.nbDeFleches--;
            _peutTirer = false;
        }
       else Debug.Log("Plus de flèches");
    }

    public void AugmenterPointageEnnemis(int valeurEnnemi)
    {
        _donnees.nbEnemisTues += valeurEnnemi;
    }
    public void AugmenterNbVie()
    {
        _donnees.nbDeVie++;
    }

    /// <summary> #tp4 Jean-Samuel
    /// Coroutine pour augmenter la vitesse de déplacement
    /// et pour transformer les particules
    /// </summary>
    /// <returns>Revient à la normal après 10 secondes</returns>
    IEnumerator CoroutineSuperVitesse(float modificateurVitesse, Color couleurParticule, float grosseurParticule)
    {
        Color couleurBase = _sr.color; //Mets la couleur de base dans une variable
        _sr.color = new Color(couleurBase.r, 0.5f, 0, 5f); //Change la couleur du perso
        _gestParticules.ModiffierParticulesEnBonus(grosseurParticule, couleurParticule);
        float vitesseBase = _vitesse; //Mets la vitesse de base dans une variable
        _vitesse = _vitesse *modificateurVitesse; //Multiplie la vitesse par 2
        yield return new WaitForSeconds(10f); //Attend 10 secondes
        _gestParticules.RemettreLesParticulesNormal();
        _vitesse = vitesseBase; //Remet la vitesse de base
        _sr.color = couleurBase; //Remet la couleur de base
    }


    /// <summary> #tp4 Jean-Samuel
    /// Coroutine pour augmenter la force du saut
    /// et pour transformer les particules
    /// </summary>
    /// <returns>Revient à la normal après 10 secondes</returns>
    IEnumerator CoroutineSuperSaut(float modificateurSaut, Color couleurParticule, float grosseurParticule)
    {
        Color couleurBase = _sr.color; //Mets la couleur de base dans une variable
        _sr.color = new Color(couleurBase.r, 0.5f, couleurBase.b); //Change la couleur du perso
        _gestParticules.ModiffierParticulesEnBonus(grosseurParticule, couleurParticule);
        float sautDeBase = _forceSaut; //Mets la force de saut de base dans une variable
        _forceSaut = _forceSaut * modificateurSaut; //Multiplie la force de saut
        yield return new WaitForSeconds(10f); //Attend 10 secondes
        _gestParticules.RemettreLesParticulesNormal();
        _forceSaut = sautDeBase; //Remet la force de saut de base
        _sr.color = couleurBase; //Remet la couleur de base
    }

    IEnumerator CoroutineAttaque()
    {
        while (_peutTirer)
        {
            Attaquer();
            yield return new WaitForSeconds(0.3f);

        }
        yield return new WaitForSeconds(1);
        _peutTirer = true;
        _coroutineAttaqueEnCour = false;
    }

}