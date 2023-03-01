using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SmartOpt.Modules.PatternLayoutsGenerator.UI.Services;

public class BusyIndicatorManager : INotifyPropertyChanged
{
    private static readonly object SyncRoot = new();
    private static BusyIndicatorManager instance;
    private readonly Dictionary<int, string> busyParameters;

    private bool isBusy;
    private string message;

    private BusyIndicatorManager()
    {
        isBusy = false;
        message = string.Empty;
        busyParameters = new Dictionary<int, string>();
    }

    public static BusyIndicatorManager Instance
    {
        get
        {
            lock (SyncRoot)
            {
                return instance ??= new BusyIndicatorManager();
            }
        }
    }

    public bool IsBusy
    {
        get => isBusy;
        private set
        {
            isBusy = value;
            OnPropertyChanged(nameof(IsBusy));
        }
    }

    public string Message
    {
        get => message;
        private set
        {
            message = value;
            OnPropertyChanged(nameof(Message));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void Show(int id, string busyMessage)
    {
        if (!busyParameters.ContainsKey(id))
        {
            busyParameters.Add(id, busyMessage);
            IsBusy = true;
            Message = busyMessage;
        }
        else
        {
            busyParameters[id] = busyMessage;
            IsBusy = true;
            Message = busyMessage;
        }
    }

    public void Close(int id)
    {
        if (busyParameters.ContainsKey(id))
        {
            busyParameters.Remove(id);
        }

        if (busyParameters.Count == 0)
        {
            IsBusy = false;
            Message = string.Empty;
        }
        else
        {
            IsBusy = true;
            Message = busyParameters.Last().Value;
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
