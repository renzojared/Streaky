using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Streaky.Udemy.Controllers.V1;
using Streaky.Udemy.Tests.Mocks;

namespace Streaky.Udemy.Tests.UnitTests;

[TestClass]
public class RootControllerTests
{
    [TestMethod]
    public async Task ifUserIsAdminGet4Links()
    {
        //Preparacion
        var authorizationService = new AuthorizationServiceMock();
        authorizationService.Result = AuthorizationResult.Success();
        var rootController = new RouteController(authorizationService);
        rootController.Url = new URLHelperMock();
        //Ejecucion

        var result = await rootController.Get();

        //Verificacion
        Assert.AreEqual(4, result.Value.Count());
    }

    [TestMethod]
    public async Task ifUserNotIsAdminGet2Links()
    {
        //Preparacion
        var authorizationService = new AuthorizationServiceMock();
        authorizationService.Result = AuthorizationResult.Failed();
        var rootController = new RouteController(authorizationService);
        rootController.Url = new URLHelperMock();
        //Ejecucion

        var result = await rootController.Get();

        //Verificacion
        Assert.AreEqual(2, result.Value.Count());
    }

    [TestMethod]
    public async Task ifUserNotIsAdminGet2LinksUseMoq()
    {
        //Preparacion
        var moq = new Mock<IAuthorizationService>();
        moq.Setup(s => s.AuthorizeAsync(
            It.IsAny<ClaimsPrincipal>(),
            It.IsAny<object>(),
            It.IsAny<IEnumerable<IAuthorizationRequirement>>()
            )).Returns(Task.FromResult(AuthorizationResult.Failed()));

        moq.Setup(s => s.AuthorizeAsync(
            It.IsAny<ClaimsPrincipal>(),
            It.IsAny<object>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(AuthorizationResult.Failed()));

        var moqHelper = new Mock<IUrlHelper>();
        moqHelper.Setup(s => s.Link(
            It.IsAny<string>(),
            It.IsAny<object>())).Returns(string.Empty);

        var rootController = new RouteController(moq.Object);
        rootController.Url = new URLHelperMock();
        //Ejecucion

        var result = await rootController.Get();

        //Verificacion
        Assert.AreEqual(2, result.Value.Count());
    }
}

