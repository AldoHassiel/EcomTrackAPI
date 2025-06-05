var builder = WebApplication.CreateBuilder(args);

//Cosas de las COORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Local",
        builder => builder
            .WithOrigins("http://localhost:5173") // URL de tu frontend Next.js
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
    options.AddPolicy("Vercel",
        builder => builder
            .WithOrigins("https://ecomtrack.vercel.app/") // URL de tu frontend Next.js
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Local");
app.UseCors("Vercel");


app.UseAuthorization();

app.MapControllers();

app.Run();
