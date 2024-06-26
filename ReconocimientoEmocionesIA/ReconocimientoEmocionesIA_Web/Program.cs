using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using ReconocimientoEmocionesIA_Entidades.EF;
using ReconocimientoEmocionesIA_Logica;
using ReconocimientoEmocionesIA_Logica.Interfaces;
using ReconocimientoEmocionesIA_Logica.Servicios;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 524288000; // 500 MB
});

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<MemeGeneratorContext>();
builder.Services.AddTransient<IImagenService, ImagenService>();
builder.Services.AddScoped<IReconocimientoEmocionesService, ReconocimientoEmocionesService>();
builder.Services.AddScoped<IMemeService, MemeService>();
builder.Services.AddScoped<IGaleriaService, GaleriaService>();


// Add services to the container.
builder.Services.AddControllersWithViews();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
