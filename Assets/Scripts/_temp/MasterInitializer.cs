using System;
using UnityEngine;

public class MasterInitializer : MonoBehaviour
{
   [SerializeField] GlobalData globalData;

   private void Awake()
   {
      globalData.FirstTalkComplete = false;
      globalData.NurseComplete = false;
      globalData.CorpseComplete = false;
      globalData.ForagingComplete = false;
      globalData.InsideDone = false;
   }
}
