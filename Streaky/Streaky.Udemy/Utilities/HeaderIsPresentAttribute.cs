﻿using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Streaky.Udemy.Utilities;

public class HeaderIsPresentAttribute : Attribute, IActionConstraint
{
    private readonly string header;
    private readonly string value;

    public HeaderIsPresentAttribute(string header, string value)
    {
        this.header = header;
        this.value = value;
    }

    public int Order => 0;

    public bool Accept(ActionConstraintContext context)
    {
        var headers = context.RouteContext.HttpContext.Request.Headers;
        if (!headers.ContainsKey(header))
            return false;

        return string.Equals(headers[header], value, StringComparison.OrdinalIgnoreCase);
    }
}

