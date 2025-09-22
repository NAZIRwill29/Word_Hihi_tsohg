using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OldVersion
{
    public class PlayerTrigger : MonoBehaviour
    {
        //[SerializeField] UnityEvent myTrigger;
        public PlayerController playerController { get; private set; }

        void Start()
        {
            playerController = GetComponentInParent<PlayerController>();
        }
        public void GetAttacked(int amount, MicrobarAnimType mat)
        {
            playerController.ChangeHealth(-amount, mat);
            //myTrigger.Invoke();
        }
    }
}