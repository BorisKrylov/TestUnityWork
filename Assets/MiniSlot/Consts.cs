namespace MiniSlot
{
    /// <summary>
    /// Основные константы FSM механики MiniSlot
    /// </summary>
    internal static class Consts
    {
        /// <summary>
        /// Название состояний
        /// </summary>
        public static class State
        {
            public const string StateInit = "Init";
            public const string StateIdle = "Idle";
            public const string StateStarting = "Starting";
            public const string StateSpin = "Spin";
            public const string StateStopping = "Stopping";
        }
        
        /// <summary>
        /// Поля модели FSM
        /// </summary>
        public static class ModelFields
        {
            /// <summary>
            /// Значение последний награды.
            /// Сейчас это индекс спрайта
            /// </summary>
            public const string LastReward = nameof(LastReward);
        }
        
        /// <summary>
        /// Названия событий в FSM
        /// </summary>
        public static class Events
        {
            /// <summary> Событие начала старта кручения слот машины </summary>
            public const string SpinStart = nameof(SpinStart);

            /// <summary> Начало события остановки слота </summary>
            public const string SpinStop = nameof(SpinStop);

            /// <summary> Окончание события остановки слота </summary>
            public const string SlotStopped = nameof(SlotStopped);

            /// <summary> Событие для запуска партиклов </summary>
            public const string PlayBurst = nameof(PlayBurst);

        }
        
        /// <summary>
        /// Констнаты кнопок
        /// </summary>
        public static class Buttons
        {
            /// <summary>
            /// Кнопка старта
            /// </summary>
            public static class StartButton
            {
                public const string EnableField = "BtnStartEnable";
                public const string OnClick = "OnStartClick";
            }
            
            /// <summary>
            /// Кнопка остановки
            /// </summary>
            public static class StopButton
            {
                public const string EnableField = "BtnStopEnable";
                public const string OnClick = "OnStopClick";
            }
        }
    }
}