using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

namespace MiniSlot.States
{
    /// <summary>
    /// Состояния вращения слотов с активной кнопкой Stop.
    /// Переходит в StateStopping
    /// </summary>
    [State(Consts.State.StateSpin)]
    internal class SpinState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Settings.Model.Set(Consts.Buttons.StartButton.EnableField, false);
            Settings.Model.Set(Consts.Buttons.StopButton.EnableField, true);
        }

        [Bind(Consts.Buttons.StopButton.OnClick)]
        private void OnStopClick()
        {
            Parent.Change(Consts.State.StateStopping);
        }
    }
}