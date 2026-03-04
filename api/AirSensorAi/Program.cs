using FireSharp.Config;
using FireSharp.Interfaces;

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


// Register Firebase configuration
builder.Services.AddSingleton<IFirebaseConfig>(provider =>
{
    var config = new FirebaseConfig
    {
        BasePath = Environment.GetEnvironmentVariable("FIREBASE_BASE_PATH") 
                   ?? throw new InvalidOperationException("FIREBASE_BASE_PATH environment variable is not set")
    };
    return config;
});

builder.Services.AddScoped<IFirebaseClient>(provider =>
{
    var config = provider.GetRequiredService<IFirebaseConfig>();
    return new FireSharp.FirebaseClient(config);
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
