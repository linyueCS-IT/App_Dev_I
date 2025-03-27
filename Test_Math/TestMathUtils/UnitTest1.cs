using Math_Test;
using NuGet.Frameworks;
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
            double b = 1.2;

            MathUtils mathUtils = new MathUtils();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => mathUtils.GetAverage(a, b));
        }
        [Fact]
        public void Test3()
        {
            // Arrange
            double a = 2.0;
            double b = 4.0;
            MathUtils mathUtils = new MathUtils();

            // Act
            double result = mathUtils.GetAverage(a, b);

            // Assert
            Assert.NotEqual(2.0, result);

        }
        [Fact]
        public void Test4() 
        { 
            // Arrange
            MathUtils mathUtils = new MathUtils();

            // Act & Assert            
            Assert.IsType<MathUtils>(mathUtils);        
        }

        [Fact]
        public void Test5()
        {
            // Arrange 
            MathUtils mathUtils = new MathUtils();

            // Act
            bool result = mathUtils.IsEven(4);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Test6()
        {
            // Arrange 
            MathUtils mathUtils = new MathUtils();

            // Act
            bool result = mathUtils.IsEven(5);

            // Assert
            Assert.False(result);
        }
    }
}