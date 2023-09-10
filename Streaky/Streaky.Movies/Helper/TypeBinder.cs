using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Streaky.Movies.Helper;

public class TypeBinder<T> : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var propertyName = bindingContext.ModelName;
        var supplierOfValues = bindingContext.ValueProvider.GetValue(propertyName);

        if (supplierOfValues == ValueProviderResult.None)
            return Task.CompletedTask;

        try
        {
            var deserializeValue = JsonConvert.DeserializeObject<T>(supplierOfValues.FirstValue);
            bindingContext.Result = ModelBindingResult.Success(deserializeValue);
        }
        catch
        {
            bindingContext.ModelState.TryAddModelError(propertyName, "Valor invalido para tipo List<int>");
        }

        return Task.CompletedTask;
    }
}

