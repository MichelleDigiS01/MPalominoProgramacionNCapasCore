using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PLCarga
{
    public class txt
    {
        public static void Mensaje()
        {
            
            string rutatxt = "C:\\Users\\digis\\Downloads\\nombres.txt";

            using (StreamReader sr = File.OpenText(rutatxt))
            {
                string linea = String.Empty;
                sr.ReadLine();//Se salta la primer linea
                while ((linea = sr.ReadLine()) != null)
                {
                    string[] lineaLeida = linea.Split("|");

                    if (lineaLeida.Length == 11)
                    {
                        string validacionCampos = ValidarFila(lineaLeida);
                        Console.WriteLine(lineaLeida[0]);
                        Console.WriteLine(lineaLeida[1]);
                        Console.WriteLine(lineaLeida[2]);
                        Console.WriteLine(lineaLeida[3]);
                        Console.WriteLine(lineaLeida[4]);
                        Console.WriteLine(lineaLeida[5]);
                        Console.WriteLine(lineaLeida[6]);
                        Console.WriteLine(lineaLeida[7]);
                        Console.WriteLine(lineaLeida[8]);
                        Console.WriteLine(lineaLeida[9]);
                        Console.WriteLine(lineaLeida[10]);



                        if (!string.IsNullOrEmpty(validacionCampos))
                        {
                            Console.WriteLine("Errores de validación:");
                            Console.WriteLine(validacionCampos);
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

        public static string ValidarFila(string[] lineaLeida)
        {
            string error = "";

            // Validar que ningún campo esté vacío
            for (int i = 0; i < lineaLeida.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lineaLeida[i]))
                {
                    error = $"El campo {i + 1} está vacío. | ";
                }
            }

            if (!Regex.IsMatch(lineaLeida[0], @"^[a-zA-Záéíóúñ]+$"))
            {
                error = error + "Solo se aceptan letras en: " + lineaLeida[0] + " |";
            }
            if (!Regex.IsMatch(lineaLeida[1], @"^[a-zA-Záéíóúñ]+$"))
            {
                error = error + "Solo se aceptan letras en: " + lineaLeida[1] + " |";
            }
            if (!Regex.IsMatch(lineaLeida[2], @"^[a-zA-Záéíóúñ]+$"))
            {
                error = error + "Solo se aceptan letras en: " + lineaLeida[2] + " |";
            }
            if (!Regex.IsMatch(lineaLeida[3], @"^[a-zA-Z0-9._-]*$"))
            {
                error = error + "No se permiten espacios en: " + lineaLeida[3] + " |";
            }
            if (!Regex.IsMatch(lineaLeida[4], @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
            {
                error += $"Ejemplo de correo válido: Example_1234.%_@gmail.com — ingresado: {lineaLeida[4]} |";
            }
            if (!Regex.IsMatch(lineaLeida[5], @"^[a-zA-Z0-9._%@-]*$"))
            {
                error = error + "Ingresa una contraseña minimo con una Minus, una Mayus, un número y un caracter especial en: " + lineaLeida[5] + " |";
            }
            if (!Regex.IsMatch(lineaLeida[6], @"^[FM]+$"))
            {
                error = error + "Solo se aceptan letras F o M en: " + lineaLeida[6] + " |";
            }
            if (!Regex.IsMatch(lineaLeida[7], @"^[0-9]+$"))
            {
                error = error + "Solo se aceptan numeros en: " + lineaLeida[7] + " |";
            }
            if (!Regex.IsMatch(lineaLeida[8], @"^[0-9]+$"))
            {
                error = error + "Solo se aceptan numeros en: " + lineaLeida[8] + " |";
            }
            if (!Regex.IsMatch(lineaLeida[9], @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[0-2])[/](19|20)\d{2}$"))
            {
                error = error + "Ingresa una fecha válida (dd/mm/yyyy) en: " + lineaLeida[9] + " |";
            }
            if (!Regex.IsMatch(lineaLeida[10], @"^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$"))
            {
                error = error + "Ingresa una CURP válida en: " + lineaLeida[10] + " |";
            }

            return error;

        }
    }
}