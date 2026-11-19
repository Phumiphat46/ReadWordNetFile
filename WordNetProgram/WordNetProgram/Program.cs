using WordNetProgram.Test_Nested_Drop_Down;
namespace WordNetProgram
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run (new Nested_Drop_Down());
            Application.Run(new modified_111126_Parent_Child_Of_Noun());
            Application.Run(new MainMenu());
        }
    }
}