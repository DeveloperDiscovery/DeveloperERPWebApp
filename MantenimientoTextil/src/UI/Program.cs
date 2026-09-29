namespace MantenimientoTextil.UI;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        var form = new Form { Text = "Mantenimiento Industrial Textil", Width = 900, Height = 600 };
        form.Controls.Add(new Label { Text = "Andamiaje inicial", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter });
        Application.Run(form);
    }
}
