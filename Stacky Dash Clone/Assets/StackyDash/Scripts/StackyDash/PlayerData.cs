using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerData",menuName ="Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("PlayerStats")]
    [SerializeField] int level;
    [SerializeField] int coin;
  


  
    public int Level
   {
     get=>PlayerPrefs.GetInt("Level",level);
     set=>PlayerPrefs.SetInt("Level",value);
   }
   

       public int Coin
    {
         get=>PlayerPrefs.GetInt("Coin",coin);
         set{ PlayerPrefs.SetInt("Coin", value); }
    }

}
