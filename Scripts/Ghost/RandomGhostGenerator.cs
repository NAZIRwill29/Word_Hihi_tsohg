using UnityEngine;
using System.Collections.Generic;

//UNUSED()
public class RandomGhostGenerator : MonoBehaviour
{
    public WordCollection ghostNames;
    public List<Sprite> ghostAppearances;
    public List<RandomGhostBehaviorSO> behaviors;

    public WordPoolManager wordPoolManager;

    void Awake()
    {
        ghostNames.LoadWordsFromTextAsset();
    }

    public RandomGhost GenerateRandomGhost(string wordCategory)
    {
        List<string> sourcePool = wordPoolManager.GetRandomWords(wordCategory, 10);

        RandomGhost newGhost = new RandomGhost();

        newGhost.ghostName = ghostNames.Words[Random.Range(0, ghostNames.Words.Count)];
        newGhost.appearance = ghostAppearances[Random.Range(0, ghostAppearances.Count)];
        newGhost.behaviorData = behaviors[Random.Range(0, behaviors.Count)];
        newGhost.wordPool = sourcePool;
        newGhost.weaknessWord = sourcePool[Random.Range(0, sourcePool.Count)];

        return newGhost;
    }
}
