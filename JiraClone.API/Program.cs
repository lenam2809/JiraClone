using AutoMapper;
using JiraClone.Data;
using JiraClone.Service.Profiles;
using JiraClone.Utils.BaseService;
using JiraClone.Utils.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<JiraCloneDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("MainConnectionString"))
);

builder.Services.AddAutoMapper(typeof(BaseService));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<BaseService>();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
