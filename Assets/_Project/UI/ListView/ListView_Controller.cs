using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using Zenject;

namespace Project
{
  public class ListView_Controller : MonoBehaviour
  {
    public void Show(List<string> labelTexts, string titleText)
    {
      for (int i = labelTexts.Count - 1; i >= 0; i--)
        _itemFactory.Create(labelTexts[i]);

      _myPopup.Data.SetLabelsTexts(titleText);
      _myPopup.Show();
    }
    void Awake()
    {
      _myPopup.Overlay.RectTransform.gameObject.SetActive(false);
    }

    [SerializeField] UIPopup _myPopup;
    [Inject] ListViewItem_Controller.Factory _itemFactory;
  }
}
