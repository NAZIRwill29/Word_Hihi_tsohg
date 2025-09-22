using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnglishAlphabetList", menuName = "WordSystem/AlphabetList/EnglishAlphabetList", order = 1)]
public class EnglishAlphabetListSO : AlphabetListSO
{
    [SerializeField]
    private List<char> m_ListAlphabet = new()
    {
        'A', 'A', 'A', 'A', 'A',
        'B',
        'C',
        'D',
        'E', 'E', 'E', 'E', 'E',
        'F',
        'G',
        'H',
        'I', 'I', 'I', 'I',
        'J',
        'K',
        'L', 'L',
        'M',
        'N', 'N', 'N',
        'O', 'O', 'O',
        'P',
        'Q',
        'R', 'R', 'R', 'R',
        'S', 'S',
        'T', 'T', 'T',
        'U', 'U',
        'V',
        'W',
        'X',
        'Y',
        'Z'
    };

    public override List<char> GetListAlphabet()
    {
        return m_ListAlphabet;
    }
}
