using NUnit.Framework;
using Zenject;

namespace Project.Tests
{
  [TestFixture]
  public class BoardManagerTests : ZenjectUnitTestFixture
  {
    [SetUp]
    public void CommonInstall()
    {
      Container.Bind<Player>().AsTransient();
      for (int i = 0; i < 40; i++)
        Container.Bind<BoardSpace>().AsCached().WithArguments<int>(i);
      Container.Bind<BoardManager>().AsCached();
      Container.Bind<UIManager>().FromSubstitute().AsSingle();
      Container.Inject(this);
    }

    [Inject] BoardManager _sut;
    [Inject] Player _player;

    [Test]
    public void Player_moves_on_the_board_according_to_dice()
    {
      var dice = new Dice { Die_1 = 1, Die_2 = 2 };

      _player.Init();
      Assert.That(_player.LocationID, Is.EqualTo(0));

      _sut.Move(_player, dice);

      Assert.That(_player.LocationID, Is.EqualTo(3));
    }

    [Test]
    public void Player_wraps_around_when_reaches_last_space()
    {
      var dice = new Dice { Die_1 = 1, Die_2 = 2 };

      _player.LocationID = 37;
      _sut.Move(_player, dice);

      Assert.That(_player.LocationID, Is.EqualTo(0));

      _player.LocationID = 38;
      _sut.Move(_player, dice);

      Assert.That(_player.LocationID, Is.EqualTo(1));
    }

    [Test]
    public void Player_receives_S200_when_passing_GO_space()
    {
      var dice = new Dice { Die_1 = 1, Die_2 = 2 };

      _player.Init();
      Assert.That(_player.Wealth, Is.EqualTo(1500));

      _player.LocationID = 37;
      _sut.Move(_player, dice);

      Assert.That(_player.Wealth, Is.EqualTo(1700));
    }
  }
}