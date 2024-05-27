using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Routing.Constraints;
using System.Collections.Generic;

using K2.WebAPI;
using Swagger.Net.Application;
using Swagger.Net;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace K2.WebAPI
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        
                        c.SingleApiVersion("v1", "K2.WebAPI");
                        c.AccessControlAllowOrigin("*");
                       /* c.IncludeXmlComments(string.Format(@"{0}\bin\SwaggerK2.WebAPI.XML", System.AppDomain.CurrentDomain.BaseDirectory));*/ 
                        c.IncludeAllXmlComments(thisAssembly, AppDomain.CurrentDomain.BaseDirectory);
                        c.IgnoreIsSpecifiedMembers();
                        c.DescribeAllEnumsAsStrings();                        //c.DescribeAllEnumsAsStrings(camelCase: false);
                        c.BasicAuth("basic").Description("AHcAaQBuAGQAYQAuAG0AYQB5AGEAcwBhAHIAaQBAAHMAbwBsAHUAdABpAGYALgBjAG8ALgBpAGQADQAKAFcAaQBuAGQAYQBNAFI=");

                    })
                .EnableSwaggerUi(c =>
                    {

                        c.ShowExtensions(true);


                        c.SetValidatorUrl("https://online.swagger.io/validator");

                        c.UImaxDisplayedTags(100);

                        // Filter the operations works as a search, to disable set to "null"
                        //
                        c.UIfilter("''");

                        // Specify which HTTP operations will have the 'Try it out!' option. An empty parameter list disables
                        // it for all operations.
                        //
                        //c.SupportedSubmitMethods("GET", "HEAD");

                        // Use the CustomAsset option to provide your own version of assets used in the swagger-ui.
                        // It's typically used to instruct Swagger-Net to return your version instead of the default
                        // when a request is made for "index.html". As with all custom content, the file must be included
                        // in your project as an "Embedded Resource", and then the resource's "Logical Name" is passed to
                        // the method as shown below.
                        //
                        //c.CustomAsset("index", thisAssembly, "YourWebApiProject.SwaggerExtensions.index.html");

                        // If your API has multiple versions and you've applied the MultipleApiVersions setting
                        // as described above, you can also enable a select box in the swagger-ui, that displays
                        // a discovery URL for each version. This provides a convenient way for users to browse documentation
                        // for different API versions.
                        //
                        //c.EnableDiscoveryUrlSelector();

                        // If your API supports the OAuth2 Implicit flow, and you've described it correctly, according to
                        // the Swagger 2.0 specification, you can enable UI support as shown below.
                        //
                        //c.EnableOAuth2Support(
                        //    clientId: "test-client-id",
                        //    clientSecret: null,
                        //    realm: "test-realm",
                        //    appName: "Swagger UI"
                        //    //additionalQueryStringParams: new Dictionary<string, string>() { { "foo", "bar" } }
                        //);
                    });
        }

        public static bool ResolveVersionSupportByRouteConstraint(ApiDescription apiDesc, string targetApiVersion)
        {
            return (apiDesc.Route.RouteTemplate.ToLower().Contains(targetApiVersion.ToLower()));
        }

        private class ApplyDocumentVendorExtensions : IDocumentFilter
        {
            public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
            {
                // Include the given data type in the final SwaggerDocument
                //
                //schemaRegistry.GetOrRegister(typeof(ExtraType));
            }
        }

        public class AssignOAuth2SecurityRequirements : IOperationFilter
        {
            public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
            {
                // Correspond each "Authorize" role to an oauth2 scope
                var scopes = apiDescription.ActionDescriptor.GetFilterPipeline()
                    .Select(filterInfo => filterInfo.Instance)
                    .OfType<AuthorizeAttribute>()
                    .SelectMany(attr => attr.Roles.Split(','))
                    .Distinct();

                if (scopes.Any())
                {
                    if (operation.security == null)
                        operation.security = new List<IDictionary<string, IEnumerable<string>>>();

                    var oAuthRequirements = new Dictionary<string, IEnumerable<string>>
                    {
                        { "oauth2", scopes }
                    };

                    operation.security.Add(oAuthRequirements);
                }
            }
        }

        private class ApplySchemaVendorExtensions : ISchemaFilter
        {
            public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
            {
                // Modify the example values in the final SwaggerDocument
                //
                if (schema.properties != null)
                {
                    foreach (var p in schema.properties)
                    {
                        switch (p.Value.format)
                        {
                            case "int32":
                                p.Value.example = 123;
                                break;
                            case "double":
                                p.Value.example = 9858.216;
                                break;
                        }
                    }
                }
            }
        }
    }
}
