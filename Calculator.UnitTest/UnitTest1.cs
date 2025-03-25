using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalculatorApp;

namespace CalculatorTests
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void Add_TwoPositiveNumbers_ReturnsCorrectSum()
        {
            // Arrange
            var calculator = new Calculator();
            int a = 5;
            int b = 7;

            // Act
            int result = calculator.Add(a, b);

            // Assert
            Assert.AreEqual(12, result);
        }

        [TestMethod]
        public void Add_PositiveAndNegativeNumbers_ReturnsCorrectSum()
        {
            // Arrange
            var calculator = new Calculator();
            int a = 10;
            int b = -3;

            // Act
            int result = calculator.Add(a, b);

            // Assert
            Assert.AreEqual(7, result);

        }
    }
}