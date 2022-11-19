using ChatApp.Algorithms;
using ChatApp.Factories.AuthTokenFactory;
using ChatApp.Hubs;
using ChatApp.Models;
using ChatApp.Repositories;
using ChatApp.Services.AuthorizationService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    builder.WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    );
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.IncludeErrorDetails = true;

    o.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = false,
        ValidIssuer = AuthOptions.ISSUER,
        ValidAudience = AuthOptions.AUDIENCE,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
    };
});

builder.Services.AddAuthorization();

builder.Services.AddTransient<IAuthTokenFactory, AuthTokenFactory>();
builder.Services.AddTransient<BaseAuthorizationService, AppUserAuthorizationService>();
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<ChatParticipantsRepository>();
builder.Services.AddSingleton<DiffieHellman>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(120),
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/hubs/chat");
    endpoints.MapHub<KeyExchangeHub>("/hubs/key-exchange");
});

app.Run();
