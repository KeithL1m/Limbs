using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RaycastUI : MonoBehaviour
{
    private List<GameObject> _players;
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
        _players = ServiceLocator.Get<PlayerManager>().GetPlayerObjects();
    }

    private void Update()
    {
        for (int i = 0; i < _players.Count; i++)
        {
            var position = _players[i].transform.position;
            position = new Vector3(position.x, position.y - 0.8f, -10);

            RaycastHit2D hit = Physics2D.Raycast(position, _players[i].transform.forward, 20f);

            if (hit.transform.tag == "HealthBar")
            {
                Debug.Log("Hit a health bar");
                _currentHealthBars.Add(hit.transform.gameObject.GetComponent<HealthBar>());
            }
        }

        foreach(var bar in _currentHealthBars)
        {
            if (!bar.Translucent)
            {
                Debug.Log("making translucent");
                bar.SetColorAlpha(_alphaEdit);
                bar.Translucent = true;
            }
        }

        foreach(var bar in _lastHealthBars)
        {
            if (!_currentHealthBars.Contains(bar))
            {
                Debug.Log("making opaque");
                bar.SetColorAlpha(1.0f);
                bar.Translucent = false;
            }
        }

        _lastHealthBars.Clear();
        _lastHealthBars.AddRange(_currentHealthBars);
        _currentHealthBars.Clear();
    }
}
