using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;


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


var path = Path.Combine(Directory.GetCurrentDirectory(), "firebase-credentials.json");

var credential = GoogleCredential.FromFile(path);

if (FirebaseApp.DefaultInstance == null)
{
    FirebaseApp.Create(new AppOptions()
    {
        Credential = credential,
        ProjectId = "airsensorai"
    });
}


// Registrar Firestore
builder.Services.AddSingleton(provider =>
{
    var credential = GoogleCredential.FromFile("firebase-credentials.json");

    return new FirestoreDbBuilder
    {
        ProjectId = "airsensorai",
        Credential = credential
    }.Build();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/teste", () => "ok");
app.UseCors("AllowAll");
app.UseRouting();
app.MapControllers();
app.Run();