using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe de base pour l'héritage de dectection de sol et l'affichage de Gizmos
/// Auteurs du code: Antoine Chartier et Jean-Samuel David
/// /// Auteur des commentaires: Jean-Samuel David(chacun doit commenter sa version)
/// </summary>
public class BasePerso : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMaskTuiles; // LayerMask des tuiles
    [SerializeField] private LayerMask _layerMaskEnnemis; // LayerMask des ennemis #synthese Antoine
    protected bool _estAuSol = false; // Personnage touche au sol?
    protected Vector2 _posBoite; // Position de la boite de détection 
    protected Vector2 _tailleBoite; // Taille de la boite de détection
    private float _grandeurPerso;
    private bool _estEnTrainDeChuter = false;
    private bool _estEnTrainDeChuterPreviousState = false;
    // [SerializeField] private AudioClip _effetChute;


    // Start is called before the first frame update
    void Start()
    {
        _grandeurPerso = GetComponent<CapsuleCollider2D>().bounds.size.y;
        _tailleBoite = new Vector2(GetComponent<CapsuleCollider2D>().bounds.extents.x, GetComponent<CapsuleCollider2D>().bounds.extents.y/3);
    }

    /// <summary>
    /// 
    /// </summary>
    virtual protected void FixedUpdate()
    {
        VerifierSol();
    }

    /// <summary>
    /// Vérifie le contact au sol avec Physics2D.OverLapBox
    /// </summary>
    private void VerifierSol()
    {
        if (_grandeurPerso == 0) Start();
        _posBoite = (Vector2)transform.position - new Vector2(0, _grandeurPerso/2); // La boîte de détection suit le personnage
        Collider2D colTuiles = Physics2D.OverlapBox(_posBoite, _tailleBoite, 0, _layerMaskTuiles);
        Collider2D colEnnemis = Physics2D.OverlapBox(_posBoite, _tailleBoite, 0, _layerMaskEnnemis);
        _estAuSol = (colTuiles | colEnnemis != null); // Si le personnage touche à un collider de tuile ou d'ennemi, il est au sol #synthese Antoine
    }
    
    // private void CheckSiAtterie()
    // {
    //     while(_estAuSol)
    //     {
    //         if(_estEnTrainDeChuter != _estEnTrainDeChuterPreviousState)
    //         {
    //             _estEnTrainDeChuter = _e
    //             if(_estEnTrainDeChuter)
    //             {
    //                 SoundManager.instance.JouerEffetSonore(_effetChute);
    //             }
    //         }
    //     }
    // }
    

    /// <summary>
    /// Dessine le gizmos
    /// change la couleur s'il y a contact avec le sol
    /// Vert = contact
    /// Rouge = n'y a pas de contact
    /// </summary>
    void OnDrawGizmos()
    {
        _posBoite = (Vector2)transform.position - new Vector2(0, _grandeurPerso/2); // La boîte de détection suit le personnage

        if (Application.isPlaying == false) VerifierSol();

        if (_estAuSol) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;

        Gizmos.DrawWireCube((Vector3)_posBoite, _tailleBoite);
    }
}
