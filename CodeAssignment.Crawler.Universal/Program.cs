using AngleSharp;
using CodeAssignment.Crawler.Abstractions;
using CodeAssignment.Crawler.Universal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);

// https://anglesharp.github.io library is used to parse HTML 
builder.Services.AddScoped<IBrowsingContext>(_ => BrowsingContext.New(Configuration.Default.WithDefaultLoader()));
builder.Services.AddScoped<ICrawler, UniversalCrawler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();