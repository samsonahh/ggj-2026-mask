using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PlayerKnockdownUI : MonoBehaviour
{
    [SerializeField, Required] private PlayerKnockdownManager _knockdownManager;
    [SerializeField, Required] private Image _knockdownIconPrefab;
    [SerializeField, Required] private Transform _iconParent;
    [SerializeField] private Color _iconColor = Color.white;
    [SerializeField, ReadOnly] private List<Image> _currentKnockdownIcons = new();
    private int _prevKnockdowns;

    private void Update()
    {
        // poll for change in knockdowns
        int remainingKnockdowns = GetRemainingKnockdowns();
        if (remainingKnockdowns != _prevKnockdowns)
        {
            _prevKnockdowns = remainingKnockdowns;
            OnKnockdownCountChanged(remainingKnockdowns);
        }
    }

    private void OnKnockdownCountChanged(int knockdownCount)
    {
        // Remove all icons
        foreach (Image icon in _currentKnockdownIcons)
        {
            if (icon == null)
                continue;
            Destroy(icon.gameObject);
        }
        _currentKnockdownIcons.Clear();

        // Add new count back
        for (int i = 0; i < knockdownCount; i++)
        {
            Image icon = Instantiate(_knockdownIconPrefab, _iconParent);
            icon.color = _iconColor;
            _currentKnockdownIcons.Add(icon);
        }
    }
    
    private int GetRemainingKnockdowns() => _knockdownManager.MaxKnockdowns + 1 - _knockdownManager.CurrentKnockdowns;
}