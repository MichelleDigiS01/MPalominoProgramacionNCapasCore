using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Direccion
    {
        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.MpalominoProgramacionNcapasContext context = new DL.MpalominoProgramacionNcapasContext())
                {
                    DL.Direccion direccionLINQ = new DL.Direccion();
                    direccionLINQ.Calle = usuario.Direccion.Calle;
                    direccionLINQ.NumeroExterior = usuario.Direccion.NumeroExterior;
                    direccionLINQ.NumeroInterior = usuario.Direccion.NumeroInterior;
                    direccionLINQ.IdColonia = usuario.Direccion.Colonia.IdColonia;
                    direccionLINQ.IdUsuario = usuario.IdUsuario;

                    context.Direccions.Add(direccionLINQ);
                    int rowsAffected = context.SaveChanges();

                    if (rowsAffected > 0)
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
                result.Ex = ex;
            }

            return result;
        }
    }
}
