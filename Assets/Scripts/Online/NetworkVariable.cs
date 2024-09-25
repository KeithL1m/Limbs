using Unity.Netcode;

public delegate void ChangeValueEvent<T>(T oldValue, T newValue);

public class NetVar<T>
{
    private event ChangeValueEvent<T> _changeValueEventFn;

    private object _variable;

    public T Value
    {
        get
        {
            if (_variable is NetworkVariable<T>)
            {
                var networkValue = _variable as NetworkVariable<T>;
                return networkValue.Value;
            }
            else
            {
                return (T)_variable;
            }
        }
        set
        {
            if (_variable is NetworkVariable<T>)
            {
                SetValueServerRpc(value);
            }
            else
            {
                T tempValue = (T)_variable;
                _variable = value;
                _changeValueEventFn?.Invoke(tempValue, (T)_variable);
            }
        }
    }


    public NetVar(T networkVariable, bool isOnline)
    {
        if (isOnline)
        {
            _variable = new NetworkVariable<T>(networkVariable);
            var networkValue = _variable as NetworkVariable<T>;
            networkValue.OnValueChanged += OnValueChange;
        }
        else
        {
            _variable = networkVariable;
        }
    }

    public void AddEvent(ChangeValueEvent<T> newEvent)
    {
        _changeValueEventFn += newEvent;
    }

    [ServerRpc]
    private void SetValueServerRpc(T value)
    {
        var networkValue = _variable as NetworkVariable<T>;
        networkValue.Value = value;
    }

    private void OnValueChange(T oldValue, T newValue)
    {
        _changeValueEventFn?.Invoke(oldValue, newValue);
    }
}
