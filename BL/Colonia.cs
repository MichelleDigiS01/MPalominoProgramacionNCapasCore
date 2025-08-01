using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Colonia
    {
        public static ML.Result GetByIdMunicipio(int IdMunicipio)
        {
            ML.Result result = new ML.Result(); //instancia-objeto

            try
            {
                using (DL.MpalominoProgramacionNcapasContext context = new DL.MpalominoProgramacionNcapasContext())
                {

                    //select ____ from Materia //pluralize
                    var resultQuery = (from coloniaDB in context.Colonias
                                       where coloniaDB.IdMunicipio == IdMunicipio
                                       select new
                                       {
                                           coloniaDB.IdColonia,
                                           coloniaDB.Nombre,
                                           coloniaDB.CodigoPostal,
                                       }).ToList();


                    if (resultQuery.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var coloniaDB in resultQuery)
                        {
                            ML.Colonia colonia = new ML.Colonia();
                            colonia.IdColonia = coloniaDB.IdColonia;
                            colonia.Nombre = coloniaDB.Nombre;
                            colonia.CodigoPostal = coloniaDB.CodigoPostal;
                            result.Objects.Add(colonia);
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
    }
}
