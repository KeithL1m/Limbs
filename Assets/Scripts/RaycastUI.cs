using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RaycastUI : MonoBehaviour
{
    private List<Player> _players;
    [SerializeField] private float _alphaEdit;
    [SerializeField] private List<HealthBar> _lastHealthBars;
    [SerializeField] private List<HealthBar> _currentHealthBars;
    [SerializeField] private GraphicRaycaster _raycaster;
    [SerializeField] private EventSystem _eventSystem;

    private void Awake()
    {
        GameLoader loader = ServiceLocator.Get<GameLoader>();
        loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _players = ServiceLocator.Get<PlayerManager>()._playerList;
    }

    private void Update()
    {
        for (int i = 0; i < _players.Count; i++)
        {
            var position = _players[i].transform.position;
            position = new Vector3(position.x, position.y, -10);

            var eventData = new PointerEventData(_eventSystem);
            eventData.position = position;

            var results = new List<RaycastResult>();
            _raycaster.Raycast(eventData, results);

            if (results.Where(r => r.gameObject.layer == 6).Count() > 0)
            {
                _currentHealthBars.Add(results[0].gameObject.GetComponent<HealthBar>());
            }
        }

        foreach(var bar in _currentHealthBars)
        {
            if (!_lastHealthBars.Contains(bar))
            {
                bar.SetColorAlpha(_alphaEdit);
            }
        }

        foreach(var bar in _lastHealthBars)
        {
            if (!_currentHealthBars.Contains(bar))
            {
                bar.SetColorAlpha(1.0f);
            }
        }
    }
}
