using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy - allowing the api to be used between two programs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder =>
        {
            builder.WithOrigins("http://localhost:3002")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply CORS policy
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.UseStaticFiles(); // Default static files middleware
//The photo direcotry to save employee photos
var photoDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Photos");
Console.WriteLine($"Serving photos from: {photoDirectory}");
//allowing the use of static photos. 
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(photoDirectory),
    RequestPath = "/Photos"
});
app.MapControllers();

app.Run();
