using System;
using Vjb.Sp.BackupLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vjb.Sp.BackupLibrary.UnitTest.Utilities
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void IsValidUrl_ValidHttpsUrl_ReturnsTrue()
        {
            // Arrange
            var testUrl = "https://testsite.sharepoint.com";

            // Act
            var result = Vjb.Sp.BackupLibrary.Utilities.Utilities.IsValidUrl(testUrl);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidUrl_ValidHttpUrl_ReturnsTrue()
        {
            // Arrange
            var testUrl = "http://testsite.sharepoint.com";

            // Act
            var result = Vjb.Sp.BackupLibrary.Utilities.Utilities.IsValidUrl(testUrl);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidUrl_UrlMissingHttp_ReturnsFalse()
        {
            // Arrange
            var testUrl = "testsite.sharepoint.com";

            // Act
            var result = Vjb.Sp.BackupLibrary.Utilities.Utilities.IsValidUrl(testUrl);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidUrl_InValidUrl_ReturnsFalse()
        {
            // Arrange
            var testUrl = "com";

            // Act
            var result = Vjb.Sp.BackupLibrary.Utilities.Utilities.IsValidUrl(testUrl);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
