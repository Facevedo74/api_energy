﻿using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using redarbor_PT.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Redarbor", Version = "v1" });
    var filePath = Path.Combine(AppContext.BaseDirectory, "prueba_redarbor.xml");
    c.IncludeXmlComments(filePath);
});


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("mainContext"));
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Redarbor v1");
    options.RoutePrefix = string.Empty;
});

app.Run();

