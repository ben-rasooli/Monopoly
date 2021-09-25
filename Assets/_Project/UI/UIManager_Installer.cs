using Doozy.Engine.UI;
using UnityEngine;
using Zenject;
using TMPro;

namespace Project
{
  public class UIManager_Installer : MonoInstaller
  {
    [SerializeField] UIButton _moveButton;
    [SerializeField] UIButton _payButton;
    [SerializeField] UIButton _purchaseButton;
    [SerializeField] GameObject _listViewItem_Prefab;
    [SerializeField] Transform _listView_Transform;

    public override void InstallBindings()
    {
      Container.BindInstance<UIButton>(_purchaseButton).WithId("purchase button").AsCached();
      Container.BindInstance<UIButton>(_payButton).WithId("pay button").AsCached();
      Container.BindInstance<UIButton>(_moveButton).WithId("move button").AsCached();
      Container.BindFactory<string, ListViewItem_Controller, ListViewItem_Controller.Factory>()
                .FromPoolableMemoryPool<string, ListViewItem_Controller, ListViewItem_ControllerPool>(poolBinder => poolBinder
                    .WithInitialSize(25)
                    .FromComponentInNewPrefab(_listViewItem_Prefab)
                    .UnderTransform(_listView_Transform));
    }
  }

  class ListViewItem_ControllerPool : MonoPoolableMemoryPool<string, IMemoryPool, ListViewItem_Controller> { }
}