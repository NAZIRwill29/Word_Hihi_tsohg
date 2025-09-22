using System.Collections.Generic;
using UnityEngine;

//weakness word owned by player
public class WeaknessWordCollection : MonoBehaviour
{
    public List<string> WeaknessWords = new(); //{ get; private set; }

    public void AddWeaknessWords(string word)
    {
        WeaknessWords.Add(word);
    }

    public void RemoveWeaknessWords(string word)
    {
        WeaknessWords.Remove(word);
    }
}
