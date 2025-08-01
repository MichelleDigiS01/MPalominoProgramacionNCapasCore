using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public IActionResult GetAll(string nombre = "", string apellidoPaterno = "", string apellidoMaterno = "")
        {
            ML.Usuario usuario = new ML.Usuario();

            ML.Result result = BL.Usuario.GetAllSP(nombre, apellidoPaterno, apellidoMaterno);

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
            ML.Result result = BL.Usuario.GetAllSP(usuario.Nombre, usuario.ApellidoPaterno, usuario.ApellidoMaterno);

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
            //ML.Result resultEstados = new ML.Result();
            //resultEstados = BL.Estado.GetAll();

            //if (resultEstados.Correct)
            //{
            //    if (usuario.Direccion == null)
            //    {
            //        usuario.Direccion = new ML.Direccion();
            //        usuario.Direccion.Colonia = new ML.Colonia();
            //        usuario.Direccion.Colonia.Municipio = new ML.Municipio();
            //        usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
            //    }

            //    usuario.Direccion.Colonia.Municipio.Estado.Estados = resultEstados.Objects;

            //    ML.Result resultMunicipios = BL.Municipio.GetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);
            //    usuario.Direccion.Colonia.Municipio.Municipios = resultMunicipios.Objects;

            //    ML.Result resultColonias = BL.Colonia.GetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio);
            //    usuario.Direccion.Colonia.Colonias = resultColonias.Objects;
            //}


            if (IdUsuario > 0) //----UPDATE-----
            {

                //obtener el usuario por id
                ML.Result result = BL.Usuario.GetById(IdUsuario.Value);//bien

                if (result.Correct)
                {
                    usuario = (ML.Usuario)result.Object;//unboxing

                    usuario.Rol.Rols = resultRols.Objects;


                }
            }
            ViewBag.FechaNacimientoFormateada = usuario.FechaNacimiento.ToString("yyyy-MM-dd");

            return View(usuario);

        }
        [HttpPost]
        public ActionResult Form(ML.Usuario usuario) //Add, update
        {


            if (usuario.IdUsuario == 0) //Add
            {
                ML.Result result = BL.Usuario.Add(usuario);

                //if (result.Correct)
                //{
                //    int IdUsuario = (int)result.Object;

                //    if (IdUsuario > 0)
                //    {
                //        usuario.IdUsuario = IdUsuario;

                //        ML.Result resultDireccionAdd = BL.Direccion.Add(usuario);
                //    }


                //}

            }
            else  //Update
            {
                ML.Result resultUpdate = BL.Usuario.Update(usuario);

            }


            return RedirectToAction("GetAll");
        }

        public ActionResult Delete(int IdUsuario) //Delete
        {
            // Primero se elimina las imagenes del usuario

            ML.Result resultDelete = BL.Usuario.Delete(IdUsuario);
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
