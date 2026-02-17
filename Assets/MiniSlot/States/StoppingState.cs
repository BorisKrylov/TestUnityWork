using AxGrid;
using AxGrid.FSM;

namespace MiniSlot.States
{
    /// <summary>
    /// Состояния остановки слотов.
    /// Слушает событие полной остановки, после чего переходит в StateIdle
    /// </summary>
    [State(Consts.State.StateStopping)]
    internal class StoppingState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Settings.Model.Set(Consts.Buttons.StartButton.EnableField, false);
            Settings.Model.Set(Consts.Buttons.StopButton.EnableField, false);

            Settings.Model.EventManager.Invoke(Consts.Events.SpinStop);
            Settings.Model.EventManager.AddAction<int>(Consts.Events.SlotStopped, OnStopped);
        }
            
        private void OnStopped(int centerIconId)
        {
            Settings.Model.Set(Consts.ModelFields.LastReward, centerIconId);
            Settings.Model.EventManager.Invoke(Consts.Events.PlayBurst);

            Parent.Change(Consts.State.StateIdle);
        }
    }
}