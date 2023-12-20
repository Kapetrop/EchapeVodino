using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
/// <summary>
/// Classe qui gère l'instanciation de la salle bonus
/// dans la scene bonus
/// Code fait et commenté par Jean-Samuel
/// #synthese Jean-Samuel
/// </summary>
public class NiveauBonus : MonoBehaviour
{
    
    [SerializeField] GameObject[] _tSallesModeles; // Tableau des salles de la scène bonus
    [SerializeField] GameObject _gmVirtualCam; // Parle au GameObjet cinemachine #tp4 Antoine
    private CinemachineVirtualCamera _cmVirtualCam; // Parle a la composante virtual caméra du cinemachine #tp4 Antoine
    public CinemachineVirtualCamera cmVirtualCam => _cmVirtualCam;
    private CinemachineConfiner2D _cmConfiner; // Parle a la composante confiner du cinemachine #tp4 Antoine
    [SerializeField] PolygonCollider2D _polyConfiner; // le polygone confiner #tp4 Antoine
    static private NiveauBonus _instance; // instance de la classe
    static public NiveauBonus instance => _instance; // getter
    [SerializeField] float _tempsDePan = 2f; // temps de pan de la caméra #synthese Jean-Samuel
    [SerializeField] GameObject _arrierePlan; // arriere plan de la salle #synthese Jean-Samuel
    

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (_instance != null) { Destroy(gameObject); return; } // Singleton
        _instance = this; // Singleton
        _cmVirtualCam = _gmVirtualCam.GetComponent<CinemachineVirtualCamera>(); // va chercher le composant VirtualCam pour lui assigner quoi folllow # tp4 Antoine
        _cmConfiner = _gmVirtualCam.GetComponent<CinemachineConfiner2D>();  // va chercher le composant Confiner pour le modifier selon la taille du niveau # tp4 Antoine

        int salle = Random.Range(0, _tSallesModeles.Length);
        Instantiate(_tSallesModeles[salle], transform.position, Quaternion.identity);
        _cmVirtualCam.Follow = Perso.instance.transform; // assigne le perso à la caméra #tp4 Antoine
    }

    //méthode pour changer le size du ortho lens
    public void ChangerSizeOrtho(float size)
    {
        Coroutine corout = StartCoroutine(ChangerSizeOrthoSur2Sec(size));
    }
    
    //Coroutine pour changer le size du ortho lens sur 2 secondes en lien avec la méthode ChangerSizeOrtho
    public IEnumerator ChangerSizeOrthoSur2Sec(float size)
    {
        float temps = 0;
        float sizeOrthoDepart = _cmVirtualCam.m_Lens.OrthographicSize;
        float grosseurAP = 2f;
        while (temps < _tempsDePan)
        {
            temps += Time.deltaTime;
            float fraction = temps/ _tempsDePan;
            fraction = Mathf.SmoothStep(0,1,fraction);
            _arrierePlan.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * grosseurAP,fraction);
            _cmVirtualCam.m_Lens.OrthographicSize = Mathf.Lerp(sizeOrthoDepart, size,fraction);
            yield return null;
        }
    }

}
