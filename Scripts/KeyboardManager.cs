using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BannedLetterKey
{
    public char Letter;
    public float Time;
    public float Cooldown;
}

public class KeyboardManager : Singleton<KeyboardManager>
{
    public List<BannedLetterKey> BannedLetterKeys = new List<BannedLetterKey>();
    private List<string> m_BannedLetters = new List<string>();
    [SerializeField] private int m_MaxNum;

    void Update()
    {
        if (GameManager.Instance.IsPause) return;
        if (BannedLetterKeys == null || BannedLetterKeys.Count == 0) return;

        for (int i = 0; i < BannedLetterKeys.Count; i++)
        {
            BannedLetterKeys[i].Cooldown -= Time.deltaTime;
            if (BannedLetterKeys[i].Cooldown <= 0)
                RemoveBannedLetterKeys(BannedLetterKeys[i]);
        }
    }

    public bool CheckIsBanned(char letter)
    {
        if (BannedLetterKeys == null || BannedLetterKeys.Count == 0)
            return false;

        char upper = char.ToUpperInvariant(letter);
        return BannedLetterKeys.Exists(x => char.ToUpperInvariant(x.Letter) == upper);
    }

    public void AddBannedLetterKeys(BannedLetterKey bannedLetterKey)
    {
        Debug.Log("AddBannedLetterKeys " + bannedLetterKey.Letter);
        if (BannedLetterKeys.Count >= m_MaxNum) return;

        bannedLetterKey.Cooldown = bannedLetterKey.Time;
        bannedLetterKey.Letter = char.ToUpperInvariant(bannedLetterKey.Letter);
        BannedLetterKeys.Add(bannedLetterKey);
        m_BannedLetters.Add(bannedLetterKey.Letter.ToString());
        GhostCombatSystem.Instance.CombatUI.ChangeDisableKey(m_BannedLetters);
    }

    public void RemoveBannedLetterKeys(char letter)
    {
        char upper = char.ToUpperInvariant(letter);
        BannedLetterKey bannedLetterKey = BannedLetterKeys.Find(x => x.Letter == upper);
        if (bannedLetterKey != null)
            RemoveBannedLetterKeys(bannedLetterKey);
    }

    public void RemoveBannedLetterKeys(BannedLetterKey bannedLetterKey)
    {
        BannedLetterKeys.Remove(bannedLetterKey);
        m_BannedLetters.Remove(bannedLetterKey.Letter.ToString());
        GhostCombatSystem.Instance.CombatUI.ChangeDisableKey(m_BannedLetters);
    }

    public void ResetBannedLetterKeys()
    {
        BannedLetterKeys.Clear();
        m_BannedLetters.Clear();
        GhostCombatSystem.Instance.CombatUI.ChangeDisableKey(m_BannedLetters);
    }
}
