using System;
using System.Linq;
using LibraryManager.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var filePath = "book.json"; // Specify the path to the JSON file

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<Library>(_ => new Library(filePath));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

app.MapGet("/books", (Library library) =>
{
    var books = library.GetBooks();
    return Results.Ok(books);
});

app.MapGet("/books/{id}", (Guid id, Library library) =>
{
    var book = library.GetBooks().FirstOrDefault(b => b.Id == id);
    if (book == null)
        return Results.NotFound();
    return Results.Ok(book);
});

app.MapPost("/books", (Book book, Library library) =>
{
    library.AddBook(book);
    return Results.Created($"/books/{book.Id}", book);
});

app.MapDelete("/books/{id}", (Guid id, Library library) =>
{
    library.RemoveBook(id);
    return Results.NoContent();
});

app.Run();
