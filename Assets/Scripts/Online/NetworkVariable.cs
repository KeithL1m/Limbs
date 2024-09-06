using Unity.Netcode;

public class NetVar<T>
{
    private object _variable;

    public NetVar(T networkVariable, bool isOnline)
    {
        if (isOnline)
        {
            _variable = new NetworkVariable<T>(networkVariable);
        }
        else
        {
            _variable = networkVariable;
        }
    }

    public void SetValue(T value)
    {
        if (_variable is NetworkVariable<T>)
        {
            SetValueServerRpc(value);
        }
        else
        {
            _variable = value;
        }
    }

    public T GetValue()
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

    [ServerRpc]
    private void SetValueServerRpc(T value)
    {
        var networkValue = _variable as NetworkVariable<T>;
        networkValue.Value = value;
    }
}
