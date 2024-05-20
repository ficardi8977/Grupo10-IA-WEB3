﻿
// This file was auto-generated by ML.NET Model Builder. 

using ReconocimientoEmocionesML_Consola;

// Create single instance of sample data from first line of dataset for model input
var imageBytes = File.ReadAllBytes(@"C:\Users\Fer\Downloads\emocionesData\disgusto\10018.jpg");
ReconocimientoEmocionesIAModelML.ModelInput sampleData = new ReconocimientoEmocionesIAModelML.ModelInput()
{
    ImageSource = imageBytes,
};

// Make a single prediction on the sample data and print results.
var sortedScoresWithLabel = ReconocimientoEmocionesIAModelML.PredictAllLabels(sampleData);
Console.WriteLine($"{"Class",-40}{"Score",-20}");
Console.WriteLine($"{"-----",-40}{"-----",-20}");

foreach (var score in sortedScoresWithLabel)
{
    Console.WriteLine($"{score.Key,-40}{score.Value,-20}");
}
