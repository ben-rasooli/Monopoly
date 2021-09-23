using Zenject;
using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Doozy.Engine.UI;
using System.Collections.Generic;

namespace Project.Tests
{
  public class BoardSpacesTests : ZenjectIntegrationTestFixture
  {
    void CommonInstall()
    {
      PreInstall();

      int jailLocationID = 4;
      Container.Bind<LandSpaceDetails>().FromScriptableObjectResource("Default Land Space Details").AsSingle();
      Container.Bind<RailRoadSpaceDetails>().FromScriptableObjectResource("Default RailRoad Space Details").AsSingle();
      Container.Bind<UIButton>().WithId("end turn button").FromNewComponentOnNewGameObject().AsCached();
      Container.Bind<UIButton>().WithId("purchase button").FromNewComponentOnNewGameObject().AsCached();
      Container.Bind<UIButton>().WithId("pay button").FromNewComponentOnNewGameObject().AsCached();
      Container.Bind<UIButton>().WithId("move button").FromNewComponentOnNewGameObject().AsCached();
      Container.Bind<UIButton>().WithId("finish game button").FromNewComponentOnNewGameObject().AsCached();
      Container.Bind<Player>().AsTransient();
      Container.BindInstance(jailLocationID).WithId("jail Location ID").AsSingle();

      Container.Bind<BoardSpace>().AsCached().WithArguments<int>(0); // GO space
      Container.Bind(typeof(BoardSpace), typeof(PropertySpace)).To<LandSpace>().AsCached().WithArguments<int>(1);
      Container.Bind(typeof(BoardSpace), typeof(PropertySpace)).To<RailRoadSpace>().AsCached().WithArguments<int>(2);
      Container.Bind(typeof(BoardSpace), typeof(PropertySpace)).To<LandSpace>().AsCached().WithArguments<int>(3);
      Container.Bind<BoardSpace>().AsCached().WithArguments<int>(jailLocationID); // jail
      Container.Bind(typeof(BoardSpace), typeof(PropertySpace)).To<LandSpace>().AsCached().WithArguments<int>(5);
      Container.Bind(typeof(BoardSpace), typeof(PropertySpace)).To<RailRoadSpace>().AsCached().WithArguments<int>(6);
      Container.Bind(typeof(BoardSpace), typeof(PropertySpace)).To<LandSpace>().AsCached().WithArguments<int>(7);
      Container.Bind<BoardSpace>().AsCached().WithArguments<int>(8); // parking
      Container.Bind<BoardSpace>().To<ChanceSpace>().AsCached().WithArguments<int>(9);
      Container.Bind(typeof(BoardSpace), typeof(PropertySpace)).To<RailRoadSpace>().AsCached().WithArguments<int>(10);
      Container.Bind<BoardSpace>().To<CommunityChestSpace>().AsCached().WithArguments<int>(11);
      Container.Bind<BoardSpace>().To<GoToJailSpace>().AsCached().WithArguments<int>(12);
      Container.Bind<BoardSpace>().To<TaxSpace>().AsCached().WithArguments<int>(13);
      Container.Bind(typeof(BoardSpace), typeof(PropertySpace)).To<RailRoadSpace>().AsCached().WithArguments<int>(14);
      Container.Bind<BoardSpace>().To<UtilitySpace>().AsCached().WithArguments<int>(15);

      Container.Bind<BoardManager>().AsSingle();
      Container.Bind<UIManager>().FromNewComponentOnNewGameObject().AsSingle();
      Container.Bind<GameManager>().AsSingle();
      Container.Inject(this);

      PostInstall();

      _player.Init();
      _UIManager.Configure(_player);
    }

    [Inject] BoardManager _boardManager;
    [Inject] List<BoardSpace> _boardSpaces;
    [Inject] UIManager _UIManager;
    [Inject] Player _player;

    [UnityTest]
    public IEnumerator Player_goes_to_jail_If_land_on_GoToJail_space()
    {
      CommonInstall();

      var dice = new Dice { Die_1 = 6, Die_2 = 6 };
      int jailLocationID = 4;

      _boardManager.Move(_player, dice);

      Assert.That(_player.LocationID, Is.EqualTo(jailLocationID));
      Assert.That(_player.IsInJail, Is.True);

      yield break;
    }

    [UnityTest]
    public IEnumerator Player_can_purchase_a_property_If_land_on_it_and_it_is_unowned()
    {
      CommonInstall();

      var dice = new Dice { Die_1 = 1, Die_2 = 2 };
      int initialwealth = _player.Wealth;
      UIButton purchaseButton = Container.ResolveId<UIButton>("purchase button");
      PropertySpace propertySpace = (PropertySpace)_boardSpaces[3];

      _boardManager.Move(_player, dice);

      Assert.That(_player.Wealth, Is.EqualTo(initialwealth));

      purchaseButton.Button.onClick.Invoke();

      Assert.That(_player.Wealth, Is.LessThan(initialwealth));
      Assert.That(propertySpace.Owner, Is.EqualTo(_player));

      yield break;
    }

    [UnityTest]
    public IEnumerator Player_should_pay_rent_If_land_on_someone_elses_property()
    {
      CommonInstall();

      var dice = new Dice { Die_1 = 1, Die_2 = 2 };
      var player_1 = _player;
      var player_2 = Container.Resolve<Player>();
      player_2.Init();
      int player_2_initialWealth = player_2.Wealth;
      UIButton purchaseButton = Container.ResolveId<UIButton>("purchase button");
      UIButton payButton = Container.ResolveId<UIButton>("pay button");

      _boardManager.Move(player_1, dice);
      purchaseButton.Button.onClick.Invoke(); // purchase property
      _UIManager.Configure(player_2);
      _boardManager.Move(player_2, dice);

      Assert.That(player_2.Wealth, Is.EqualTo(player_2_initialWealth));

      payButton.Button.onClick.Invoke();

      Assert.That(player_2.Wealth, Is.LessThan(player_2_initialWealth));

      yield break;
    }

    [UnityTest]
    public IEnumerator Player_should_pay_tax_If_land_on_Tax_space()
    {
      CommonInstall();

      var dice = new Dice { Die_1 = 6, Die_2 = 7 };
      int initialwealth = _player.Wealth;
      UIButton payButton = Container.ResolveId<UIButton>("pay button");

      _boardManager.Move(_player, dice);

      Assert.That(_player.Wealth, Is.EqualTo(initialwealth));

      payButton.Button.onClick.Invoke();

      Assert.That(_player.Wealth, Is.LessThan(initialwealth));
      yield break;
    }
  }
}