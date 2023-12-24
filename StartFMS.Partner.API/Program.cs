using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenAI;
using OpenAI.Extensions;
using OpenAI.Managers;
using StartFMS.EF;
using StartFMS.Extensions.Configuration;
using StartFMS.Extensions.Line;
using StartFMS.Partner.API.Filters;
using StartFMS.Partner.API.Helper;


var builder = WebApplication.CreateBuilder(args);
var config = Config.GetConfiguration<Program>(); //加入設定檔
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenAIService();

//add core content
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
        });

    options.AddPolicy("AnotherPolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
});

//Append Filter 
builder.Services.AddControllers(content =>
{
    content.Filters.Add(typeof(LogActionFilters));
    content.Filters.Add(typeof(LogExceptionFilter));
    content.Filters.Add(typeof(ApiResultFilter));
});


//設定參數
builder.Services.AddDbContext<StartFmsBackendContext>(content =>
{
    content.UseSqlServer(config.GetConnectionString("Default"));
});
var openAiService = new OpenAIService(new OpenAiOptions()
{
    ApiKey = config.GetValue<string>("OpenAIServiceOptions:ApiKey"),
});
builder.Services.AddSingleton<OpenAIService>(openAiService);

var lineBots = new LineBot()
{
    ChannelToken = config.GetValue<string>("Line:Bots:channelToken"),
    AdminUserID = config.GetValue<string>("Line:Bots:adminUserID")
};
builder.Services.AddSingleton<LineBot>(lineBots);

var lineLogin = new LineLogin()
{
    ChannelToken = config.GetValue<string>("Line:Login:channelToken"),
    AdminUserID = config.GetValue<string>("Line:Login:adminUserID"),
    urlRequest = new LineLogin.UrlRequest
    {
        url = config.GetValue<string>("Line:Login:openIdConnect:url"),
        response_type = config.GetValue<string>("Line:Login:openIdConnect:response_type"),
        client_id = config.GetValue<string>("Line:Login:openIdConnect:client_id"),
        redirect_uri = config.GetValue<string>("Line:Login:openIdConnect:redirect_uri"),
        scope = config.GetValue<string>("Line:Login:openIdConnect:scope"),
        state = config.GetValue<string>("Line:Login:openIdConnect:state"),
    }
};
builder.Services.AddSingleton<LineLogin>(lineLogin);

var lineNotify = new LineNotify()
{
    ChannelToken = config.GetValue<string>("Line:Notify:channelToken"),
    DeveloperToken = config.GetValue<string>("Line:Notify:developerToken"),
    urlRequest = new LineNotify.UrlRequest
    {
        url = config.GetValue<string>("Line:Notify:openIdConnect:url"),
        response_type = config.GetValue<string>("Line:Notify:openIdConnect:response_type"),
        client_id = config.GetValue<string>("Line:Notify:openIdConnect:client_id"),
        redirect_uri = config.GetValue<string>("Line:Notify:openIdConnect:redirect_uri"),
        scope = config.GetValue<string>("Line:Notify:openIdConnect:scope"),
        state = config.GetValue<string>("Line:Notify:openIdConnect:state"),
    }
};
builder.Services.AddSingleton<LineNotify>(lineNotify);
builder.Services.AddSingleton<DiscordSocketClient>(serviceProvider =>
{
    var client = new DiscordSocketClient();
    var token = config.GetValue<string>("Discord:Bots:Token");
    client.LoginAsync(TokenType.Bot, token).GetAwaiter().GetResult();
    client.StartAsync().GetAwaiter().GetResult();
    if (client.ConnectionState == ConnectionState.Connected)
    {
        Console.WriteLine("Discord bot is connected.");
    }
    else
    {
        Console.WriteLine("Discord bot is not connected.");
    }
    return client;
});
var discordBot = new DiscordBot(builder.Services.BuildServiceProvider().GetRequiredService<DiscordSocketClient>());
discordBot.Execute();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Start Five Minutes Backend API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();

