using System.Text;
using LibraryApi.Data;
using LibraryApi.Repositories;
using LibraryApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter your JWT token."
        }
    );

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        }
    );
});

builder.Services.AddDbContext<LibraryDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration
                .GetConnectionString(
                    "LibraryConnection"
                )
        )
);


builder.Services.AddScoped<
    IBookRepository,
    BookRepository>();

builder.Services.AddScoped<
    IBookService,
    BookService>();


var jwtKey =
    builder.Configuration["Jwt:Key"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "JWT signing key is missing."
    );
}


var key =
    Encoding.UTF8.GetBytes(jwtKey);


builder.Services
    .AddAuthentication(
        options =>
        {
            options.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;

            options.DefaultChallengeScheme =
                JwtBearerDefaults.AuthenticationScheme;
        }
    )
    .AddJwtBearer(
        options =>
        {
            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey =
                        true,

                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            key
                        ),

                    ValidateIssuer =
                        true,

                    ValidIssuer =
                        builder.Configuration[
                            "Jwt:Issuer"
                        ],

                    ValidateAudience =
                        true,

                    ValidAudience =
                        builder.Configuration[
                            "Jwt:Audience"
                        ],

                    ValidateLifetime =
                        true,

                    ClockSkew =
                        TimeSpan.Zero
                };
        }
    );


builder.Services.AddAuthorization();


builder.Services.AddCors(
    options =>
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
            }
        );
    }
);


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


app.UseCors("AngularPolicy");


app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();


app.Run();