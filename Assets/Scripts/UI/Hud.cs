using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary> #tp4 Antoine
/// controle HUD (heads up display)
/// </summary>
public class Hud : MonoBehaviour
{
    private bool _estPause = false; // si le jeu est en pause #synthèse Antoine
    [SerializeField] Image _fondNoir; // image du fond noir #synthèse Antoine
    private float _alpha = 1f; // valeur de transparence #synthèse Antoine


    [Header("Source")]
    [SerializeField] private SOPerso _donnees;
    [SerializeField] SONavigation _donneesNav; // lien vers SO navigation #synthèse Antoine


    [Header("Champs")]
    [SerializeField] private TextMeshProUGUI _champNbBrocolis;
    [SerializeField] private TextMeshProUGUI _champNumNiveau;
    [SerializeField] private TextMeshProUGUI _champTemps;
    [SerializeField] private GameObject _panneauPause;
    private Animator _animTemps;

    [Header("Portrait")]
    [SerializeField] private GameObject[] _iconVie;  // tableau de gameObjects pour les icone de vies #synthèse Antoine
    private int _vies; // nombre de vie du SOPerso #synthèse Antoine
    [SerializeField] private Image _imgPortaitVodinoHeureux; // image du portrait de Vodino heureux #synthèse Antoine
    [SerializeField] private Image _imgPortaitVodinoHeurte; // image du portrait de Vodino blessé #synthèse Antoine`

    [Header("Flèches")]
    private int _nbFleches; // nombre de fleches du SOPerso #synthèse Antoine
    [SerializeField] private GameObject _fleche; // gameobject de la fleche #synthèse Antoine
    [SerializeField] private GameObject _PaquetFleches; // gameobject du paquet de fleches #synthèse Antoine



    private void Start()
    {
        // S'abonne a l'évenement de mise a jour des données pour Activé la fonction d'affichage de données # tp4 Antoine
        _donnees.evenementMiseAJour.AddListener(ActualiseToi);
        Niveau.instance.activeHorloge.AddListener(ChangerVitesseAnim); // s'abonne à l'évenement de l'horloge #synthèse Antoine
        _animTemps = _champTemps.GetComponent<Animator>();
        _alpha = 0f;
        _fondNoir.color = new Color(_fondNoir.color.r, _fondNoir.color.g, _fondNoir.color.b, _alpha);
        _imgPortaitVodinoHeureux.gameObject.SetActive(true);
        _imgPortaitVodinoHeurte.gameObject.SetActive(false);
        _panneauPause.SetActive(false);
        for (int i = 0; i < _donnees.nbDeFlechesMax; i++) // instancie le nombre de fleches max #synthèse Antoine
        {
            Instantiate(_fleche, _PaquetFleches.transform);
        }
        // ActualiseToi();
    }

    /// <summary>#synthèse Antoine
    /// Modifie la vitesse de l'animation selon le modificateur de temps 
    /// </summary>
    private void ChangerVitesseAnim()
    {
        _animTemps.speed = Niveau.instance.modificateurTemps;
    }

    /// <summary>
    /// Actualise les champs du UI avec les infos du SOPerso #tp4 Antoine
    /// </summary>
    void ActualiseToi()
    {
        _champNbBrocolis.text = _donnees.nbDeBrocolis.ToString();
        _champNumNiveau.text = _donnees.niveau.ToString();
        if (_champTemps.text != _donnees.tempsRestant.ToString()) _animTemps.SetTrigger("change");
        _champTemps.text = _donnees.tempsRestant.ToString();

        // fait disparaitre tous les prefabs de fleches puis rapparaitre le bon nombre #synthèse Antoine
        if (_donnees.nbDeFleches != _nbFleches) // si le nombre de fleches change #synthèse Antoine
        {
            for (int i = 0; i < _donnees.nbDeFlechesMax; i++)
            {
                _PaquetFleches.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < _donnees.nbDeFleches; i++)
            {
                _PaquetFleches.transform.GetChild(i).gameObject.SetActive(true);
            }
            _nbFleches = _donnees.nbDeFleches;
        }


        //fait disparaitre tous les icones de vie puis rapparaitre le bon nombre #synthèse Antoine
        if (_donnees.nbDeVie != _vies) // si le nombre de vie change #synthèse Antoine
        {
        for (int i = 0; i < _iconVie.Length; i++)
        {
            _iconVie[i].SetActive(false);
        }
        for (int i = 0; i < _donnees.nbDeVie; i++)
        {
            if (i > _iconVie.Length) break;
            _iconVie[i].SetActive(true);
        }
        _vies = _donnees.nbDeVie;
        if (_vies == 0)
        {
            StartCoroutine(CoroutineFonduNoir());
        }

        StartCoroutine(CoroutineChangerTeteVodino());
        }
    }

    /// <summary> #synthèse Antoine
    /// Coroutine qui change le portrait de Vodino quand il se fait toucher
    /// </summary>
    IEnumerator CoroutineChangerTeteVodino()
    {
        _vies = _donnees.nbDeVie;
        yield return new WaitForSeconds(0.2f);
        _imgPortaitVodinoHeurte.gameObject.SetActive(true);
        _imgPortaitVodinoHeureux.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        _imgPortaitVodinoHeureux.gameObject.SetActive(true);
        _imgPortaitVodinoHeurte.gameObject.SetActive(false);
    }

    /// <summary>#synthèse Antoine
    /// Coroutine qui fait un fondu noir et changer la scène
    /// </summary>
    IEnumerator CoroutineFonduNoir()
    {
        Perso.instance.rb.bodyType = RigidbodyType2D.Static;
        _fondNoir.color = new Color(_fondNoir.color.r, _fondNoir.color.g, _fondNoir.color.b, _alpha);
        while (_alpha < 1f)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            Debug.Log(_alpha);
            _alpha += 0.005f;
            _fondNoir.color = new Color(_fondNoir.color.r, _fondNoir.color.g, _fondNoir.color.b, _alpha);
            if (_alpha > 0.98) // quand le fade terine, change de scène #synthèse Antoine
            {
                _donnees.Initialiser();
                _donneesNav.AllezScore(); 
            }
        }

        yield return null;
    }
    /// <summary> #synthèse Antoine
    /// Met le jeu en pause ou en marche avec escape 
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _estPause = !_estPause;
            if (_estPause)
            {
                Time.timeScale = 0;
                _panneauPause.SetActive(true);
                SoundManager.instance.ReduitVolumeTousLesPistes();
                ;
            }
            else
            {
                Time.timeScale = 1;
                _panneauPause.SetActive(false);
                SoundManager.instance.RemonteTousLesPistes();
            }
        }
    }

    /// <summary> #synthèse Antoine
    /// Retourne au menu principal
    /// </summary>
    public void RetournerMenu()
    {
        Time.timeScale = 1;
        Perso.instance.donneesNav.AllezAuMenu();
    }
}
