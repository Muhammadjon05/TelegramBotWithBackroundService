using TelegramBotApi;
using TelegramBotApi.Managers;
using TelegramBotApi.Managers.Contracts;
using TelegramBotApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IQuestion), typeof(QuestionRepository));
builder.Services.AddScoped(typeof(IUser), typeof(UserServices));
builder.Services.AddScoped<QuestionManager>();
builder.Services.AddScoped<UserManager>();
builder.Services.AddScoped<DailyMessageSender>();
builder.Services.AddHostedService<DailyMessageSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
