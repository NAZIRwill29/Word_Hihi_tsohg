// using UnityEngine;
// using System.Collections.Generic;

// /// <summary>
// /// Use if you want automated tools to generate GhostTemplate assets in editor time (not at runtime)
// /// Otherwise, manually create them via the Unity inspector.
// /// </summary>
// public class GhostTemplateGenerator : MonoBehaviour
// {
//     public List<string> ghostNames;
//     public List<string> appearances;
//     public List<string> clueSources;
//     public List<WordPool> WordPools;

//     public GhostTemplate CreateTemplate()
//     {
//         GhostTemplate newTemplate = ScriptableObject.CreateInstance<GhostTemplate>();

//         newTemplate.ghostName = GetRandom(ghostNames);
//         newTemplate.appearanceDescription = GetRandom(appearances);
//         newTemplate.entryDescription = "A ghostly figure appears… " + newTemplate.appearanceDescription;

//         newTemplate.clueSources = GetRandomSubset(clueSources, 1, 3);
//         for (int i = 0; i < newTemplate.WordPools.Count; i++)
//         {
//             newTemplate.WordPools[i] = GetRandomSubset(WordPools, 2, 4);
//         }
//         newTemplate.WordPools = GetRandomSubset(WordPools, 2, 4);
//         newTemplate.memoryWords = GetRandomSubset(memoryWords, 2, 4);
//         newTemplate.ritualWords = GetRandomSubset(ritualWords, 2, 4);

//         newTemplate.correctCombos = new List<WordCombo>
//         {
//             new WordCombo { word1 = GetRandom(newTemplate.emotionWords), word2 = GetRandom(newTemplate.memoryWords), resultText = "The ghost calms." }
//         };

//         newTemplate.wrongCombos = new List<WordCombo>
//         {
//             new WordCombo { word1 = GetRandom(emotionWords), word2 = GetRandom(ritualWords), resultText = "The ghost shrieks!" }
//         };

//         newTemplate.onSuccess = "The spirit fades peacefully.";
//         newTemplate.onFail = "The ghost becomes more violent.";
//         newTemplate.rewards = new List<string> { "Charm", "Clue: Forgotten Tomb" };

//         return newTemplate;
//     }

//     private T GetRandom<T>(List<T> list) => list[Random.Range(0, list.Count)];

//     private List<T> GetRandomSubset<T>(List<T> list)
//     {
//         int count = Random.Range(min, max + 1);
//         List<T> copy = new List<T>(list);
//         List<T> result = new List<T>();
//         for (int i = 0; i < count && copy.Count > 0; i++)
//         {
//             int index = Random.Range(0, copy.Count);
//             result.Add(copy[index]);
//             copy.RemoveAt(index);
//         }
//         return result;
//     }
// }
