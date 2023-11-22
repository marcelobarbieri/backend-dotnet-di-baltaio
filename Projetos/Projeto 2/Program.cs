using DependencyInjectionLifetimeSample.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections;

var builder = WebApplication.CreateBuilder(args);

builder.Services.TryAddTransient<IService, PrimaryService>();
builder.Services.TryAddTransient<IService, PrimaryService>();
builder.Services.TryAddTransient<IService, SecondaryService>();

var app = builder.Build();

app.MapGet("/", (IEnumerable<IService> services) 
    => Results.Ok(services.Select(x => x.GetType().Name)));

app.Run();

public interface IService
{

}