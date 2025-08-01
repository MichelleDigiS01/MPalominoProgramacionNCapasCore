using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Rol
    {
        public static ML.Result GetAll() //Lista semestre
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.MpalominoProgramacionNcapasContext context = new DL.MpalominoProgramacionNcapasContext())
                {
                    var listRols = (from rolDB in context.Rols
                                    select rolDB).ToList();

                    if (listRols.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var item in listRols)
                        {
                            ML.Rol rol = new ML.Rol();
                            rol.IdRol = item.IdRol;
                            rol.Nombre = item.Nombre;

                            result.Objects.Add(rol);
                        }

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontraron registro";
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
