using UnityEngine;
using TMPro;
using Zenject;

namespace Project
{
  public class ListViewItem_Controller : MonoBehaviour, IPoolable<string, IMemoryPool>
  {
    #region dependencies
    [SerializeField] TextMeshProUGUI _textUI;
    #endregion

    public void OnSpawned(string labelText, IMemoryPool pool)
    {
      _textUI.SetText(labelText);
      _pool = pool;
    }

    public void OnDespawned() => _pool = null;

    void OnDisable() => _pool?.Despawn(this);

    #region details
    IMemoryPool _pool;

    public class Factory : PlaceholderFactory<string, ListViewItem_Controller> { }
    #endregion
  }
}
