using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public UsuarioController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }
        public ActionResult GuardarCargaMasiva()
        {
            // Extraer la ruta del archivo correcto de la sesion
            string rutaTxt = HttpContext.Session.GetString("rutaCorrectos");
            string rutaExcel = HttpContext.Session.GetString("rutaCorrectosExcel");


            if (rutaTxt != null)
            {
                using (StreamReader sr = new StreamReader(rutaTxt))
                {
                    string linea = String.Empty;
                    sr.ReadLine();
                    while ((linea = sr.ReadLine()) != null)
                    {
                        string[] lineaLeida = linea.Split("|");//linea de datos
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.Rol = new ML.Rol();


                        usuario.Nombre = lineaLeida[0];
                        usuario.ApellidoPaterno = lineaLeida[1];
                        usuario.ApellidoMaterno = lineaLeida[2];
                        usuario.UserName = lineaLeida[3];
                        usuario.Email = lineaLeida[4];
                        usuario.Password = lineaLeida[5];
                        usuario.Sexo = lineaLeida[6];
                        usuario.Telefono = lineaLeida[7];
                        usuario.Celular = lineaLeida[8];
                        usuario.FechaNacimiento = lineaLeida[9];
                        usuario.Curp = lineaLeida[10];
                        usuario.Rol.IdRol = Convert.ToInt32(lineaLeida[11]);


                        BL.Usuario.AddSP(usuario);
                    }
                }
                //Limpiar sesion
                HttpContext.Session.Remove("rutaCorrectos");
            }
            else if (rutaExcel != null)
            {
                var baseConnection = _configuration.GetSection("ConnectionOleDb")["OleDbBase"];
                var connectionString = string.Format(baseConnection, rutaExcel);
                ML.Result resultExcel = BL.Usuario.Excel(connectionString);

                foreach (ML.Usuario itemExcel in resultExcel.Objects)
                {
                    BL.Usuario.AddSP(itemExcel);

                }
                HttpContext.Session.Remove("rutaCorrectosExcel");
            }



            return RedirectToAction("GetAll");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();

            usuario.Errores = new List<object>();
            usuario.Correctos = new List<object>();

            usuario.Nombre = "";
            usuario.ApellidoPaterno = "";
            usuario.ApellidoMaterno = "";

            usuario.Rol = new ML.Rol();
            usuario.Rol.IdRol = 0;

            ML.Result result = BL.Usuario.GetAllSP(usuario);

            if (result.Correct)
            {
                usuario.Usuarios = result.Objects;
            }
            else
            {
                ViewBag.Mensaje = result.ErrorMessage;
            }
            //Consulta todos los roles 
            ML.Result resultRols = new ML.Result();

            resultRols = BL.Rol.GetAll();


            if (resultRols.Correct)
            {
                usuario.Rol = new ML.Rol();
                usuario.Rol.Rols = resultRols.Objects;
            }

            return View(usuario);
        }

        [HttpPost]
        public IActionResult GetAll(ML.Usuario usuario, string tipoArchivo, IFormFile archivo)
        {
            usuario.Errores = new List<object>();
            usuario.Correctos = new List<object>();

            if (tipoArchivo == null)
            {
                //condicion si vienen nullo
                inicializarUsuario(usuario);

            }

            else if (tipoArchivo == "txt")
            {
                //string extension = Path.GetExtension(inptArchivo.FileName);
                if (archivo.FileName.Split(".")[1] == "txt")
                {
                    if (archivo != null)
                    {
                        using (StreamReader sr = new StreamReader(archivo.OpenReadStream()))
                        {
                            string linea = String.Empty;
                            sr.ReadLine();
                            int numeroLinea = 2;
                            while ((linea = sr.ReadLine()) != null)
                            {
                                string[] lineaLeida = linea.Split("|");

                                string validacionCampos = ValidarFila(lineaLeida);

                                if (validacionCampos.Contains("es correcto"))
                                {
                                    // agregar a la lista de correctos
                                    usuario.Correctos.Add($"Linea{numeroLinea}|{validacionCampos}");

                                }
                                else
                                {
                                    // agregar a la lista de errores
                                    usuario.Errores.Add($"Linea {numeroLinea}|{validacionCampos}");
                                }
                                numeroLinea++;

                            }
                        }

                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string nombreCompleto = Path.GetFileNameWithoutExtension(archivo.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";

                        if (usuario.Errores.Count > 0)
                        {
                            // archivo con errores
                            string rutaCompleta = Path.Combine(webRootPath, "txt", "errores", nombreCompleto);

                            //Session["rutaErrores"] = rutaCompleta;
                            HttpContext.Session.SetString("rutaErrores", rutaCompleta);


                            if (!System.IO.File.Exists(rutaCompleta))
                            {
                                using (StreamWriter streamWriter = new StreamWriter(rutaCompleta))
                                {
                                    foreach (var linea in usuario.Errores)
                                    {
                                        streamWriter.WriteLine(linea);
                                    }
                                }
                            }
                        }
                        else
                        {
                            string rutaCompleta = Path.Combine(webRootPath, "txt", "correctos", nombreCompleto);

                            HttpContext.Session.SetString("rutaCorrectos", rutaCompleta);

                            var session = HttpContext.Session.GetString("rutaCorrectos");

                            if (!System.IO.File.Exists(rutaCompleta))
                            {
                                using (FileStream source = new FileStream(rutaCompleta, FileMode.Create))
                                {
                                    archivo.CopyTo(source);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                string nombreArchivo = archivo.FileName;
                string extensionArchivo = Path.GetExtension(archivo.FileName);
                string extension = archivo.FileName.Split(".")[1];
                int numeroLinea = 2;
                if (archivo.FileName.Split(".")[1] == "xlsx")
                {
                    if (archivo != null)
                    {
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string nombreCompleto = Path.GetFileNameWithoutExtension(archivo.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                        var baseConnection = _configuration.GetSection("ConnectionOleDb")["OleDbBase"];
                        string rutaCompleta = Path.Combine(webRootPath, "excel", nombreCompleto);
                        var connectionString = string.Format(baseConnection, rutaCompleta);

                        // Valido si existe mi archivo y lo guardo
                        if (!System.IO.File.Exists(rutaCompleta))
                        {
                            using (FileStream source = new FileStream(rutaCompleta, FileMode.Create))
                            {
                                archivo.CopyTo(source);
                            }
                        }
                        // Leer mi archivo de Excel.
                        ML.Result resultExcel = BL.Usuario.Excel(connectionString);

                        //Validacion de los campos y mostrar errores y correctos
                        if (resultExcel.Correct)
                        {
                            foreach (ML.Usuario excelObj in resultExcel.Objects)
                            {
                                string[] lineaLeida = 
                                {
                                        excelObj.Nombre ?? "",
                                        excelObj.ApellidoPaterno ?? "",
                                        excelObj.ApellidoMaterno ?? "",
                                        excelObj.UserName ?? "",
                                        excelObj.Email ?? "",
                                        excelObj.Password ?? "",
                                        excelObj.Sexo ?? "",
                                        excelObj.Telefono ?? "",
                                        excelObj.Celular ?? "",
                                        excelObj.FechaNacimiento ?? "",
                                        excelObj.Curp ?? "",
                                        excelObj.Rol?.IdRol.ToString() ?? "",

                                };
                                string resultValidacion = ValidarFila(lineaLeida);

                                if (resultValidacion.Contains("es correcto"))
                                {
                                    // agregar a la lista de correctos
                                    usuario.Correctos.Add($"Linea{numeroLinea}|{resultValidacion}");

                                }
                                else
                                {
                                    // agregar a la lista de errores
                                    usuario.Errores.Add($"Linea {numeroLinea}|{resultValidacion}");
                                }
                                numeroLinea++;
                            }
                        }
                        if (usuario.Errores.Count == 0)
                        {
                            HttpContext.Session.SetString("rutaCorrectosExcel", rutaCompleta);
                        }
                    }
                }
            }
            inicializarUsuario(usuario);
            return View(usuario);
        }
        public static string ValidarFila(string[] lineaLeida)
        {
            string error = "";

            // Validar que ningún campo esté vacío
            //for (int i = 0; i < lineaLeida.Length; i++)
            //{
            //    if (string.IsNullOrWhiteSpace(lineaLeida[i]))
            //    {
            //        error = $"El campo {i + 1} está vacío. | ";
            //    }
            //}

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
            if (!Regex.IsMatch(lineaLeida[5], @"^[a-zA-Z0-9._%#!@-]*$"))
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

            if (error != "")
            {
                return error;
            }
            else
            {
                return error += "El registro " + lineaLeida[0] + " es correcto";
            }
        }


        [HttpGet]
        public ActionResult Form(int? IdUsuario)
        {
            ML.Usuario usuario = new ML.Usuario();

            ML.Result resultRols = new ML.Result();

            resultRols = BL.Rol.GetAll();
            //Consulta todos los roles 

            if (resultRols.Correct)
            {
                usuario.Rol = new ML.Rol();
                usuario.Rol.Rols = resultRols.Objects;
            }

            if (IdUsuario > 0) //----UPDATE-----
            {

                //obtener el usuario por id
                ML.Result result = BL.Usuario.GetByIdSP(IdUsuario.Value);//bien

                if (result.Correct)
                {
                    usuario = (ML.Usuario)result.Objects[0]; //unboxing y accede al primer elemento de esa lista

                    if (usuario.Rol == null)
                    {
                        usuario.Rol = new ML.Rol();
                    }
                    usuario.Rol.Rols = resultRols.Objects;

                }
            }

            return View(usuario);

        }
        [HttpPost]
        public ActionResult Form(ML.Usuario usuario) //Add, update
        {

            bool formulario = ModelState.IsValid;

            if (formulario)
            {
                if (usuario.IdUsuario == 0) //Add
                {

                    ML.Result resultaAdd = BL.Usuario.AddSP(usuario);

                }
                else
                {  //Update
                    ML.Result resultUpdate = BL.Usuario.UpdateSP(usuario);

                }
                Console.WriteLine("Formulario valido");

                return RedirectToAction("GetAll");
            }

            ML.Result result = BL.Rol.GetAll();
            if (result.Correct)
            {
                usuario.Rol.Rols = result.Objects;
            }


            return View(usuario);


        }

        public ActionResult Delete(int IdUsuario) //Delete
        {
            // Primero se elimina las imagenes del usuario

            ML.Result resultDelete = BL.Usuario.DeleteSP(IdUsuario);

            if (resultDelete.Correct)
            {
                ViewBag.Mensaje = "Usuario eliminado correctamente";
            }
            else
            {
                ViewBag.Mensaje = "Ocurrio un error al agregar el usuario.";
            }

            return RedirectToAction("GetAll");
        }
        private void inicializarUsuario(ML.Usuario usuario)
        {
            usuario.Nombre = usuario.Nombre ?? "";
            usuario.ApellidoPaterno = usuario.ApellidoPaterno ?? "";
            usuario.ApellidoMaterno = usuario.ApellidoMaterno ?? "";

            ML.Result result = BL.Usuario.GetAllSP(usuario);

            if (result.Correct)
            {
                usuario.Usuarios = result.Objects;
            }
            else
            {
                ViewBag.Mensaje = result.ErrorMessage;
            }
            //Consulta todos los roles 
            ML.Result resultRols = new ML.Result();

            resultRols = BL.Rol.GetAll();


            if (resultRols.Correct)
            {
                usuario.Rol = new ML.Rol();
                usuario.Rol.Rols = resultRols.Objects;
            }
        }

    }
}
