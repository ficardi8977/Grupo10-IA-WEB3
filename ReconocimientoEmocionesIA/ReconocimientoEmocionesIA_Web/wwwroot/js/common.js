﻿
function generarMeme(fileName)
{
    fetch(`/Home/GenerarMeme/?fileName=${fileName}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok ' + response.statusText);
            }
            return response.json();
        })
        .then(memeData => {

            console.log(memeData);

            const params = new URLSearchParams();

            params.append('emocionId', memeData.frase.idEmotion);
            params.append('fraseId', memeData.frase.idPhrase);
            params.append('imagen', memeData.imagen);
            params.append('frase', memeData.frase.description);

            memeData.emociones.forEach((emocion, index) => {
                params.append(`Emociones[${index}].Nombre`, emocion.nombre);
                params.append(`Emociones[${index}].Porcentaje`, emocion.porcentaje * 100);
            });

            const queryString = params.toString();

            window.location.href = 'https://localhost:7218/Home/Index?' + queryString;
            spinner.style.display = "none";
        })
        .catch(error => {
            spinner.style.display = "none";
        });
}

function realizarAccionFetch(api, imageData, contentType) {
    spinner.style.display = "inline-flex";
    const options = {
        method: 'POST',
        body: imageData
    };

    if (contentType) {
        options.headers = {
            'Content-Type': contentType
        };
    }

    fetch(api, options)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok ' + response.statusText);
            }
            return response.json();
        })
        .then(data => {
            generarMeme(data.fileName);
        })
        .catch(error => {
            spinner.style.display = "none";
        });
}