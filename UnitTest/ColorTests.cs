using Narumikazuchi.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media;

namespace UnitTest
{
    [TestClass]
    public class ColorTests
    {
        [TestMethod]
        public void RgbToHslTest()
        {
            Color rgb = Color.FromRgb(128, 64, 255);
            HslColor hsl = (HslColor)rgb;
            Assert.AreEqual(rgb, (Color)hsl);
        }

        [TestMethod]
        public void RgbToHsvTest()
        {
            Color rgb = Color.FromRgb(128, 64, 255);
            HsvColor hsv = (HsvColor)rgb;
            Assert.AreEqual(rgb, (Color)hsv);
        }
    }
}
