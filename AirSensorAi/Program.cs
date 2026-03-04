using FirebaseAdmin;
using FirebaseAdmin.Database;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
builder.Services.AddSwaggerGen();

// Register Firebase services
builder.Services.AddSingleton<FirebaseApp>(provider =>
{
    string jsonConfig = Environment.GetEnvironmentVariable("FIREBASE_CONFIG");
    
    if (string.IsNullOrEmpty(jsonConfig))
    {
        throw new InvalidOperationException("FIREBASE_CONFIG environment variable is not set");
    }
    
    if (FirebaseApp.DefaultInstance == null)
    {
        return FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromJson(jsonConfig)
        });
    }
    
    return FirebaseApp.DefaultInstance;
});

builder.Services.AddSingleton<FirebaseDatabase>(provider =>
{
    var firebaseApp = provider.GetRequiredService<FirebaseApp>();
    return FirebaseDatabase.GetInstance(firebaseApp, "https://airsensorai-default-rtdb.firebaseio.com");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.MapGet("/teste", () => "ok");
app.UseCors("AllowAll");
app.UseRouting();
app.MapControllers();
app.Run();
