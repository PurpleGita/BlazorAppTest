using BlazorAppTest;
using BlazorAppTest.Components.Pages;
using Bunit;
using Bunit.TestDoubles;
using System.Diagnostics;
using NSubstitute;
using BlazorAppTest.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
namespace BlazorAppTestUnit;

public class UnitTest1
{
    
    //skill issue
    //test login når den ikke er logget
    //test login når den er logget ind som bruger
    //test login når den er logget ind som admin

    UserManager<ApplicationUser> UserManager { get; set; }

    public UnitTest1()
    {
        UserManager = UserManagerHelper.CreateUserManager();
    }
    [Fact]
    public void TestLoginWhenNotLoggedIn()
    {

        //Arange
        using var ctx = new TestContext();
        var authContext = ctx.AddTestAuthorization();  
        ctx.Services.AddSingleton(UserManager);

        //Act
        var cut = ctx.RenderComponent<Home>();
        var select = cut.Find("#View");



        //Assert
        select.TextContent.MarkupMatches("you are NOT logged in, dummy");


    }


    [Fact]
    public void TestLoginWhenLoggedInUser()
    {
        //Arange
        using var ctx = new TestContext();
        var authContext = ctx.AddTestAuthorization();
        authContext.SetAuthorized("testUser");
        ctx.Services.AddSingleton(UserManager);

        //Act
        var cut = ctx.RenderComponent<Home>();
        var select = cut.Find("#View");



        //Assert
        select.TextContent.MarkupMatches("you are logged in!!!");

    }

    [Fact]
    public void TestLoginWhenLoggedInAdmin()
    {
        //Arange
        using var ctx = new TestContext();
        var authContext = ctx.AddTestAuthorization();
        authContext.SetAuthorized("testUser");
        authContext.SetRoles("Admin");
        ctx.Services.AddSingleton(UserManager);


        //Act
        var cut = ctx.RenderComponent<Home>();
        var select = cut.Find("#AdminView");


        //Assert
        select.TextContent.MarkupMatches("Btw you da Admin");

    }

    [Fact]
    public void TestLoginWhenNotLoggedInWithCode() 
    {
        //Arange
        using var ctx = new TestContext();
        var authContext = ctx.AddTestAuthorization();
        ctx.Services.AddSingleton(UserManager);
        

        //Act
        var cut = ctx.RenderComponent<Home>();


        //Assert
        Assert.False(cut.Instance._Authenticated);
        Assert.False(cut.Instance._IsAdmin);
    }


    [Fact]
    public void TestLoginWhenLoggedInWithCode()
    {
        //Arange
        using var ctx = new TestContext();
        var authContext = ctx.AddTestAuthorization();
        ctx.Services.AddSingleton(UserManager);
        authContext.SetAuthorized("testUser");

        //Act
        var cut = ctx.RenderComponent<Home>();


        //Assert
        Assert.True(cut.Instance._Authenticated);
        Assert.False(cut.Instance._IsAdmin);
    }

    [Fact]
    public void TestLoginWhenLoggedInAsAdminWithCode()
    {
        //Arange
        using var ctx = new TestContext();
        var authContext = ctx.AddTestAuthorization();
        ctx.Services.AddSingleton(UserManager);
        authContext.SetAuthorized("testUser");
        authContext.SetRoles("Admin");

        //Act
        var cut = ctx.RenderComponent<Home>();


        //Assert
        Assert.True(cut.Instance._Authenticated);
        Assert.True(cut.Instance._IsAdmin);
    }


    public static class UserManagerHelper
    {
        public static UserManager<ApplicationUser> CreateUserManager()
        {
            var store = Substitute.For<IUserStore<ApplicationUser>>();
            var options = Substitute.For<IOptions<IdentityOptions>>();
            var passwordHasher = Substitute.For<IPasswordHasher<ApplicationUser>>();
            var userValidators = new List<IUserValidator<ApplicationUser>> { Substitute.For<IUserValidator<ApplicationUser>>() };
            var passwordValidators = new List<IPasswordValidator<ApplicationUser>> { Substitute.For<IPasswordValidator<ApplicationUser>>() };
            var keyNormalizer = Substitute.For<ILookupNormalizer>();
            var errors = Substitute.For<IdentityErrorDescriber>();
            var services = Substitute.For<IServiceProvider>();
            var logger = Substitute.For<ILogger<UserManager<ApplicationUser>>>();

            return new UserManager<ApplicationUser>(store, options, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger);
        }
    }
}