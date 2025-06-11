using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "nuevo ingrediente",menuName = "ItemsIngredientePocion")]
public class ItemIngredientes : ScriptableObject
{
    public int ID;
    public string nombre;

    public Sprite imagen;
}
