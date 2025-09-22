using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IndoAlphabetList", menuName = "WordSystem/AlphabetList/IndoAlphabetList", order = 1)]
public class IndoAlphabetListSO : AlphabetListSO
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
        'K', 'K', 'K',
        'L', 'L',
        'M',
        'N', 'N', 'N',
        'O',
        'P',
        'Q',
        'R', 'R', 'R',
        'S', 'S',
        'T', 'T',
        'U', 'U', 'U', 'U',
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
