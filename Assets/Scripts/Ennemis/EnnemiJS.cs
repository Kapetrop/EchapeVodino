using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary> 
/// Classe qui gère les ennemis qui lancent des projectiles
/// Code fait et commenté par Jean-Samuel
/// </summary>
public class EnnemiJS : BaseEnnemi
{
    [SerializeField] private ProjectileEnnemi _projectile; // prefab du projectile #synthese Jean-Samuel
    private float _tempsDeRecharge; // temps de recharge entre chaque attaque #synthese Jean-Samuel
    private bool _peutTirer = false; // booléen qui permet de savoir si l'ennemi peut tirer #synthese Jean-Samuel
    private bool _coroutineAttaqueEnCour = false; // booléen qui permet de savoir si la coroutine est en cours #synthese Jean-Samuel
    [SerializeField] private float _rayonDetectionJS; // rayon de détection #synthese Jean-Samuel
    private bool _estActif = false;

    /// <summary>
    /// Méthode start de la baseEnnemi est appeler et le temps de recharge est initialisé
    /// </summary>
    protected override void Start()
    {
        base.Start();
        _tempsDeRecharge = Random.Range(0, 2f);
    }
    /// <summary> #synthese Antoine
    ///  Détecte si le joueur est dans la zone de détection
    /// </summary>
    /// <param name="collision"></param>
    private void FixedUpdate()
    {
        _estActif = base.VerifierPerso(_rayonDetectionJS, new Vector3(1,0,0));
        base.FaireAnimation();
    
    }
    void Update()
    {
        //permet de ne pas déclencher la coroutine si elle est déjà en cours #synthese Jean-Samuel
        if (_estActif && !_coroutineAttaqueEnCour)
        {

            Coroutine corout = StartCoroutine(CoroutineAttaque());
            _coroutineAttaqueEnCour = true;
        }
    }
    /// <summary>
    /// Méthode qui permet de lancer le projectile
    /// </summary>
    private void Attaquer()
    {
        Instantiate(_projectile, transform.position, Quaternion.identity);
    }
    /// <summary>
    /// Coroutine qui permet de lancer le projectile et de le recharger
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoroutineAttaque()
    {
        yield return new WaitForSeconds(_tempsDeRecharge);
        while (_estActif)
        {
            
            Attaquer();
            _tempsDeRecharge = Random.Range(0, 2f);
           
            yield return new WaitForSeconds(_tempsDeRecharge);
            _peutTirer = true;
            _coroutineAttaqueEnCour = false;
        }
    }

}
