using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenAI.GPT3.Extensions;
using StartFMS.Extensions.Configuration;
using StartFMS.Extensions.Line;
using StartFMS.Models.Backend;
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
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(
        builder => {
            builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
        });

    options.AddPolicy("AnotherPolicy",
        builder => {
            builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
});

//設定參數
//builder.Services.AddDbContext<A00_BackendContext>(content => {
//    content.UseSqlServer(config.GetConnectionString("Default"));
//});

var lineBots = new LineBot() {
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



builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Start Five Minutes Backend API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
