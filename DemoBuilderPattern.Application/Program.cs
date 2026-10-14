using Tools.Security;

var builder = WebApplication.CreateBuilder(args);

string policyName = "Corsican Police Department";
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCustomSecurity(o =>
{
    o.UseCors = true;
    o.AddOrigins("https://localhost:4200", "https://10.0.5.105:4200/");
    o.SetMethods(Verbs.ALL);
    o.AddHeaders("X-AppId");
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCustomSecurity();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
