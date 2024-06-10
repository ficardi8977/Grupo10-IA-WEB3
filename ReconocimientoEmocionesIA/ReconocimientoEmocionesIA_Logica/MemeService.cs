﻿using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using ReconocimientoEmocionesIA_Entidades;
using ReconocimientoEmocionesIA_Entidades.EF;
using ReconocimientoEmocionesIA_Logica.Interfaces;
using ReconocimientoEmocionesIA_Logica.Servicios;

namespace ReconocimientoEmocionesIA_Logica;

public class MemeService : IMemeService
{
    IReconocimientoEmocionesService reconocimientoEmocionesService;
    IImagenService imagenService;
    MemeGeneratorContext ctx;

    public MemeService(IImagenService imagenService, IReconocimientoEmocionesService reconocimientoEmocionesService, MemeGeneratorContext ctx) {
        this.reconocimientoEmocionesService = reconocimientoEmocionesService;
        this.imagenService = imagenService;
        this.ctx = ctx;
    }

    public Meme Generar(string fileName, string webRootPath)
    {
        Meme meme = new Meme();
        meme.Imagen= fileName;
        meme.Emociones = this.reconocimientoEmocionesService.ListarEmocionesDetectadas(File.ReadAllBytes(this.imagenService.ObtenerPathImagen(fileName, webRootPath)));
        meme.Frase = ObtenerFrasePorEmocion(meme.Emociones.First().Nombre);

        var emocion = meme.Emociones.FirstOrDefault();


        string imagePath = this.imagenService.ObtenerPathImagen(fileName, webRootPath);

        // Crear una copia de la imagen en memoria para evitar problemas de acceso
        using (MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(imagePath)))
        using (Image image = Image.FromStream(memoryStream))
        using (Graphics graphics = Graphics.FromImage(image))
        using (Font font = new Font("Arial", 20, FontStyle.Bold, GraphicsUnit.Pixel))
        {
            // Medir el tamaño del texto
            SizeF textSize = graphics.MeasureString(meme.Frase.Description, font);
            // Calcular la posición centrada horizontalmente y en la parte superior de la imagen
            PointF position = new PointF((image.Width - textSize.Width) / 2, 10); // Ajusta la separación desde el borde superior según tus necesidades

            // Crear el pincel para el borde sombreado
            SolidBrush shadowBrush = new SolidBrush(Color.Black);
            // Crear el pincel para el texto
            SolidBrush textBrush = new SolidBrush(Color.White);

            // Dibujar el borde sombreado
            graphics.DrawString(meme.Frase.Description, font, shadowBrush, position.X - 1, position.Y - 1);
            graphics.DrawString(meme.Frase.Description, font, shadowBrush, position.X + 1, position.Y - 1);
            graphics.DrawString(meme.Frase.Description, font, shadowBrush, position.X - 1, position.Y + 1);
            graphics.DrawString(meme.Frase.Description, font, shadowBrush, position.X + 1, position.Y + 1);

            // Dibujar el texto principal
            graphics.DrawString(meme.Frase.Description, font, textBrush, position);

            // Sobrescribir la imagen original
            image.Save(imagePath, ImageFormat.Jpeg);
        }

        meme.Imagen = imagePath;
        return meme;
    }

    public bool GuardarMeme(string fileName, int idEmotion, int idPhrase)
    {
        var memes = this.ctx.MemeImages;

        var meme = new MemeImage
        {
            ImagePath = fileName,
            IdEmotion = idEmotion,
            IdPhrase = idPhrase
        };

        memes.Add(meme);
        this.ctx.SaveChanges();

        return true;
    }


    private Phrase ObtenerFrasePorEmocion(string emocion)
    {
        var frase =  this.ctx.Phrases
        .Where(x => x.IdEmotionNavigation.Name == emocion)
        .AsEnumerable()
        .OrderBy(e => Guid.NewGuid())
        .FirstOrDefault();

        if (frase == null)
        {
          return this.ctx.Phrases
        .Where(x => x.IdEmotionNavigation.Name == "felicidad")
        .AsEnumerable()
        .OrderBy(e => Guid.NewGuid())
        .FirstOrDefault();

        }

        return frase;



    }

}
