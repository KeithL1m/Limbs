using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct PlayerData : IEquatable<PlayerData>
{
    public ulong clientId;

    public bool Equals(PlayerData other)
    {
        return clientId == other.clientId;
    }
}
