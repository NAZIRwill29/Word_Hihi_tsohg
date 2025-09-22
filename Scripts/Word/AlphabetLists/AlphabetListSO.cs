using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AlphabetList", menuName = "WordSystem/AlphabetList/AlphabetList", order = 1)]
public class AlphabetListSO : ScriptableObject
{
    public NameDataFlyweight LanguageNameData;

    public virtual List<char> GetListAlphabet()
    {
        return null;
    }
}
