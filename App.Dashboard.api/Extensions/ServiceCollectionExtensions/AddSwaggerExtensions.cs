namespace App.Dashboard.api.Extensions.ServiceCollectionExtensions;

public static class AddSwaggerExtensions
{
    public static IServiceCollection AddAppSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("LEXT", new() { Title = "LEXT APIs", Version = "v1" });

            c.SwaggerDoc("Mobile", new()
            {
                Title = "LEXT Mobile API",
                Version = "v1"
            });

            c.DocInclusionPredicate((docName, apiDesc) =>
            {
                var group = apiDesc.GroupName;
                if (!string.IsNullOrEmpty(group))
                    return string.Equals(group, docName, StringComparison.OrdinalIgnoreCase);
                return docName.Equals("LEXT", StringComparison.OrdinalIgnoreCase);
            });

            //c.TagActionsBy(api => new[] { api.GroupName ?? "LEXT" });
            c.TagActionsBy(api =>
            {
                var controllerName = api.ActionDescriptor.RouteValues["controller"];
                return new[] { controllerName ?? "Default" };
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Add your valid token."
            });

            c.AddSecurityDefinition("language", new OpenApiSecurityScheme
            {
                Name = "language",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "language",
                In = ParameterLocation.Header,
                Description = "Add your language."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() }
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "language" } }, Array.Empty<string>() }
            });

            var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xml);
            if (File.Exists(xmlPath))
                c.IncludeXmlComments(xmlPath);
        });

        return services;
    }
}
