using LibraryApi.Data;
using LibraryApi.Repositories;
using LibraryApi.Services;
using Microsoft.EntityFrameworkCore;

var builder =
    WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Register EF Core DbContext
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "LibraryConnection"
        )
    )
);


// Register Repository
builder.Services.AddScoped<
    IBookRepository,
    BookRepository
>();


// Register Service
builder.Services.AddScoped<
    IBookService,
    BookService
>();


// CORS for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AngularPolicy",
        policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:4200"
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// You can keep this if HTTPS works correctly.
// If you get HTTPS redirect warnings locally,
// you can comment it out temporarily.
app.UseHttpsRedirection();


app.UseCors("AngularPolicy");

app.MapControllers();

app.Run();