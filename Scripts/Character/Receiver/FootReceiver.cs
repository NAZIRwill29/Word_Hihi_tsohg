using UnityEngine;

public class FootReceiver : Receiver, IReceiver, IAnimatioanable
{
    public void InitAnimation(string name)
    {
        if (name == "Hit" || name == "Heal")
        {
            ObjectHealth objectHealth = ObjectT.ObjectHealth;
            if (objectHealth)
            {
                Debug.Log("InitAnimation");
                if (name == "Hit")
                    objectHealth.IsHit = true;
                else if (name == "Heal")
                    objectHealth.IsHeal = true;
            }
        }
    }
}
