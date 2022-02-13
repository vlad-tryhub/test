using CodeAssignment.Crawler.Universal.RestClient;
using CodeAssignment.Crawler.Wikipedia.RestClient;
using CodeAssignment.Orchestrator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddScoped<IUniversalCrawlerClient, UniversalCrawlerClient>();
builder.Services.AddScoped<IWikipediaCrawlerClient, WikipediaCrawlerClient>();
builder.Services.AddScoped<ICrawlerResolver, CrawlerResolver>();
builder.Services.AddSingleton<IMongoDatabase>(_ => new MongoClient(
    builder.Configuration.GetConnectionString("MongoDB")
).GetDatabase("demo"));

builder.Services.Configure<UniversalCrawlerOptions>(builder.Configuration.GetSection("Crawlers:Universal"));
builder.Services.Configure<WikipediaCrawlerOptions>(builder.Configuration.GetSection("Crawlers:Wikipedia"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();