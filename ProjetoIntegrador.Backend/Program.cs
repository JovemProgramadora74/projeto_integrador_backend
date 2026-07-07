var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/status", () => Results.Ok(new { status = "Servidor Online" }))
    .WithName("PegarStatusServidor");

app.Run();
