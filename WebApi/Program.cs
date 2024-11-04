using Application.Options;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Infrastructure.Persistence;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApi.Classes;
using WebApi.Middlewares;


var builder = WebApplication.CreateBuilder(args);

var redisOptions = new RedisOptions();
builder.Configuration.GetSection("Redis").Bind(redisOptions);
var conns = new ConnectionStringsOptions();
builder.Configuration.GetSection("ConnectionStrings").Bind(conns);
var MbOptions = new MessageBrokerOptions();
builder.Configuration.GetSection("MessageBroker").Bind(MbOptions);

builder.Services.Configure<ConnectionStringsOptions>(builder.Configuration.GetSection("ConnectionStrings"))
                .Configure<RedisOptions>(builder.Configuration.GetSection("Redis"));

Log.Logger = new LoggerConfiguration().Enrich.FromLogContext().CreateBootstrapLogger();
builder.Host.UseSerilog(((ctx, lc) => lc
.ReadFrom.Configuration(ctx.Configuration)));




// Configuration for auth
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}
)
.AddJwtBearer(options =>
{
    builder.Configuration.GetSection("Auth").Bind(options);
    options.Audience = options.Audience.Replace("{MicroserviceName}", builder.Configuration["MicroserviceName"]);
    options.TokenValidationParameters.ValidAudience = options.TokenValidationParameters.ValidAudience.Replace("{MicroserviceName}", builder.Configuration["MicroserviceName"]);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddWebUIServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(conns, redisOptions, builder.Configuration, MbOptions);

//health and readiness
builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "live" }) // Basic health check
            .AddSqlServer(
                connectionString: conns.DefaultConnection,
                name: "sql",
                failureStatus: HealthStatus.Unhealthy, // Optional: Customize failure status
                tags: new[] { "db", "sql", "ready" });

// swagger
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();
builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Webapi.xml"));
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


//------------------per versionimin----------
var apiVersioningBuilder = builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));
});
apiVersioningBuilder.AddApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
//-----------------End versionimi -----------



var app = builder.Build();



var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    //using (var scope = app.Services.CreateScope())
    //{
    //    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
    //    await initialiser.InitialiseAsync();
    //}

}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// --- per swagger
app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
    options.PreSerializeFilters.Add((swagger, req) =>
    {
        swagger.Servers = new List<OpenApiServer>() { new OpenApiServer() { Url = $"https://{req.Host}" } };
    });
});
app.UseSwaggerUI(c =>
{
    foreach (var desc in apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        c.SwaggerEndpoint($"../swagger/{desc.GroupName}/swagger.json", desc.ApiVersion.ToString());
        c.DefaultModelsExpandDepth(-1);
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    }
});
//-----------------End swagger -----------
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<LogUserNameMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseSerilogRequestLogging();


//health and readiness
app.MapHealthChecks("/health/liveness", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live"),
    ResponseWriter = HealthCheckClass.WriteHealthResponse
});
app.MapHealthChecks("/health/readiness", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"), // Readiness checks
    ResponseWriter = HealthCheckClass.WriteHealthResponse
});

app.Run();
