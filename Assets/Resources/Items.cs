using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "nuevo item",menuName = "Items")]
public class Items : ScriptableObject
{
    public int ID;
    public string nombre;

    public Sprite imagen;
}
