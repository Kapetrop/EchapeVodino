using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Projectil", menuName = "Projectil")]
public class SOProjectil : ScriptableObject
{
    [SerializeField] private AudioClip _sonImpact;
    [SerializeField] Sprite _sprite;
    
}
