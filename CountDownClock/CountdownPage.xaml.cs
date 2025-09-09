using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CountDownClock;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class CountdownPage : Page
{
    #region Private Variable

    private Timer _timer;
    private int _totalSeconds;
    private int _remainingSeconds;
    private bool _isRunning;

    #endregion

    #region Construction

    public CountdownPage()
    {
        InitializeComponent();
        _isRunning = false;
        UpdateTimeDisplay();
    }

    #endregion

    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
        if (!_isRunning)
        {
            StartCountdown();
        }
        else
        {
            StopCountdown();
        }
    }

    #region Timer Control Methods

    private void StartCountdown()
    {
        var hours = (int)HoursInput.Value;
        var minutes = (int)MinutesInput.Value;
        var seconds = (int)SecondsInput.Value;

        _totalSeconds = hours * 3600 + minutes * 60 + seconds;
        _remainingSeconds = _totalSeconds;

        if (_totalSeconds <= 0)
        {
            StatusText.Text = "Please enter valid time!";
            return;
        }

        _isRunning = true;

        HoursInput.IsEnabled = false;
        MinutesInput.IsEnabled = false;
        SecondsInput.IsEnabled = false;

        StartButton.Content = "Stop Timer";

        StatusText.Text = "Timer Begin...";

        _timer = new Timer(TimerTick, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

        UpdateTimeDisplay();
    }

    private void StopCountdown()
    {
        _timer?.Dispose();
        _timer = null;

        _isRunning = false;

        HoursInput.IsEnabled = true;
        MinutesInput.IsEnabled = true;
        SecondsInput.IsEnabled = true;

        StartButton.Content = "Start";
        StatusText.Text = "Timer ended";
    }

    #endregion

    #region Timer Process

    private void TimerTick(object state)
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            _remainingSeconds--;

            UpdateTimeDisplay();

            if (_remainingSeconds <= 0)
            {
                CountdownFinished();
            }
        });
    }

    private void CountdownFinished()
    {
        _timer?.Dispose();
        _timer = null;

        _isRunning = false;

        HoursInput.IsEnabled = true;
        MinutesInput.IsEnabled = true;
        SecondsInput.IsEnabled = true;

        StartButton.Content = "Start";
        StatusText.Text = "Time out!";
        TimeDisplay.Text = "00:00:00";
    }

    #endregion

    #region UI Update Methods

    private void UpdateTimeDisplay()
    {
        if (_isRunning)
        {
            var hours = _remainingSeconds / 3600;
            var minutes = (_remainingSeconds % 3600) / 60;
            var seconds = _remainingSeconds % 60;

            TimeDisplay.Text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }
        else
        {
            var hours = (int)HoursInput.Value;
            var minutes = (int)MinutesInput.Value;
            var seconds = (int)SecondsInput.Value;

            TimeDisplay.Text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }
    }

    #endregion

    #region Cleanup

    private void Page_Unloaded(object sender, RoutedEventArgs e)
    {
        _timer?.Dispose();
        _timer = null;
    }

    #endregion
}
