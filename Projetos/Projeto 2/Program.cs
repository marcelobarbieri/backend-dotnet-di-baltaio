using DependencyInjectionLifetimeSample.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections;

var builder = WebApplication.CreateBuilder(args);

//var descriptor = new ServiceDescriptor(
//    typeof(IService),
//    typeof(PrimaryService),
//    ServiceLifetime.Transient);
//builder.Services.TryAddEnumerable(descriptor);

builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IService, PrimaryService>());
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IService, PrimaryService>()); // permite
//builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IService, SecondaryService>()); // não permite

var app = builder.Build();

app.MapGet("/", (IEnumerable<IService> services) 
    => Results.Ok(services.Select(x => x.GetType().Name)));

app.Run();

public interface IService
{

}