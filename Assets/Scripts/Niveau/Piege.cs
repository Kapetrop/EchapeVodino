using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//tp3 Jean-Samuel
/// <summary>
/// Classe pour Géré les piège
/// vide pour le moment à construire
/// Créer et commenter par Jean-Samuel
/// est créer pour le moment pour rendre l'effector détectable dans la colission pour modification dans perso
/// </summary>
public class Piege : MonoBehaviour
{
    /// <summary>
    /// Valeur du nom du piege
    /// </summary>
    /// <value>retourne son nom</value>
    [SerializeField] string _nom = "piege";

    public string nom { get => _nom;}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
