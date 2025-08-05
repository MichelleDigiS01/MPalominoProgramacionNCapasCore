using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Usuario
    {

        public static ML.Result GetAllSP(ML.Usuario usuario)
        {
            
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MpalominoProgramacionNcapasContext context = new DL.MpalominoProgramacionNcapasContext())
                {
                    // conexion, en donde voy a guardar la informacion , ejecutar el SP


                    //var query = context.UsuarioGetAllDTO.FromSqlInterpolated($@"EXEC UsuarioGet {usuario.Nombre}, {usuario.ApellidoPaterno}, {usuario.ApellidoMaterno}, {usuario.Rol.IdRol}").ToList();

                    var query = context.UsuarioGetAllDTO.FromSqlInterpolated($@"EXEC UsuarioGetAllView {usuario.Nombre}, {usuario.ApellidoPaterno}, {usuario.ApellidoMaterno}, {usuario.Rol.IdRol}").ToList();

                    //var query = context.UsuarioGetAllDTO.FromSqlInterpolated($@"EXEC UsuarioGetAllDynamic {usuario.Nombre}, {usuario.ApellidoPaterno}, {usuario.ApellidoMaterno}, {usuario.Rol.IdRol}").ToList();

                    // FromSqlRaw - SELECT

                    // ExecuteSqlRaw   -INSERT UPDATE Y DELETE 

                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var usuarioObj in query)
                        {
                            ML.Usuario usuarios = new ML.Usuario();

                            usuarios.IdUsuario = usuarioObj.IdUsuario;
                            usuarios.UserName = usuarioObj.UserName;
                            usuarios.Nombre = usuarioObj.UsuarioNombre;
                            usuarios.ApellidoPaterno = usuarioObj.ApellidoPaterno;
                            usuarios.ApellidoMaterno = usuarioObj.ApellidoMaterno;
                            usuarios.Email = usuarioObj.Email;
                            usuarios.Password = usuarioObj.Password;
                            usuarios.Sexo = usuarioObj.Sexo;
                            usuarios.Telefono = usuarioObj.Telefono;
                            usuarios.Celular = usuarioObj.Celular;
                            usuarios.FechaNacimiento = Convert.ToDateTime(usuarioObj.FechaNacimiento);
                            usuarios.Curp = usuarioObj.Curp;

                            usuarios.Rol = new ML.Rol();
                            usuarios.Rol.IdRol = usuarioObj.IdRol;
                            usuarios.Rol.Nombre = usuarioObj.RolNombre;

                            result.Objects.Add(usuarios);

                        }

                        result.Correct = true;
                    }
                    else
                    {
                        result.Objects = new List<object>();
                        result.Correct = false;
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;

            }

            return result;

        }

        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MpalominoProgramacionNcapasContext context = new DL.MpalominoProgramacionNcapasContext())
                {
                    var listaUsuarios = (from usuario in context.Usuarios
                                         join roldb in context.Rols on usuario.IdRol equals roldb.IdRol
                                         select new
                                         {
                                             usuario.IdUsuario,
                                             usuario.UserName,
                                             nombreUsuario = usuario.Nombre,
                                             usuario.ApellidoPaterno,
                                             usuario.ApellidoMaterno,
                                             usuario.Email,
                                             usuario.Password,
                                             usuario.Sexo,
                                             usuario.Telefono,
                                             usuario.Celular,
                                             usuario.FechaNacimiento,
                                             usuario.Curp,
                                             roldb.IdRol,
                                             nombreRol = roldb.Nombre
                                         }).ToList();//utilizado para traer todos los datos de la bd

                    result.Objects = new List<object>();

                    if (listaUsuarios != null && listaUsuarios.ToList().Count > 0)
                    {
                        foreach (var usuarioobj in listaUsuarios)
                        {
                            ML.Usuario usuario = new ML.Usuario();

                            usuario.IdUsuario = usuarioobj.IdUsuario;
                            usuario.UserName = usuarioobj.UserName;
                            usuario.Nombre = usuarioobj.nombreUsuario;
                            usuario.ApellidoPaterno = usuarioobj.ApellidoPaterno;
                            usuario.ApellidoMaterno = usuarioobj.ApellidoMaterno;
                            usuario.Email = usuarioobj.Email;
                            usuario.Password = usuarioobj.Password;
                            usuario.Sexo = usuarioobj.Sexo;
                            usuario.Telefono = usuarioobj.Telefono;
                            usuario.Celular  = usuarioobj.Celular;
                            usuario.FechaNacimiento = Convert.ToDateTime(usuarioobj.FechaNacimiento);
                            usuario.Curp = usuarioobj.Curp;

                            usuario.Rol = new ML.Rol();
                            usuario.Rol.IdRol = usuarioobj.IdRol;
                            usuario.Rol.Nombre = usuarioobj.nombreRol;

                            result.Objects.Add(usuario);

                        }

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;

            }

            return result;
        }

        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MpalominoProgramacionNcapasContext context = new DL.MpalominoProgramacionNcapasContext())

                {
                    DL.Usuario usuarioDL = new DL.Usuario();

                    usuarioDL.UserName = usuario.UserName;
                    usuarioDL.Nombre = usuario.Nombre;
                    usuarioDL.ApellidoPaterno = usuario.ApellidoPaterno;
                    usuarioDL.ApellidoMaterno = usuario.ApellidoMaterno;
                    usuarioDL.Email = usuario.Email;
                    usuarioDL.Password = usuario.Password;
                    usuarioDL.Sexo = usuario.Sexo;
                    usuarioDL.Telefono = usuario.Telefono;
                    usuarioDL.Celular = usuario.Celular;
                    usuarioDL.FechaNacimiento = usuario.FechaNacimiento;
                    usuarioDL.Curp = usuario.Curp;
                    usuarioDL.IdRol = usuario.Rol.IdRol;

                    context.Usuarios.Add(usuarioDL);
                    int RowsAffected = context.SaveChanges();

                    if (RowsAffected > 0)
                    {
                        //returna usuario
                        result.Object = usuarioDL.IdUsuario;  // int 10 boxing
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }

            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;

        }

        public static ML.Result GetById(int idUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.MpalominoProgramacionNcapasContext context = new DL.MpalominoProgramacionNcapasContext())
                {
                    var ResultQuery = (from usuario in context.Usuarios
                                       join rolDB in context.Rols on usuario.IdRol equals rolDB.IdRol

                                       where usuario.IdUsuario == idUsuario
                                       select new
                                       {
                                           usuario.IdUsuario,
                                           usuario.UserName,
                                           usuario.Nombre,
                                           usuario.ApellidoPaterno,
                                           usuario.ApellidoMaterno,
                                           usuario.Email,
                                           usuario.Password,
                                           usuario.Sexo,
                                           usuario.Telefono,
                                           usuario.Celular,
                                           usuario.FechaNacimiento,
                                           usuario.Curp,
                                           rolDB.IdRol

                                       }).FirstOrDefault();
                    //Utilizado para traer el dato de un solo usuario

                    if (ResultQuery != null)
                    {
                        result.Objects = new List<object>();
                        ML.Usuario usuario = new ML.Usuario();

                        usuario.IdUsuario = ResultQuery.IdUsuario;
                        usuario.UserName = ResultQuery.UserName;
                        usuario.Nombre = ResultQuery.Nombre;
                        usuario.ApellidoPaterno = ResultQuery.ApellidoPaterno;
                        usuario.ApellidoMaterno = ResultQuery.ApellidoMaterno;
                        usuario.Email = ResultQuery.Email;
                        usuario.Password = ResultQuery.Password;
                        usuario.Sexo = ResultQuery.Sexo;
                        usuario.Telefono = ResultQuery.Telefono;
                        usuario.Celular = ResultQuery.Celular;
                        usuario.FechaNacimiento = Convert.ToDateTime(ResultQuery.FechaNacimiento);
                        usuario.Curp = ResultQuery.Curp;

                        usuario.Rol = new ML.Rol();//INSTANCIA PARA PODER TRAER EL DATO DEL IDROL
                        usuario.Rol.IdRol = ResultQuery.IdRol;

                        result.Object = usuario;

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MpalominoProgramacionNcapasContext context = new DL.MpalominoProgramacionNcapasContext())
                {
                    var query = (from usuarioDB in context.Usuarios
                                 where usuarioDB.IdUsuario == usuario.IdUsuario
                                 select usuarioDB).SingleOrDefault();

                    if (query != null)
                    {
                        query.UserName = usuario.UserName;
                        query.Nombre = usuario.Nombre;
                        query.ApellidoPaterno = usuario.ApellidoPaterno;
                        query.ApellidoMaterno = usuario.ApellidoMaterno;
                        query.Email = usuario.Email;
                        query.Password = usuario.Password;
                        query.Sexo = usuario.Sexo;
                        query.Telefono = usuario.Telefono;
                        query.Celular = usuario.Celular;
                        query.FechaNacimiento = usuario.FechaNacimiento;
                        query.Curp = usuario.Curp;
                        query.IdRol = usuario.Rol.IdRol;

                        context.SaveChanges();

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static ML.Result Delete(int IdUsuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MpalominoProgramacionNcapasContext context = new DL.MpalominoProgramacionNcapasContext())
                {
                    var resultQuery = (from a in context.Usuarios
                                       where a.IdUsuario == IdUsuario
                                       select a).First();

                    context.Usuarios.Remove(resultQuery);

                    int RowsAffected = context.SaveChanges();

                    if (RowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }

                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
}
