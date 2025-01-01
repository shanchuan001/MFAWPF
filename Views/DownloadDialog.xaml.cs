using MFAWPF.Utils;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace MFAWPF.Views;

public partial class DownloadDialog 
{
    public DownloadDialog(string title)
    {
        Title = title;
        InitializeComponent();
    }
    
    public void UpdateProgress(double progressPercentage)
    {
        Dispatcher.Invoke(() =>
        {
            ProgressBar.Value = progressPercentage;
        });
    }

    public void SetText(string text)
    {
        Dispatcher.Invoke(() =>
        {
            TextBlock.Text = text;
        });
    }
    
    private void Restart(object sender, RoutedEventArgs e)
    {
        Process.Start(Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty);
        Growls.Process(Application.Current.Shutdown);
    }
}

