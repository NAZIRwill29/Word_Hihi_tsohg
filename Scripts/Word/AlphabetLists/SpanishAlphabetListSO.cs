using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpanishAlphabetList", menuName = "WordSystem/AlphabetList/SpanishAlphabetList", order = 1)]
public class SpanishAlphabetListSO : AlphabetListSO
{
    [SerializeField]
    private List<char> m_ListAlphabet = new()
    {
        'A', 'A', 'A', 'A', 'A',
        'B',
        'C',
        'D', 'D',
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
        'O', 'O', 'O', 'O',
        'P',
        'Q',
        'R', 'R', 'R',
        'S', 'S', 'S',
        'T',
        'U', 'U',
        'V',
        'W',
        'X',
        'Y',
        'Z',
        'Á',
        'É',
        'Í',
        'Ó',
        'Ú',
        'Ü'
    };

    public override List<char> GetListAlphabet()
    {
        return m_ListAlphabet;
    }
}
