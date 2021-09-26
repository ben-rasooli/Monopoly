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
      UpdateSpaceDetailsUI();
      DisableEndTurnButton();
      DisablePurchaseButton();
      DisablePayButton();
      DisableMoveButton();
      _turnCountText.SetText($"Turn {_gameManager.TurnCount.ToString()}");
      _playerNameText.SetText(player.Name);
      UpdatePlayerWealth();
    }

    public void EnablePurchaseButton(Action<Player> purchaseAction, int price)
    {
      _purchaseButton.SetLabelText($"Purchase ${price}");
      _purchaseButton.EnableButton();
      _purchaseButton.Button.onClick.RemoveAllListeners();
      _purchaseButton.Button.onClick.AddListener(() =>
      {
        purchaseAction(_player);
        UpdatePlayerWealth();
      });
    }

    public void DisablePurchaseButton()
    {
      _purchaseButton.DisableButton();
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

    public void UpdateSpaceDetailsUI()
    {
      string spaceDetails = _boardManager.GetSpaceDetails(_player.LocationID);
      _spaceDetailsUI_controller.Show(spaceDetails);
    }

    public void ShowGetOutOfJailPopup(Action<Player> payAction, Action<Player> rollDiceAciton)
    {
      UIPopup popup = UIPopup.GetPopup("Get Out Of Jail");
      popup.Data.SetButtonsCallbacks(() => { payAction.Invoke(_player); }, () => { rollDiceAciton.Invoke(_player); });
      popup.Show();
    }

    public void OnPlayerStartMoving()
    {
      _controlsUI.Hide();
      _spaceDetailsUI_controller.Hide();
    }

    public void OnPlayerStopedMoving()
    {
      _controlsUI.Show();
      UpdateSpaceDetailsUI();
    }

    #region dependencies
    [Inject] GameManager _gameManager;
    [Inject] BoardManager _boardManager;
    [Inject] Jail _jail;
    [Inject(Id = "purchase button")] UIButton _purchaseButton;
    [Inject(Id = "pay button")] UIButton _payButton;
    [Inject(Id = "move button")] UIButton _moveButton;
    [SerializeField] UIView _controlsUI;
    [SerializeField] ListView_Controller _listView_Controller;
    [SerializeField] SpaceDetailsUI_Controller _spaceDetailsUI_controller;
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
      var tempList = ListPool<string>.Instance.Spawn();
      foreach (var asset in assets)
        tempList.Add(asset.Details);
      if (tempList.Count < 1)
        tempList.Add("Has no asset yet. Buy some properties.");
      _listView_Controller.Show(tempList, $"{_player.Name} Asset List");
      ListPool<string>.Instance.Despawn(tempList);
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
      var turnStatsList = _gameManager.TurnStatsList;
      var tempList = ListPool<string>.Instance.Spawn();
      foreach (TurnStats stats in turnStatsList)
        tempList.Add(stats.ToString());
      _listView_Controller.Show(tempList, $"History of turns");
      ListPool<string>.Instance.Despawn(tempList);
    }
    #endregion
  }
}
