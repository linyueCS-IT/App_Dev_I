using Math_Test;
namespace TestMathUtils
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            double a = 1;
            double b = 2;

            MathUtils mathUtils = new MathUtils();

            // Act
            double result = mathUtils.GetAverage(a, b);

            // Assert

            Assert.Equal((a + b) / 2, result);
        }

        [Fact]
        public void Test2()
        {
            // Arrange
            double a = 1.2;
            double b = -2;

            MathUtils mathUtils = new MathUtils();

            // Act
            double result = mathUtils.GetAverage(a,b);

            // Assert

            Assert.Equal(result, result);

            //Assert.Fail();

            Assert.Throws(typeof(Exception),() => mathUtils.GetAverage(a,b));
        }
    }
}