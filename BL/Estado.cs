using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Estado
    {
        public static ML.Result GetAll()
        {

            ML.Result result = new ML.Result(); //instancia-objeto

            try
            {
                using (DL.MpalominoProgramacionNcapasContext context = new DL.MpalominoProgramacionNcapasContext())
                {

                    //select ____ from Materia //pluralize
                    var resultQuery = (from estadoDb in context.Estados
                                       select new
                                       {
                                           estadoDb.IdEstado,
                                           estadoDb.Nombre,

                                       }).ToList();


                    if (resultQuery.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var estadoDb in resultQuery)
                        {
                            ML.Estado estado = new ML.Estado();
                            estado.IdEstado = estadoDb.IdEstado;
                            estado.Nombre = estadoDb.Nombre;
                            result.Objects.Add(estado);
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
