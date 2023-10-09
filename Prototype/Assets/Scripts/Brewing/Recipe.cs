using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A scriptable object back-end class that specifies the particulars for each item
// Right click > Create > GUI > ShopItem
[CreateAssetMenu(fileName = "Recipe", menuName = "GUI/Recipes", order = 1)]
public class Recipe : ScriptableObject
{
    [Header("Ingredients")]
    public ShopItem ingredient01;
    public ShopItem ingredient02;
    public ShopItem ingredient03;

    [Header("Potion")]
    public ShopItem potion;
}
