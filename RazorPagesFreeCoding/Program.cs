using RazorPagesFreeCoding.Domain;
using RazorPagesFreeCoding.Infrastructure;
using RazorPagesFreeCoding.Infrastructure.Configuration;
using Serilog;
using Serilog.Events;
using Polly;
using RazorPagesFreeCoding.Infrastructure.Policies;

Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.CreateBootstrapLogger();

try
{
	Log.Information("Launching the web host");

	var builder = WebApplication.CreateBuilder(args);

		builder.Host.UseSerilog((context, services, configuration)=>configuration
		.ReadFrom.Configuration(context.Configuration)
		.ReadFrom.Services(services)
		.Enrich.FromLogContext());

	// Add services to the container.


	builder.Services.AddControllersWithViews();

	builder.Services.AddSingleton<Catalog>();

	builder.Configuration.AddUserSecrets<Program>();

	builder.Services.AddSingleton<SmtpClientPolicy>();

	builder.Services.Configure<SmtpCredentials>(builder.Configuration.GetSection("SmtpCredentials"));

	builder.Services.AddSingleton<IMailSender, SmtpMailSender>();

	var app = builder.Build();

	app.UseSerilogRequestLogging(configure=>configure.MessageTemplate=
		"HTTP {RequestMethod} {RequestPath} responed {StatusCode} in {Elapsed:0.0000}ms");

	// Configure the HTTP request pipeline.
	if (!app.Environment.IsDevelopment())
	{
		app.UseExceptionHandler("/Home/Error");
		// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		app.UseHsts();
	}

	app.UseHttpsRedirection();
	app.UseStaticFiles();

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

