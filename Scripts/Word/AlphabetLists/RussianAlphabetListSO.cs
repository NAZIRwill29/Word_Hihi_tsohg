using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RussianAlphabetList", menuName = "WordSystem/AlphabetList/RussianAlphabetList", order = 1)]
public class RussianAlphabetListSO : AlphabetListSO
{
    [SerializeField]
    private List<char> m_ListAlphabet = new()
    {
        'A', 'A', 'A', 'A',
        'Б',
        'В', 'В',
        'Г',
        'Д',
        'Е', 'Е', 'Е', 'Е', 'Е',
        'Ё',
        'Ж',
        'З',
        'И', 'И', 'И', 'И',
        'Й',
        'К',
        'Л', 'Л',
        'М',
        'Н', 'Н', 'Н',
        'О', 'О', 'О', 'О', 'О',
        'П',
        'Р', 'Р',
        'С', 'С', 'С',
        'Т', 'Т', 'Т',
        'У',
        'Ф',
        'Х',
        'Ц',
        'Ч',
        'Ш',
        'Щ',
        'Ъ',
        'Ы',
        'Ь',
        'Э',
        'Ю',
        'Я'
    };

    public override List<char> GetListAlphabet()
    {
        return m_ListAlphabet;
    }
}
