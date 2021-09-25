using System;
using Doozy.Engine.UI;
using UnityEngine;
using Zenject;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Project
{
  public class UIManager_Controller : MonoBehaviour
  {
    public void Configure(Player player)
    {
      _player = player;
      ShowMessage(player.Name);
      DisableEndTurnButton();
      DisablePurchaseButton();
      DisablePayButton();
      DisableMoveButton();
      _turnCountText.SetText($"Turn {_gameManager.TurnCount.ToString()}");
      _playerNameText.SetText(player.Name);
      UpdatePlayerWealth();
    }

    public void EnablePurchaseButton(Action<Player> purchaseAction, LandSpaceDetails details)
    {
      enablePurchaseButton(purchaseAction, details.Price, details.ToString());
    }

    public void EnablePurchaseButton(Action<Player> purchaseAction, RailRoadSpaceDetails details)
    {
      enablePurchaseButton(purchaseAction, details.Price, details.ToString());
    }

    public void DisablePurchaseButton()
    {
      _purchaseButton.DisableButton();
      _propertyDetailsUI.Hide();
    }

    public void EnablePayButton(Action<Player, Player> payAction, Player owner, int payAmount)
    {
      enablePayButton(() =>
        {
          payAction(_player, owner);
          _payButton.SetLabelText("Pay");
          UpdatePlayerWealth();
        }, payAmount);
    }

    public void EnablePayButton(Action<Player> payAction, int payAmount)
    {
      enablePayButton(() =>
      {
        payAction(_player);
        _payButton.SetLabelText("Pay");
        UpdatePlayerWealth();
      }, payAmount);
    }

    public void DisablePayButton()
    {
      _payButton.DisableButton();
    }

    public void EnableMoveButton()
    {
      _moveButton.EnableButton();
      _getOutOfJailButton.DisableButton();
    }

    public void DisableMoveButton()
    {
      _moveButton.DisableButton();
    }

    public void EnableEndTurnButton()
    {
      _endTurnButton.EnableButton();
    }

    public void DisableEndTurnButton()
    {
      _endTurnButton.DisableButton();
    }

    public void ShowMessage(string message)
    {
      UIPopup popup = UIPopup.GetPopup("Message Popup");
      popup.Data.SetLabelsTexts(message);
      UIPopupManager.ShowPopup(popup, true, false);
    }

    public void ShowError(string message)
    {
      UIPopup popup = UIPopup.GetPopup("Error Popup");
      popup.Data.SetLabelsTexts(message);
      UIPopupManager.ShowPopup(popup, true, false);
    }

    public void UpdatePlayerWealth()
    {
      _playerWealthText.SetText(_player.Wealth.ToString());
    }

    public void ShowGetOutOfJailPopup(Action<Player> payAction, Action<Player> rollDiceAciton)
    {
      UIPopup popup = UIPopup.GetPopup("Get Out Of Jail");
      popup.Data.SetButtonsCallbacks(() => { payAction.Invoke(_player); }, () => { rollDiceAciton.Invoke(_player); });
      popup.Show();
    }

    #region dependencies
    [Inject] GameManager _gameManager;
    [Inject] BoardManager _boardManager;
    [Inject] Jail _jail;
    [Inject(Id = "purchase button")] UIButton _purchaseButton;
    [Inject(Id = "pay button")] UIButton _payButton;
    [Inject(Id = "move button")] UIButton _moveButton;
    [SerializeField] ListView_Controller _listView_Controller;
    [SerializeField] PropertyDetails_Controller _propertyDetailsUI;
    [SerializeField] UIButton _endTurnButton;
    [SerializeField] UIButton _assetsButton;
    [SerializeField] UIButton _getOutOfJailButton;
    [SerializeField] UIButton _finishGameButton;
    [SerializeField] TextMeshProUGUI _turnCountText;
    [SerializeField] TextMeshProUGUI _playerNameText;
    [SerializeField] TextMeshProUGUI _playerWealthText;
    #endregion

    void OnEnable()
    {
      _endTurnButton.Button.onClick.AddListener(onEndTurn);
      _assetsButton.Button.onClick.AddListener(onShowAssets);
      _moveButton.Button.onClick.AddListener(onMove);
      _getOutOfJailButton.Button.onClick.AddListener(onGetOutOfJail);
      _finishGameButton.Button.onClick.AddListener(onFinishGame);
    }

    private void OnDisable()
    {
      _endTurnButton.Button.onClick.RemoveAllListeners();
      _assetsButton.Button.onClick.RemoveAllListeners();
      _moveButton.Button.onClick.RemoveAllListeners();
      _getOutOfJailButton.Button.onClick.RemoveAllListeners();
      _finishGameButton.Button.onClick.RemoveAllListeners();
    }

    #region details
    Player _player;

    void enablePurchaseButton(Action<Player> purchaseAction, int price, string propertyDetails)
    {
      _purchaseButton.SetLabelText($"Purchase ${price}");
      _purchaseButton.EnableButton();
      _purchaseButton.Button.onClick.RemoveAllListeners();
      _purchaseButton.Button.onClick.AddListener(() =>
      {
        purchaseAction(_player);
        UpdatePlayerWealth();
      });
      _propertyDetailsUI.Show(propertyDetails);
    }

    void enablePayButton(UnityAction payAction, int payAmount)
    {
      DisableEndTurnButton();
      _payButton.SetLabelText($"Pay ${payAmount}");
      _payButton.EnableButton();
      _payButton.Button.onClick.RemoveAllListeners();
      _payButton.Button.onClick.AddListener(payAction);
    }

    void onEndTurn()
    {
      DisableEndTurnButton();
      _gameManager.EndTurn();
    }

    void onShowAssets()
    {
      var assets = _boardManager.GetPlayerAssets(_player);
      List<string> data = new List<string> { "one", "two", "three", "four" };
      _listView_Controller.Show(data, $"{_player.Name} Asset List");
      // configure the Player assets menu
      // show the menu
    }

    void onMove()
    {
      _gameManager.Move(out bool didRollDouble);
    }

    void onGetOutOfJail()
    {
      _jail.ShowGetOutOfJail(_player);
    }

    void onFinishGame()
    {
      _gameManager.FinishGame();
    }
    #endregion
  }
}
