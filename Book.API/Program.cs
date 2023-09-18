using Book.Application.Extensions;
using Book.Application.Interfaces;
using Book.Application.Models;
using Book.Application.Services;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookAPI", Description = "CRUD API FOR BOOKS", Version = "v1" });
});

builder.Services.RegisterDatabaseContext(builder.Configuration);
builder.Services.AddRepository();
builder.Services.AddKeycloackAuth(builder.Configuration);

var app = builder.Build();

app.MapGet("/api/Books", async (IBookService bookService) => await bookService.ListAllBooks());
app.MapGet("/api/Books/{id}", async (Guid id ,IBookService bookService) 
    => await bookService!.GetBookById(id));

app.MapPost("/api/Books", (BookModel book ,IBookService bookService) => bookService!.Create(book)).RequireAuthorization("Admin");;
app.MapPut("/api/Books", (BookModel book ,IBookService bookService) => bookService!.Update(book)).RequireAuthorization("Admin");;
app.MapDelete("/api/Books/{id}", (Guid id,IBookService bookService) => bookService!.Delete(id)).RequireAuthorization("Admin");;

app.MapSwagger();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookAPI v1");
});

app.UseAuthentication();
app.UseAuthorization();

app.Run();