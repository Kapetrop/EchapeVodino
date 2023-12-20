using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName ="Navigation", menuName = "Navigation")]

/// <summary> #tp3 Antoine
/// gère la navigation entre les scènes
/// </summary>
public class SONavigation : ScriptableObject
{
    [SerializeField] SOPerso _donneesPerso; // pour parler au donnés du perso -> pour les rénitialisé
    public void Jouer()
    {
        SoundManager.instance.ChangerEtatLecturePiste(TypePiste.musiqueBase, true);
        AllerSceneSuivante();
    }
    public void SortirBoutique()
    {
        _donneesPerso.niveau++;
        SoundManager.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenBoutique, false);
        AllerScenePrecedente();
    }
    public void AllerSceneSuivante()
    {
        SoundManager.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenSalle, false);
        SoundManager.instance.ChangerEtatLecturePiste(TypePiste.musiqueBaseBasse, false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1); 
    }
    public void AllerScenePrecedente()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1); 
    }
    public void AllezAuMenu()
    {
        SoundManager.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenSalle, false);
        SoundManager.instance.ChangerEtatLecturePiste(TypePiste.musiqueBaseBasse, false);
        SceneManager.LoadScene(0);
    }
    public void AllezAuGenerique()
    {
        SoundManager.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenSalle, true);
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings-1);
    }
    public void AllezScore()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings-3);
    }
    
    public void AllerALaBoutique()
    {
        SoundManager.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenSalle, false);
        SoundManager.instance.ChangerEtatLecturePiste(TypePiste.musiqueBaseBasse, false);
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings-4);
    }
    public void AllerAuNiveauBonus()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings-2);
        SoundManager.instance.ChangerEtatLecturePiste(TypePiste.musiqueBaseBasse, true);
    }

}
