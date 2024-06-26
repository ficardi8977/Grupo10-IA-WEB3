﻿using ReconocimientoEmocionesIA_Entidades.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReconocimientoEmocionesIA_Entidades
{
    public class Meme
    {
        public string Imagen { get; set; }

        public Phrase Frase { get; set; }
        public int EmocionId { get; set; } 
        public int FraseId { get; set; }

        public List<Emocion> Emociones { get; set; }
    }
}
