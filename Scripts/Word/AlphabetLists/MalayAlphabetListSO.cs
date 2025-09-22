using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MalayAlphabetList", menuName = "WordSystem/AlphabetList/MalayAlphabetList", order = 1)]
public class MalayAlphabetListSO : AlphabetListSO
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
        'K', 'K',
        'L', 'L',
        'M', 'M', 'M',
        'N', 'N', 'N',
        'O',
        'P',
        'Q',
        'R', 'R', 'R',
        'S', 'S',
        'T',
        'U', 'U', 'U', 'U',
        'V',
        'W',
        'X',
        'Y',
        'Z',
    };

    public override List<char> GetListAlphabet()
    {
        return m_ListAlphabet;
    }
}
