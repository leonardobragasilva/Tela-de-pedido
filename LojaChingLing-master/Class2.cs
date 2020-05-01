using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LojaCL
{
    public class Class2
    {
        //metodo para codificar, pd usar interno, public e private
        public string Base64Encode(string textoEncode) 
        {
            var textoEncodeBytes = System.Text.Encoding.UTF8.GetBytes(textoEncode);
            return System.Convert.ToBase64String(textoEncodeBytes);
        }







    }
}
