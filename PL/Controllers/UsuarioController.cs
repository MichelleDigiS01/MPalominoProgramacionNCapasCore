using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();


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
        public IActionResult GetAll(ML.Usuario usuario)
        {
            //condicion si vienen nullo
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

            return View(usuario);
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
                    usuario = (ML.Usuario)result.Objects[0]; //unboxing y accede al primer elemnto de esa lista

                    if (usuario.Rol == null)
                    {
                        usuario.Rol = new ML.Rol();
                    }
                    usuario.Rol.Rols = resultRols.Objects;

                }
            }
            ViewBag.FechaNacimientoFormateada = Convert.ToDateTime(usuario.FechaNacimiento);

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

                }else{  //Update
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

    }
}
