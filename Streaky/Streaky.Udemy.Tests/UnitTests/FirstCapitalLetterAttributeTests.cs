using System.ComponentModel.DataAnnotations;
using Streaky.Udemy.Validator;

namespace Streaky.Udemy.Tests.UnitTests;

[TestClass]
public class FirstCapitalLetterAttributeTests
{
    [TestMethod]
    public void FirstLetterLowerReturnError()
    {
        var firstLetterUpper = new FirstCapitalLetterAttribute();
        var value = "renzo";
        var valContext = new ValidationContext(new { Name = value });

        var result = firstLetterUpper.GetValidationResult(value, valContext);

        Assert.AreEqual("La primera letra debe ser mayuscula.", result.ErrorMessage);
    }

    [TestMethod]
    public void NullValueNotReturnError()
    {
        var firstLetterUpper = new FirstCapitalLetterAttribute();
        string value = null;
        var valContext = new ValidationContext(new { Name = value });

        var result = firstLetterUpper.GetValidationResult(value, valContext);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void ValueWithFirstLetterUpperNotReturnError()
    {
        var firstLetterUpper = new FirstCapitalLetterAttribute();
        string value = "Renzo";
        var valContext = new ValidationContext(new { Name = value });

        var result = firstLetterUpper.GetValidationResult(value, valContext);

        Assert.IsNull(result);
    }
}
