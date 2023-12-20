using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary> #tp4 Antoine
/// Gestionnaire des effets sonores et musiques
/// </summary>
public class SoundManager : MonoBehaviour
{
    static private SoundManager _instance; // instance de la classe
    static public SoundManager instance => _instance; // getter
    private AudioSource _sourceEffetsSonores; // audioSource pour les effets sonores, celui visible dans l'inspecteur hors mode jeu #tp4 Antoine
    private AudioSource _sourceEffetsSonoresPerso; // audioSource spécifique pour les effets sonores des personnages #synthese Antoine
    [SerializeField] SOPiste[] _tPistes; // tableau de SO des pistes musicales #tp4 Antoine
    [SerializeField] float _pitchMinEffetsSonores = 0.8f;
    [SerializeField] float _pitchMaxEffetsSonores = 1.2f;
    [SerializeField] float _volumeMusiqueMain = 1f;
    private bool _estMuted = true; // état initial du toggle de musique menu principal #synthèse Antoine>
    private bool _isFading = false; // si un fondu est en cours #synthèse Antoine
    [SerializeField] Image[] _imgToggleMusique; // images du toggle de musique menu principal #synthèse Antoine

    void Start()
    {
        if (_instance != null) { Destroy(gameObject); return; } // Singleton
        _instance = this; // Singleton
        DontDestroyOnLoad(gameObject); // empeche sa destruction au changement de scène, donc son continue de jouer #tp4 Antoine

        _sourceEffetsSonores = GetComponent<AudioSource>();
        _sourceEffetsSonoresPerso = gameObject.AddComponent<AudioSource>();  // crée un AudioSource spécialement pour les effets du personnage #synthese Antoine
        CreerLesSourcesMusicales();
        _imgToggleMusique[0].gameObject.SetActive(_estMuted); // affiche l'image du toggle selon l'état initial #synthèse Antoine
        _imgToggleMusique[1].gameObject.SetActive(!_estMuted);
    }
    public void ActiveMusiqueBase()
    {
        if (!_isFading)
        {
            ChangerEtatLecturePiste(TypePiste.musiqueBase, _estMuted);
            _isFading = true;
            _estMuted = !_estMuted;
            _imgToggleMusique[0].gameObject.SetActive(_estMuted); // affiche l'image du toggle selon l'état #synthèse Antoine
            _imgToggleMusique[1].gameObject.SetActive(!_estMuted);
        }
    }
    /// <summary> #synthèse Antoine
    /// Change le pitch de la piste de base  selon le modificateur de temps
    /// </summary>
    public void AltererPitch()
    {
        if (Niveau.instance.modificateurTemps == 1f) // si le temps est normal, on remet le pitch normal #synthèse Antoine
        {
            _tPistes[0].source.pitch = 1f;
        }
        else // sinon on change le pitch selon le modificateur de temps #synthèse Antoine
        {
            _tPistes[0].source.pitch = Niveau.instance.modificateurTemps;
        }
    }

    /// <summary> #tp4 Antoine
    /// Crée un composant AudioSource par prog pour chacune des pistes
    /// </summary>
    private void CreerLesSourcesMusicales()
    {
        foreach (SOPiste piste in _tPistes)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            piste.Initialiser(source);
        }
    }

    /// <summary> #synthèse Antoine
    /// Permet de jouer un effets sonore avec un pitch aléatoire et personnalisable
    /// dependament d'ou provient le son il y a deux sources audio differentes ( objets differents & Perso) chacun a sa fonction
    /// </summary>
    /// <param name="clip">l'effet sonore à jouer</param>
    /// <param name="volume">le volume, par défaut au max :1 </param>
    /// <param name="pitchReduit">reduire le pitch de 10% ?</param>
    /// <param name="aucunPitch">pas de pitch ?</param>
    public void JouerEffetSonore(AudioClip clip, float volume = 1f, bool pitchReduit = false, bool aucunPitch = false)
    {
        if (aucunPitch) _sourceEffetsSonores.pitch = 1f;
        else if (pitchReduit) _sourceEffetsSonores.pitch = Random.Range(_pitchMinEffetsSonores * 1.1f, _pitchMaxEffetsSonores / 1.1f);
        else _sourceEffetsSonores.pitch = Random.Range(_pitchMinEffetsSonores, _pitchMaxEffetsSonores);
        _sourceEffetsSonores.PlayOneShot(clip, volume);
    }
    public void JouerEffetSonorePerso(AudioClip clip, float volume = 1f, bool pitchReduit = false, bool aucunPitch = false)
    {
        if (aucunPitch) _sourceEffetsSonores.pitch = 1f;
        if (pitchReduit) _sourceEffetsSonoresPerso.pitch = Random.Range(_pitchMinEffetsSonores * 1.1f, _pitchMaxEffetsSonores / 1.1f);
        else _sourceEffetsSonoresPerso.pitch = Random.Range(_pitchMinEffetsSonores, _pitchMaxEffetsSonores);
        _sourceEffetsSonoresPerso.PlayOneShot(clip, 1);
    }

    /// <summary> #tp4 Antoine
    /// change l'état de lecture d'une piste en modifiant son volume
    /// </summary>
    /// <param name="type">la piste</param>
    /// <param name="estActif">Marche ou Arret</param>
    public void ChangerEtatLecturePiste(TypePiste type, bool estActif)
    {
        foreach (SOPiste piste in _tPistes)
        {
            if (piste.type == type)
            {
                piste.estActif = estActif;
                return;
            }
        }
    }
    /// <summary> #synthèse Antoine
    /// Réduit le volume de 25% de tous les pistes actives 
    /// </summary>
    public void ReduitVolumeTousLesPistes()
    {
        foreach (SOPiste piste in _tPistes)
        {
            if (piste.estActif)
            {
                piste.volumeOptimal -= piste.volumeOptimal * 0.5f; // reduit le volume de 50% #synthèse Antoine
                piste.estActif = true; // pour provoquer le changement de volume;
            }
        }
    }
    private void EteinVolumeTousLesPistes()
    {
        foreach (SOPiste piste in _tPistes)
        {
            if (piste.estActif)
            {
                piste.volumeOptimal = 0;
                piste.estActif = true; // pour provoquer le changement de volume;
            }
        }
    }
    /// <summary> #synthèse Antoine
    /// Augmente le volume de 25% de tous les pistes actives
    /// </summary>
    public void RemonteTousLesPistes()
    {
        foreach (SOPiste piste in _tPistes)
        {
            if (piste.estActif)
            {
                piste.volumeOptimal *= 2f; // remet le 50% du volume #synthèse Antoine
                piste.estActif = true; // pour provoquer le changement de volume;
            }
        }
    }
    public void RallumeTousLesPistes()
    {
        foreach (SOPiste piste in _tPistes)
        {
            if (piste.estActif)
            {
                piste.volumeOptimal = piste.volumeOptimalInitial;
                piste.estActif = true; // pour provoquer le changement de volume;
            }
        }
    }

    /// <summary> #synthèse Antoine
    /// fadein la source audio recu en parametre
    /// </summary>
    /// <param name="source">AudioSource à fondre</param>
    /// <param name="fadeTime">durée du fondu></param>
    public void FadeIn(SOPiste piste, float fadeTime)
    {
        StartCoroutine(CoroutineFadeIn(piste, fadeTime));
    }
    public void FadeOut(SOPiste piste, float fadeTime)
    {
        StartCoroutine(CoroutineFadeOut(piste, fadeTime));
    }
    /// <summary> #synthese Antoine
    /// execute un fondu en entrée sur la source audio recu en parametre
    /// </summary>
    /// <param name="source">AudioSource à modifier</param>
    /// <param name="fadeTime">durée du fondu</param>
    IEnumerator CoroutineFadeIn(SOPiste piste, float fadeTime)
    {
        float temps = 0;
        float startVolume = piste.source.volume;
        while (piste.source.volume < piste.volumeOptimal)
        {
            temps += Time.deltaTime;
            piste.source.volume = Mathf.Lerp(startVolume, piste.volumeOptimal, temps / fadeTime);

            yield return null;
        }
        piste.source.volume = piste.volumeOptimal;
        _isFading = false;
    }
    IEnumerator CoroutineFadeOut(SOPiste piste, float fadeTime)
    {
        float startVolume = piste.source.volume;
        float temps = 0;

        while (piste.source.volume > 0)
        {
            temps += Time.deltaTime;
            piste.source.volume = Mathf.Lerp(startVolume, 0, temps / fadeTime);

            yield return null;
        }
        piste.source.volume = 0;
        _isFading = false;
    }
}
