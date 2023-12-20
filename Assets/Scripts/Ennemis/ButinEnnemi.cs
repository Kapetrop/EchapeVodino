using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Classe qui gère le butin des ennemis
/// Code fait et commenté par Jean-Samuel
/// </summary>
public class ButinEnnemi : MonoBehaviour
{

    private Rigidbody2D _rb;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

    }
    //méthode pour diriger le butin dans une direction aléatoir vers le haut
    public void LancerButin(GameObject butin)
    {
        Vector3 nouvellePosition = new Vector3(transform.position.x, transform.position.y, 0);

        GameObject nouveauButin = Instantiate(butin, nouvellePosition, Quaternion.identity);
        Rigidbody2D rbButin = nouveauButin.GetComponent<Rigidbody2D>();

        if (rbButin != null)
        {
            Vector2 direction = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
            rbButin.AddForce(direction * 400f);
        }
    }
}
