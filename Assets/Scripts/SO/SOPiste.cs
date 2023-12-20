using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Piste musicale", fileName = "DonneesPiste")] // permet de creer un SO a partir du menu Assets/Create/... #tp4 Antoine

/// <summary> #tp4 Antoine
/// Servira de scriptable object pour les pistes musicales syncro
/// </summary>
public class SOPiste : ScriptableObject
{
    [SerializeField] private TypePiste _type; // un enum pour le type de piste #tp4 Antoine
    [SerializeField] private AudioClip _clip; //le clip à jouer #tp4 Antoine
    [SerializeField] private bool _estActifParDefaut; //permet de choisir l'état initial #tp4 Antoine
    [SerializeField] private bool _estActif; //c'est l'état actuel #tp4 Antoine  
    [SerializeField] private float _volumeOptimalInitial;
    public float volumeOptimalInitial => _volumeOptimalInitial; // getter du volume optimal initial #synthèse Antoine
    [SerializeField] private float _volumeOptimal;
    //setter et getter du volume optimal #synthèse Antoine
    public float volumeOptimal
    {
        get => _volumeOptimal;
        set => _volumeOptimal = value;
    }
    private AudioSource _source; //la source audio qui jouera le clip #tp4 Antoine
    public AudioSource source => _source; // getter de la source #tp4 Antoine
    public TypePiste type => _type; // getter du type #tp4 Antoine

    // getter et setter de l'etat appelle changer volume lorsqu'il est modifié #tp4 Antoine
    public bool estActif
    {
        get => _estActif;
        set
        {
            _estActif = value;
            AjusterVolume();
        }
    }

    /// <summary> #tp4 Antoine
    /// ajuste les parametre de la source audio a sa création
    /// </summary>
    /// <param name="source">audio source créer par prog par le SoundManager</param>
    public void Initialiser(AudioSource source)
    {
        _source = source;
        _source.clip = _clip;
        _source.loop = true;
        _source.playOnAwake = false;
        _source.volume = 0;
        _volumeOptimal = _volumeOptimalInitial; //initialise le volume optimal #synthèse Antoine
        _source.Play();
        _estActif = _estActifParDefaut;
    }
    /// <summary> #tp4 Antoine
    /// change le volume de la source audio en fonction de l'état de la piste
    /// </summary>
    public void AjusterVolume()
    {
        if (_estActif)
        {
            SoundManager.instance.FadeIn(this, 1f);
        }
        else
        {
            SoundManager.instance.FadeOut(this, 0.5f);
        }
    }
}
