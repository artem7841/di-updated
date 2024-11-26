using FractalPainting.Application;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// services.AddSingleton<IApiAction, KochFractalAction>();
// services.AddSingleton<IApiAction, >();
//
// var serviceProvider = services.BuildServiceProvider();
// var app = serviceProvider.GetRequiredService<App();

await new App().Run();