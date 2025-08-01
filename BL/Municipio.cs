using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Municipio
    {
        public static ML.Result GetByIdEstado(int IdEstado)
        {
            ML.Result result = new ML.Result(); //instancia-objeto

            try
            {
                using (DL.MpalominoProgramacionNcapasContext context = new DL.MpalominoProgramacionNcapasContext())
                {

                    //select ____ from Materia //pluralize
                    var resultQuery = (from municipioDB in context.Municipios
                                       where municipioDB.IdEstado == IdEstado
                                       select new
                                       {
                                           municipioDB.IdMunicipio,
                                           municipioDB.Nombre,
                                       }).ToList();


                    if (resultQuery.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var municipioDB in resultQuery)
                        {
                            ML.Municipio municipio = new ML.Municipio();
                            municipio.IdMunicipio = municipioDB.IdMunicipio;
                            municipio.Nombre = municipioDB.Nombre;
                            result.Objects.Add(municipio);
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
