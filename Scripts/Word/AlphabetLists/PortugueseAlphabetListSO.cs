using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PortugueseAlphabetList", menuName = "WordSystem/AlphabetList/PortugueseAlphabetList", order = 1)]
public class PortugueseAlphabetListSO : AlphabetListSO
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
        'L', 'L',
        'M',
        'N', 'N', 'N',
        'O', 'O', 'O', 'O',
        'P',
        'Q',
        'R', 'R', 'R',
        'S', 'S', 'S',
        'T', 'T',
        'U', 'U',
        'V',
        'X',
        'Z',
        'Á',
        'Â',
        'Ã',
        'À',
        'Ç',
        'É',
        'Ê',
        'Í',
        'Ó',
        'Ô',
        'Õ',
        'Ú'
    };

    public override List<char> GetListAlphabet()
    {
        return m_ListAlphabet;
    }
}
