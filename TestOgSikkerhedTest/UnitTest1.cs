using Bunit;
using Bunit.TestDoubles;
using System.ComponentModel.DataAnnotations;
using TestOgSikkerhed.Components.Pages;
using TestOgSikkerhed.Migrations;
using static TestOgSikkerhed.Components.Account.Pages.Register;

namespace TestOgSikkerhedTest
{
    public class UnitTest1
    {
        [Fact]
        public void AuthenticatedTest()
        {
            // Arrange
            using var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetAuthorized("");

            // Act
            var cut = ctx.RenderComponent<Home>();

            // Assert
            cut.MarkupMatches("<h1>Hello, world!</h1><div>\r\n Welcome to your new app. You are logged in\r\n</div>");
        }

        [Fact]
        public void NotAuthenticatedTest()
        {
            // Arrange
            using var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetNotAuthorized();

            // Act
            var cut = ctx.RenderComponent<Home>();

            // Assert
            cut.MarkupMatches("<h1>Hello, world!</h1><div>\r\n Welcome to your new app. You are NOT logged in\r\n</div>");
        }

        [Fact]
        public void AuthenticatedTest_InCode()
        {
            // Arrange
            using var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetAuthorized("");

            // Act
            var cut = ctx.RenderComponent<Home>();
            var inst = cut.Instance;

            // Assert
            Assert.Equal(true, inst._isAuthenticated);
        }

        [Fact]
        public void NotAuthenticatedTest_InCode()
        {
            // Arrange
            using var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetNotAuthorized();

            // Act
            var cut = ctx.RenderComponent<Home>();
            var inst = cut.Instance;

            // Assert
            Assert.Equal(false, inst._isAuthenticated);
        }

        [Fact]
        public void IsAdminTest()
        {
            // Arrange
            using var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetAuthorized("");
            authContext.SetRoles("Admin");

            // Act
            var cut = ctx.RenderComponent<Home>();

            // Assert
            cut.MarkupMatches("<h1>Hello, world!</h1>\r\n" +
                "<div>\r\n  Welcome to your new app. You are logged in\r\n</div>" +
                "\r\n<p>\r\n  You are admin\r\n</p>");
        }

        [Fact]
        public void IsNotAdminTest()
        {
            // Arrange
            using var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetAuthorized("");
            authContext.SetRoles("");

            // Act
            var cut = ctx.RenderComponent<Home>();

            // Assert
            cut.MarkupMatches("<h1>Hello, world!</h1>\r\n" +
                "<div>\r\n  Welcome to your new app. You are logged in\r\n</div>");
        }

        [Fact]
        public void IsAdminTest_InCode()
        {
            // Arrange
            using var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetAuthorized("");
            authContext.SetRoles("Admin");

            // Act
            var cut = ctx.RenderComponent<Home>();
            var inst = cut.Instance;

            // Assert
            Assert.Equal(true, inst._isAdmin);
        }

        [Fact]
        public void IsNotAdminTest_InCode()
        {
            // Arrange
            using var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetAuthorized("");
            authContext.SetRoles("");

            // Act
            var cut = ctx.RenderComponent<Home>();
            var inst = cut.Instance;

            // Assert
            Assert.Equal(false, inst._isAdmin);
        }

        private static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }

        [Theory]
        [InlineData("Weak1!", false)]        // For kort password
        [InlineData("NoNumberOrSpecial", false)] // Mangler tal og specialtegn
        [InlineData("ValidP@ssw0rd", true)]  // Gyldigt password
        [InlineData("", false)]              // Mangler password
        public void PasswordValidation_Should_Work_Correctly(string password, bool expected)
        {
            // Arrange
            var model = new InputModel
            {
                Email = "test@test.com",
                Password = password,
                ConfirmPassword = password
            };

            // Act
            var results = ValidateModel(model);
            bool isValid = results.Count == 0;

            // Assert
            Assert.Equal(expected, isValid);
        }

        [Fact]
        public void ConfirmPassword_Should_Match_Password()
        {
            // Arrange
            var model = new InputModel
            {
                Email = "test@test.com",
                Password = "ValidP@ssw0rd",
                ConfirmPassword = "DifferentP@ssw0rd"
            };

            // Act
            var results = ValidateModel(model);

            // Assert
            Assert.Contains(results, v => v.ErrorMessage == "The password and confirmation password do not match.");
        }
    }
}

