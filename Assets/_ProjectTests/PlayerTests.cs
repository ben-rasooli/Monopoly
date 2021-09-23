using NUnit.Framework;
using Zenject;

namespace Project.Tests
{
  [TestFixture]
  public class PlayerTests : ZenjectUnitTestFixture
  {
    [SetUp]
    public void CommonInstall()
    {
      Container.Bind<Player>().AsSingle();
      Container.Inject(this);
    }

    [Inject]
    Player _sut; // system under test

    [Test]
    public void Player_starts_the_game_with_1500_dollar()
    {
      _sut.Init();
      Assert.That(_sut.Wealth, Is.EqualTo(1500));
    }

    [Test]
    public void Player_starts_the_game_from_the_first_space_on_the_board()
    {
      _sut.Init();
      Assert.That(_sut.LocationID, Is.EqualTo(0));
    }
  }
}