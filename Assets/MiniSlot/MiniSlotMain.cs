using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using UnityEngine;

namespace MiniSlot
{
    public class MiniSlotMain : MonoBehaviourExtBind
    {
        private MiniSlotView view;

        [OnAwake]
        private void AwakeThis()
        {
            Log.Debug("ExMain Awake");
        }
        
        [OnStart]
        private void StartThis()
        {
            Log.Debug("ExMain Start");

            Settings.Fsm = new FSM();
            MiniSlotFsm.Install(Settings.Fsm);
        }

        [OnUpdate]
        private void UpdateThis()
        {
            Settings.Fsm.Update(Time.deltaTime);
        }
    }
}