using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using MiniSlot.States;
using UnityEngine;

namespace MiniSlot
{
    /// <summary>
    /// Основная точка входа в FSM слотов
    /// </summary>
    public class MiniSlotMain : MonoBehaviourExtBind
    {
        [OnStart]
        private void StartThis()
        {
            Log.Debug("MiniSlot Start");
            
            Settings.Fsm = new FSM();
            
            Settings.Fsm.Add(new InitState());
            Settings.Fsm.Add(new IdleState());
            Settings.Fsm.Add(new StartingState());
            Settings.Fsm.Add(new SpinState());
            Settings.Fsm.Add(new StoppingState());
            
            Settings.Fsm.Start(Consts.State.StateInit);
        }

        [OnUpdate]
        private void UpdateThis()
        {
            Settings.Fsm.Update(Time.deltaTime);
        }
    }
}