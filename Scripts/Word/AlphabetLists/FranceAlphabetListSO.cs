using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FranceAlphabetList", menuName = "WordSystem/AlphabetList/FranceAlphabetList", order = 1)]
public class FranceAlphabetListSO : AlphabetListSO
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
        'O', 'O', 'O', 'O',
        'P',
        'Q',
        'R', 'R', 'R',
        'S', 'S', 'S',
        'T', 'T',
        'U', 'U',
        'V',
        'W',
        'X',
        'Y',
        'Z',
        'É',
        'À',
        'È',
        'Ù',
        'Ë',
        'Ü',
        'Ï',
        'Â',
        'Ê',
        'Î',
        'Ô',
        'Û',
        'Ç',
        'Æ',
        'Œ'
    };

    public override List<char> GetListAlphabet()
    {
        return m_ListAlphabet;
    }
}
