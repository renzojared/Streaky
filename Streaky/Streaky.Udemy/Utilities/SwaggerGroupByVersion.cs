using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Streaky.Udemy.Utilities;

public class SwaggerGroupByVersion : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        var namespaceController = controller.ControllerType.Namespace;
        var versionAPI = namespaceController.Split('.').Last().ToLower();
        controller.ApiExplorer.GroupName = versionAPI;
    }
}

