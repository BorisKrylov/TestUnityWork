using AxGrid;
using AxGrid.FSM;

namespace MiniSlot.States
{
    /// <summary>
    /// Состояние начала вращения слотов.
    /// Через 3 секунды активирует кнопку остановки и переходит в StateSpin.
    /// </summary>
    [State(Consts.State.StateStarting)]
    internal class StartingState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Settings.Model.Set(Consts.Buttons.StartButton.EnableField, false);
            Settings.Model.Set(Consts.Buttons.StopButton.EnableField, false);

            Settings.Model.EventManager.Invoke(Consts.Events.SpinStart);
        }

        [One(3f)]
        private void EnableStopAfterDelay()
        {
            Settings.Model.Set(Consts.Buttons.StopButton.EnableField, true);
            Parent.Change(Consts.State.StateSpin);
        }
    }
}