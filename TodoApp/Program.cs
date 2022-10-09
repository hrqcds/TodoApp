using TodoApp.Config;
using TodoApp.Routes;

var app = ConfigBuilder.Run();
ConfigApp.Run(app);

AppRoute.Run(app);

app.Run();

