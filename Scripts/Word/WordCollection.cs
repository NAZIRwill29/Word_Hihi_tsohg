using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WordCollectionCategory : WordCollection
{
    public NameDataFlyweight CategoryNameData;
}

[System.Serializable]
public class WordCollection
{
    public TextAsset WordTextAsset;
    public List<string> Words;

    public void LoadWordsFromTextAsset()
    {
        WordLoader.LoadWordsFromTextAsset(Words, WordTextAsset);
    }
}
