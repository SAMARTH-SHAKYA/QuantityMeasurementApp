namespace QuantityMeasurementApp.App
{
    /// <summary>
    /// Contract for the interactive console menu.
    /// </summary>
    public interface IMenu
    {
        /// <summary>
        /// Starts the interactive menu loop until the user exits.
        /// </summary>
        void Run();
    }
}
