using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PGSauce.AudioManagement;

namespace PGSauce.Games.DropDaBomb
{
    public class BackgroundMusic : MonoBehaviour
    {
        [SerializeField] private AudioObject audioObject;

        private void Start()
        {
            AudioController.Instance.Play(audioObject);
        }
    }
}
