using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.OleDb;

namespace BL
{
    public class Usuario
    {
        public static ML.Result Excel(string connectionString)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (OleDbConnection context = new OleDbConnection(connectionString))
                {
                    OleDbCommand oleDbCommand = new OleDbCommand();
                    oleDbCommand.Connection = context;
                    oleDbCommand.CommandText = "SELECT * FROM [Sheet1$]";
                    context.Open();
                    OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand);
                    DataTable dataTable = new DataTable();
                    oleDbDataAdapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            ML.Usuario usuario = new ML.Usuario();
                            usuario.Rol = new ML.Rol();


                            usuario.Nombre = row[0].ToString();
                            usuario.ApellidoPaterno = row[1].ToString();
                            usuario.ApellidoMaterno = row[2].ToString();
                            usuario.UserName = row[3].ToString();
                            usuario.Email = row[4].ToString();
                            usuario.Password = row[5].ToString();
                            usuario.Sexo = row[6].ToString();
                            usuario.Telefono = row[7].ToString();
                            usuario.Celular = row[8].ToString();
                            usuario.FechaNacimiento = row[9].ToString();
                            usuario.Curp = row[10].ToString();
                            usuario.Rol.IdRol = Convert.ToInt32(row[11]);

                            result.Objects.Add(usuario);
                        }
                        result.Correct = true;
                    }

                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.Correct = false;
            }

            return result;
        }

        public static ML.Result GetAllSP(ML.Usuario usuario)
        {

            ML.Result result = new ML.Result();
            try
            {
                using (DL.MpalominoProgramacionNcapasContext context = new DL.MpalominoProgramacionNcapasContext())
                {
                    // conexion, en donde voy a guardar la informacion , ejecutar el SP

                    var query = context.UsuarioGetAllDTO.FromSqlInterpolated($@"EXEC UsuarioGet {usuario.Nombre}, {usuario.ApellidoPaterno}, {usuario.ApellidoMaterno}, {usuario.Rol.IdRol}").ToList();

                    //var query = context.UsuarioGetAllDTO.FromSqlInterpolated($@"EXEC UsuarioGetAllView {usuario.Nombre}, {usuario.ApellidoPaterno}, {usuario.ApellidoMaterno}, {usuario.Rol.IdRol}").ToList();

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
                            usuarios.FechaNacimiento = Convert.ToString(usuarioObj.FechaNacimiento);
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

        public static ML.Result AddSP(ML.Usuario usuario)
        {

            ML.Result result = new ML.Result();
            try
            {
                using (DL.MpalominoProgramacionNcapasContext context = new DL.MpalominoProgramacionNcapasContext())
                {
                    // conexion, en donde voy a guardar la informacion , ejecutar el SP
                    // ExecuteSqlRaw   -INSERT UPDATE Y DELETE 
                    var query = context.Database.ExecuteSqlInterpolated($@"EXEC UsuarioAdd {usuario.UserName},{usuario.Nombre}, {usuario.ApellidoPaterno}, {usuario.ApellidoMaterno},{usuario.Email}, {usuario.Password}, {usuario.Sexo}, {usuario.Telefono}, {usuario.Celular}, {usuario.FechaNacimiento}, {usuario.Curp}, {usuario.Rol.IdRol}");

                    if (query > 0)
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

        public static ML.Result UpdateSP(ML.Usuario usuario)
        {

            ML.Result result = new ML.Result();
            try
            {
                using (DL.MpalominoProgramacionNcapasContext context = new DL.MpalominoProgramacionNcapasContext())
                {
                    // conexion, en donde voy a guardar la informacion , ejecutar el SP
                    // ExecuteSqlRaw   -INSERT UPDATE Y DELETE 
                    var query = context.Database.ExecuteSqlInterpolated($@"EXEC UsuarioUpdate {usuario.IdUsuario},{usuario.UserName},{usuario.Nombre}, {usuario.ApellidoPaterno}, {usuario.ApellidoMaterno},{usuario.Email}, {usuario.Password}, {usuario.Sexo}, {usuario.Telefono}, {usuario.Celular}, {usuario.FechaNacimiento}, {usuario.Curp}, {usuario.Rol.IdRol}");

                    if (query > 0)
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

        public static ML.Result GetByIdSP(int IdUsuario)
        {

            ML.Result result = new ML.Result();
            try
            {
                using (DL.MpalominoProgramacionNcapasContext context = new DL.MpalominoProgramacionNcapasContext())
                {
                    // conexion, en donde voy a guardar la informacion , ejecutar el SP

                    var query = context.UsuarioGetAllDTO.FromSqlInterpolated($@"EXEC UsuarioGetById {IdUsuario}").ToList();


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
                            usuarios.FechaNacimiento = Convert.ToString(usuarioObj.FechaNacimiento);
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

        public static ML.Result DeleteSP(int IdUsuario)
        {

            ML.Result result = new ML.Result();
            try
            {
                using (DL.MpalominoProgramacionNcapasContext context = new DL.MpalominoProgramacionNcapasContext())
                {
                    // conexion, en donde voy a guardar la informacion , ejecutar el SP

                    var query = context.Database.ExecuteSqlInterpolated($@"EXEC UsuarioDelete {IdUsuario}");

                    if (query > 0)
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
                            usuario.Celular = usuarioobj.Celular;
                            usuario.FechaNacimiento = Convert.ToString(usuarioobj.FechaNacimiento);
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
                    usuarioDL.FechaNacimiento = Convert.ToDateTime(usuario.FechaNacimiento);
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
                        usuario.FechaNacimiento = Convert.ToString(ResultQuery.FechaNacimiento);
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
                        query.FechaNacimiento = Convert.ToDateTime(usuario.FechaNacimiento);
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
