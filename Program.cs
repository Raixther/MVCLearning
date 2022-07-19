using MVCLearning.Config;
using MVCLearning.Domain;
using MVCLearning.EventsConsumers;
using MVCLearning.Infrastructure;
using MVCLearning.Middleware;

using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.CreateBootstrapLogger();

try
{

var builder = WebApplication.CreateBuilder(args); Log.Information("Launching the web host");

builder.Host.UseSerilog((context, services, configuration) => configuration
.ReadFrom.Configuration(context.Configuration)
.ReadFrom.Services(services)
.Enrich.FromLogContext());

	// Add services to the container.
builder.Services.AddSingleton<Catalog>();
builder.Services.AddControllersWithViews();

builder.Services.Configure<SmtpConfig>(builder.Configuration.GetSection("SmtpConfig"));

builder.Services.AddSingleton<IMailSender, SmtpMailSender>();
builder.Services.AddHostedService<ProductAddedEventHandler>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseMiddleware<RequestCounterMiddleware>();

app.UseRouting();


app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
}
catch (Exception ex)
{
	Log.Fatal(ex, "Host terminated unexpectedly");
	return 1;
}
finally
{
	Log.CloseAndFlush();
}

return 0;
