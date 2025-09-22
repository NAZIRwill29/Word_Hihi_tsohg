using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AlphabetListManager", menuName = "WordSystem/AlphabetListManager", order = 1)]
public class AlphabetListManagerSO : ScriptableObject
{
    [SerializeField] private List<AlphabetListSO> m_AlphabetListSOs;
    public AlphabetListSO CurrentAlphabetListSO;// { get; private set; }
    public List<char> listAllAlphabets = new()
    {
        'A',
        'B',
        'C',
        'D',
        'E',
        'F',
        'G',
        'H',
        'I',
        'J',
        'K',
        'L',
        'M',
        'N',
        'O',
        'P',
        'Q',
        'R',
        'S',
        'T',
        'U',
        'V',
        'W',
        'X',
        'Y',
        'Z',
        'Á',
        'Â',
        'Ã',
        'À',
        'Ç',
        'É',
        'Ê',
        'Ë',
        'È',
        'Ù',
        'Ü',
        'Ú',
        'Û',
        'Í',
        'Î',
        'Ï',
        'Ó',
        'Ô',
        'Õ',
        'Æ',
        'Œ',
        'Б',
        'В',
        'Г',
        'Д',
        'Е',
        'Ё',
        'Ж',
        'З',
        'И',
        'Й',
        'К',
        'Л',
        'М',
        'Н',
        'О',
        'П',
        'Р',
        'С',
        'Т',
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

    public void SetAlphabetList(string languageName)
    {
        CurrentAlphabetListSO = m_AlphabetListSOs.Find(x => x.LanguageNameData.Name == languageName);
    }

}
