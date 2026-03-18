using MobileChargingStation.Lib.Boundary;
using NUnit.Framework;

namespace MobileChargingStation.Lib.Test.Boundary;

[TestFixture]
public class TestDisplay
{
    private IDisplay _uut;

    [SetUp]
    public void Setup()
    {
        _uut = new Display();
        
    }

    [Test]
    public void ShowMessage_ConsoleWritesMessage()
    {
        // Arrange
       
        StringWriter writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        _uut.ShowMessage("Hello");


        //assert
        Assert.That(writer.ToString(), Does.Contain("Hello"));
    }
}